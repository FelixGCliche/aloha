using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    // Author : Tout le monde
    [Findable(Tags.Player)]
    public class Player : Character, IHurtable, IHurtableTemperature, IArtefactCollector, ITemperature
    {
        [Header("Other")] 
        [SerializeField] private bool isInvincible;
        [SerializeField] private bool isSpecialFalling = false;
        [SerializeField][Tooltip("Le temps d'attente avec la caméra du niveau")]
        [Min(0f)] private float startWaitTime = 2f;

        [Header("PlayerTemperatureStats")]
        [SerializeField] [Tooltip("Montant de dégat prit par seconde")]
        private float temperatureDPS = 0.05f;
        [SerializeField] private float freezingTime = 2f;
        [SerializeField] private float freezeEffectDelay = 4f;
        [SerializeField] [Range(-1f, 0f)] private float playerTemperatureColdRate = -0.5f;
        [SerializeField] [Range(0f, 1f)] private float playerTemperatureHotRate = 0.5f;

        [Header("Movement Settings")] 
        [SerializeField] private bool doubleJumpUnlocked;
        [SerializeField] private bool canInfiniteJump;

        [Header("Audio")]
        [SerializeField] private AudioClip playerJumpSound;
        [SerializeField] private AudioClip changeTemperatureSound;
        [SerializeField] private AudioClip landingSound;

        private bool hasTouchedGroundSinceLastJump;
        private bool canDoubleJump;
        private bool hasFrozeInputs;
        private bool startScene;
        private bool lookAtPlayer;
        private bool slideToggle;
        private bool isFlipped;

        private InputAction dashInput;
        private InputActions.GameActions gameInputs;
        private InputAction grappleInput;
        private InputAction jumpInputs;
        private InputAction freezeInput;
        private InputAction switchTempStateInputs;
        private InputAction zoomOutInput;
        
        private OnPlayerDeathEventChannel onPlayerDeathEventChannel;
        private OnPlayerLifeModifiedEventChannel onPlayerLifeModifiedEventChannel;
        private OnTemperatureModifiedEventChannel onTemperatureModifiedEventChannel;
        private OnPlayerTempStateChangedEventChannel onPlayerTempStateChangedEventChannel;
        private OnActionActivatedChannel onActionActivatedChannel;

        private new Rigidbody2D rigidbody2D;
        private new CapsuleCollider2D collider;
        private FreezeTriggerZone freezeTriggerZone;
        private PlayerEffectsAnimator playerEffects;
        private PlayerMotionAnimator playerMotions;
        private GameMemory gameMemory;
        private FullLevelCamera fullLevelCamera;
        private PlayerTemperatureAura aura;
        private Grapple grapple;
        private DashSensor dashSensor;
        private UserInterface ui;
        private GameObject objectsToFlip;
        private AudioSource audioSource;
        
        private bool SlideToggle
        {
            set
            {
                slideToggle = value;
                Mover.SlideToggle = value;
            }
        }

        public bool IsInHub { get; set; }
        public TempState PlayerTemperatureState { get; private set; }
        public TemperatureStats TemperatureStats { get; private set; }
        private InputAction MoveInputs { get; set; }
        public FreezeTriggerZone FreezeTriggerZone => freezeTriggerZone;
        private float PlayerTemperatureVariationFactor
        {
            get
            {
                switch (PlayerTemperatureState)
                {
                    case TempState.Frozen:
                        return playerTemperatureColdRate;
                    case TempState.Hot:
                        return playerTemperatureHotRate;
                }

                return 0f;
            }
        }

        private new void Awake()
        {
            base.Awake();
            
            gameObject.layer = Layers.Player;

            MoveInputs = Finder.Inputs.Actions.Game.Move;
            jumpInputs = Finder.Inputs.Actions.Game.Jump;
            switchTempStateInputs = Finder.Inputs.Actions.Game.SwitchTemperatureState;
            gameInputs = Finder.Inputs.Actions.Game;
            grappleInput = Finder.Inputs.Actions.Game.Grapple;
            freezeInput = Finder.Inputs.Actions.Game.Freeze;
            dashInput = Finder.Inputs.Actions.Game.Dash;
            zoomOutInput = Finder.Inputs.Actions.Game.ZoomOut;

            rigidbody2D = GetComponent<Rigidbody2D>();
            collider = GetComponent<CapsuleCollider2D>();
            aura = GetComponentInChildren<PlayerTemperatureAura>();
            freezeTriggerZone = GetComponentInChildren<FreezeTriggerZone>();
            TemperatureStats = GetComponent<TemperatureStats>();
            fullLevelCamera = gameObject.Parent().GetComponentInChildren<FullLevelCamera>();
            grapple = GetComponentInChildren<Grapple>();
            audioSource = GetComponent<AudioSource>();

            ui = Finder.UserInterface;
            gameMemory = Finder.GameMemory;
            
            onPlayerDeathEventChannel = Finder.OnPlayerDeathEventChannel;
            onPlayerLifeModifiedEventChannel = Finder.OnPlayerLifeModifiedEventChannel;
            onTemperatureModifiedEventChannel = Finder.OnTemperatureModifiedEventChannel;
            onTemperatureModifiedEventChannel.Publish(TemperatureStats);
            onPlayerTempStateChangedEventChannel = Finder.OnPlayerTempStateChangedEventChannel;
            onActionActivatedChannel = Finder.OnActionActivatedChannel;

            GameObject dashGameObject = GameObject.Find(GameObjects.DashSensor);
            dashSensor = dashGameObject.GetComponent<DashSensor>();

            GameObject playerEffectsGo = GameObject.Find(GameObjects.PlayerEffect);
            playerEffects = playerEffectsGo.GetComponent<PlayerEffectsAnimator>();

            GameObject playerMotionsGo = GameObject.Find(GameObjects.PlayerMotionAnimator);
            playerMotions = playerMotionsGo.GetComponent<PlayerMotionAnimator>();
            
            objectsToFlip = GameObject.Find(GameObjects.Flip);
            
            slideToggle = false;
            isFlipped = false;
        }

        private new void OnEnable()
        {
            base.OnEnable();

            PlayerTemperatureState = gameMemory.LastPlayerState;
            
            startScene = false;
            lookAtPlayer = false;
            onPlayerLifeModifiedEventChannel.Publish(this);
            
            TemperatureStats.OnColdThreshold += FreezePlayer;
            TemperatureStats.OnHeatThreshold += BurnPlayer;
        }

        private void Start()
        {
            
            playerMotions.SwitchColor(PlayerTemperatureState);
            onPlayerTempStateChangedEventChannel.Publish(PlayerTemperatureState);
            
            IEnumerator Routine()
            {
                EnableTemperatureVariation(false);
                yield return new WaitForSeconds(startWaitTime);
                EnableControls();
            }

            StartCoroutine(Routine());
            
            // Tout les méthodes qui sont en lien avec cet appel son nécessaire,
            // car Finder.player ne fonctionne pas dans le Start des ArtefactCooldown
            ui.DisplayArtefactCoolDown();
        }

        private void Update()
        {
            UpdatePlayerMotionState();
            
            onPlayerLifeModifiedEventChannel.Publish(this);
            onTemperatureModifiedEventChannel.Publish(TemperatureStats);

            TemperatureStats.ObjectInfluence = PlayerTemperatureVariationFactor;

            if (!startScene)
            {
                if (MoveInputs.ReadValue<Vector2>().x > 0 || MoveInputs.ReadValue<Vector2>().x < 0)
                {
                    fullLevelCamera.StartPlayerCamera();
                    if (!IsInHub)
                        EnableTemperatureVariation(true);
                    startScene = true;
                    lookAtPlayer = true;
                }
            }

            if (zoomOutInput.triggered)
            {
                if (lookAtPlayer)
                {
                    fullLevelCamera.StartFullLevelCamera();
                    lookAtPlayer = false;
                }
                else
                {
                    fullLevelCamera.StartPlayerCamera();
                    lookAtPlayer = true;
                }
            }

            if (terrainSensor.SensedObjects.Count > 0)
            {
                Mover.TouchIce = terrainSensor.SensedObjects[0].TemperatureStats.TemperatureState == TempState.Frozen;
            }

            VerifiyUpcomingDashCollision();

            #region Controls

            bool newIsGrounded = IsGrounded;
            
            if(!Mover.IsGrounded && newIsGrounded)
                audioSource.PlayOneShot(landingSound);
            
            Mover.IsGrounded = newIsGrounded;
            
            Vector2 moveInputs = MoveInputs.ReadValue<Vector2>();

            FacingDirection = moveInputs;

            Mover.Move(moveInputs, grapple.IsGrappling);

            if (switchTempStateInputs.triggered)
                SwitchTempState();

            if (!hasTouchedGroundSinceLastJump && IsGrounded && rigidbody2D.velocity.y <= 0)
                hasTouchedGroundSinceLastJump = IsGrounded;

            if (!isSpecialFalling)
            {
                if (jumpInputs.triggered)
                {
                    if (canInfiniteJump)
                    {
                        audioSource.PlayOneShot(playerJumpSound);
                        Mover.Jump();
                        playerEffects.FireJumpEffect();
                    }
                    else if (hasTouchedGroundSinceLastJump)
                    {
                        audioSource.PlayOneShot(playerJumpSound);
                        Mover.Jump();
                        playerEffects.FireJumpEffect();
                        canDoubleJump = true;
                        hasTouchedGroundSinceLastJump = false;
                    }
                    else if (doubleJumpUnlocked && canDoubleJump)
                    {
                        audioSource.PlayOneShot(playerJumpSound);
                        Mover.DoubleJump();
                        playerEffects.FireJumpEffect();
                        canDoubleJump = false;
                    }
                }

                if (gameInputs.Slide.triggered && PlayerTemperatureState == TempState.Frozen)
                    SlideToggle = !slideToggle;

                if (dashInput.triggered)
                {
                    if (Mover.Dash(FacingDirection))
                    {
                        onActionActivatedChannel.Publish(ActionsWithCooldown.Dash);
                        playerEffects.FireDashEffect();
                        isSpecialFalling = true;
                    }
                }

                if (gameMemory.HasCollectedArtefact(ArtefactType.Grapple) && grappleInput.triggered)
                    grapple.GrappleTo();


                if (gameMemory.HasCollectedArtefact(ArtefactType.Freeze) && freezeInput.triggered &&
                    PlayerTemperatureState == TempState.Frozen)
                {
                    if (freezeTriggerZone.Freeze())
                    {
                        onActionActivatedChannel.Publish(ActionsWithCooldown.FreezeEnemy);
                        playerEffects.FireFreezeEffect();
                    }
                }
            }
            else if (IsGrounded)
            {
                isSpecialFalling = false;
            }

            #endregion

#if UNITY_EDITOR
            if (gameInputs.ActivateInvincibility.triggered)
                isInvincible = !isInvincible;
            if (gameInputs.ActivateInfiniteJump.triggered)
                canInfiniteJump = !canInfiniteJump;
#endif

            UpdateOrientationDependentComponent();
        }

        private void OnDisable()
        {
            DisableControls();

            TemperatureStats.OnColdThreshold -= FreezePlayer;
            TemperatureStats.OnHeatThreshold -= BurnPlayer;
        }

        // Author: David D
        public void CollectArtefact(ArtefactType artefactType)
        {
            switch (artefactType)
            {
                case ArtefactType.Geyser:
                {
                    gameMemory.AddArtefactToCollectedList(ArtefactType.Geyser);
                    break;
                }
                case ArtefactType.Grapple:
                {
                    grappleInput.Enable();
                    gameMemory.AddArtefactToCollectedList(ArtefactType.Grapple);
                    break;
                }
                case ArtefactType.Freeze:
                {
                    freezeInput.Enable();
                    gameMemory.AddArtefactToCollectedList(ArtefactType.Freeze);
                    break;
                }
                case ArtefactType.DashBreak:
                {
                    gameMemory.AddArtefactToCollectedList(ArtefactType.DashBreak);
                    break;
                }
                case ArtefactType.DoubleJump:
                {
                    doubleJumpUnlocked = true;
                    gameMemory.AddArtefactToCollectedList(ArtefactType.DoubleJump);
                    break;
                }
            }
        }

        //Author: Félix B
        public void Hurt(float damage)
        {
            if (isInvincible) return;

            Vitals.TakeDamage(damage);
            onPlayerLifeModifiedEventChannel.Publish(this);

            if (Vitals.IsDead)
            {
                DisableControls();
                isInvincible = true; // EMPÊCHE LE NIVEAU DE CHARGER PLUS D'UNE FOIS.
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                onPlayerDeathEventChannel.Publish();
                gameMemory.CleanData();
            }
        }

        public void DisableControls()
        {
            gameInputs.Disable();
        }

        // Author: David D
        public void EnableControls()
        {
            gameInputs.Enable();

            if (gameMemory.HasCollectedArtefact(ArtefactType.Grapple)) grappleInput.Enable();

            if (gameMemory.HasCollectedArtefact(ArtefactType.Freeze)) freezeInput.Enable();

            if (gameMemory.HasCollectedArtefact(ArtefactType.DoubleJump)) doubleJumpUnlocked = true;
        }

        // Author: Félix Bo
        private void UpdateOrientationDependentComponent()
        {
            Vector2 moveInputs = MoveInputs.ReadValue<Vector2>();
            if(!Mathf.Approximately(moveInputs.x, 0) && moveInputs.x > 0)
                FlipX(false);
            else if(!Mathf.Approximately(moveInputs.x, 0) && moveInputs.x < 0)
                FlipX(true);
        }
        
        private void FlipX(bool flipped)
        {
            if (isFlipped != flipped)
            {
                objectsToFlip.transform.Rotate(new Vector3(0,180,0));
                isFlipped = flipped;
            }
        }

        public void SetPlayerIsInvincible(bool isInvincible)
        {
            this.isInvincible = isInvincible;
        }

        public float GetLifePoints()
        {
            return Vitals.LifePoints;
        }

        // Author: Félix B
        private void VerifiyUpcomingDashCollision()
        {
            if (Mover.IsDashing)
            { 
                if (dashSensor.DashDestroyableSensor2D.SensedObjects.Count > 0)
                {
                    foreach (var destroyable in dashSensor.DashDestroyableSensor2D.SensedObjects)
                    {
                        float timeBeforeCollision = GetTimeBeforeCollision(destroyable);

                        if (!destroyable.DoorBreakingArtefactNeeded)
                        {
                            StartCoroutine(destroyable.DestructionCountDown(timeBeforeCollision));
                        }
                        else if (gameMemory.HasCollectedArtefact(ArtefactType.DashBreak))
                        {
                            StartCoroutine(destroyable.DestructionCountDown(timeBeforeCollision));
                        }
                    }
                }
            }
        }

        // Author: Félix B
        private float GetTimeBeforeCollision(IDashDestroyable destroyable)
        {
            float distance = collider.Distance(destroyable.Collider).distance;
            
            if (rigidbody2D.velocity.magnitude == 0 || distance <= 0)
                return 0;
            
            return distance / rigidbody2D.velocity.magnitude;
        }
        
        // Author: Félix B
        private void UpdatePlayerMotionState()
        {
            PlayerMotionState newState = PlayerMotionState.Idle;
                
            if ((rigidbody2D.velocity.x != 0 && MoveInputs.ReadValue<Vector2>() != Vector2.zero) || TemperatureStats.HasReachMaxTemperature)
                newState = PlayerMotionState.Running;
            
            if (Mover.IsDashing)
                newState = PlayerMotionState.Dashing;
            else if(rigidbody2D.velocity.y > 0 && !IsGrounded)
                newState = PlayerMotionState.Jumping;
            else if(!IsGrounded)
                newState = PlayerMotionState.Falling;
            else if (Vitals.IsDead)
                newState = PlayerMotionState.Dying;

            playerMotions.MotionState = newState;
        }

        public float GetInitialLifePoint()
        {
            return Vitals.InitialLifePoints;
        }

        #region Temperature

        public void HurtTemperatureFromEnemy(TempState enemyState)
        {
#if UNITY_EDITOR
            Debug.Assert(TemperatureStats != null, "playerTemperature is null on HurtTemperature");
#endif
            switch (enemyState)
            {
                case TempState.Frozen:
                    TemperatureStats.SetTemperatureToMin();
                    break;
                case TempState.Hot:
                    TemperatureStats.SetTemperatureToMax();
                    break;
            }
        }

        private void EnableTemperatureVariation(bool enableTemperatureVariation)
        {
#if UNITY_EDITOR
            Debug.Assert(TemperatureStats != null, "playerTemperature is null on EnableTemperatureVariation");
            Debug.Assert(onTemperatureModifiedEventChannel != null,
                "onTemperatureModifiedEventChannel is null on ForcePublishOnTemperatureModifiedEventChannel");
#endif
            onTemperatureModifiedEventChannel.Publish(TemperatureStats);
            TemperatureStats.enabled = enableTemperatureVariation;
        }

        // Author: David D, Félix B
        private void BurnPlayer(TemperatureStats temperatureStats)
        {
            Hurt(temperatureDPS * Time.deltaTime);

            StartCoroutine(BurnPlayerRoutine());
        }

        // Author: David D, Félix B
        private void FreezePlayer(TemperatureStats temperatureStats)
        {
            Hurt(temperatureDPS * Time.deltaTime);

            if (!hasFrozeInputs)
                StartCoroutine(FreezePlayerRoutine());
        }

        // Author: David D, Félix B
        private IEnumerator FreezePlayerRoutine()
        {
            hasFrozeInputs = true;
            DisableControls();
            yield return new WaitForSeconds(freezingTime);
            EnableControls();
            yield return new WaitForSeconds(freezeEffectDelay);
            hasFrozeInputs = false;
        }

        // Author: David D, Félix B
        private IEnumerator BurnPlayerRoutine()
        {
            while (TemperatureStats.HasReachMaxTemperature)
            {
                Mover.Move(FacingDirection);
                yield return null;
            }
        }

        // Author: David D, Félix B
        private void SwitchTempState()
        {
            audioSource.PlayOneShot(changeTemperatureSound);
            PlayerTemperatureState = PlayerTemperatureState.Next();
            onPlayerTempStateChangedEventChannel.Publish(PlayerTemperatureState);
            aura.OnPlayerStateChange();
            playerMotions.SwitchColor(PlayerTemperatureState);
            gameMemory.LastPlayerState = PlayerTemperatureState;

            if (PlayerTemperatureState == TempState.Hot)
            {
                SlideToggle = false;
            }
        }

        #endregion
    }
}