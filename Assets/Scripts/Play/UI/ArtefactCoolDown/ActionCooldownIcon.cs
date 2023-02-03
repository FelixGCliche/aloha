using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Dorion
    public enum ActionsWithCooldown
    {
        Dash,
        FreezeEnemy
    }
    public class ActionCooldownIcon : MonoBehaviour
    {
        [SerializeField] private ActionsWithCooldown actionType;
        
        private float cooldownTime;
        private OnActionActivatedChannel onActionActivatedChannel;
        private OnArtefactCollectEventChannel onArtefactCollectEventChannel;
        private Image image;
        private GameMemory gameMemory;

        private void Awake()
        {
            onActionActivatedChannel = Finder.OnActionActivatedChannel;
            onArtefactCollectEventChannel = Finder.OnArtefactCollectEventChannel;
            image = GetComponent<Image>();
            image.enabled = false;
            gameMemory = Finder.GameMemory;
        }

        // Supposer être un Start() mais le Finder ne trouve pas le player à ce moment la...
        public void DisplayArtefactCooldown()
        {
            switch (actionType)
            {
                case ActionsWithCooldown.Dash:
                    cooldownTime = Finder.Player.GetComponentInChildren<Mover>().DashCooldownDuration;
                    break;
                case ActionsWithCooldown.FreezeEnemy:
                    if(gameMemory.HasCollectedArtefact(ArtefactType.Freeze))
                        OnArtefactCollect(ArtefactType.Freeze);
                    break;
            }
        }

        private void OnEnable()
        {
            image.fillAmount = 1;
            onActionActivatedChannel.OnArtefactActivated += OnActionActivation;
            onArtefactCollectEventChannel.OnArtefactCollect += OnArtefactCollect;
        }
        
        private void OnDisable()
        {
            onActionActivatedChannel.OnArtefactActivated -= OnActionActivation;
            onArtefactCollectEventChannel.OnArtefactCollect -= OnArtefactCollect;
        }

        private void OnArtefactCollect(ArtefactType type)
        {
            switch (type)
            {
                case ArtefactType.Freeze:
                    cooldownTime = Finder.Player.FreezeTriggerZone.CoolDownTime;
                    break;
            }
        }

        private void OnActionActivation(ActionsWithCooldown type)
        {
            if (type != actionType) return;

            image.enabled = true;
            
            IEnumerator OnStartCooldownRoutine()
            {
                float count = 0f;

                while (count < cooldownTime)
                {
                    count += Time.deltaTime;
                    image.fillAmount = Mathf.Lerp(0, 1, count / cooldownTime);

                    yield return null;
                }

                image.enabled = false;
            }

            StartCoroutine(OnStartCooldownRoutine());
        }
    }
}