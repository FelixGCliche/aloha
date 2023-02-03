using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // LouisRD
    public class PauseMenu : MonoBehaviour
    {
        private InputActions.GameActions gameInputs;
        private Main main;
        private GameObject pauseMenu;
        private Image backgroundImage;
        private InputActions.PauseMenuActions pauseMenuInputs;
        private Player player;
        private Button quitGameButton;
        private Button resumeGameButton;

        private bool IsPaused
        {
            get => Time.timeScale == 0;
            set
            {
                Time.timeScale = value ? 0 : 1;
                pauseMenu.SetActive(value);
                backgroundImage.enabled = value;
                if (value)
                {
                    pauseMenuInputs.Enable();
                    player.DisableControls();
                }
                else
                {
                    pauseMenuInputs.Disable();
                    player.EnableControls();
                }
            }
        }

        private void Awake()
        {
            main = Finder.Main;
            pauseMenuInputs = Finder.Inputs.Actions.PauseMenu;
            gameInputs = Finder.Inputs.Actions.Game;
            player = Finder.Player;

            pauseMenu = transform.Find(GameObjects.PauseMenu).gameObject;
            backgroundImage = gameObject.GetComponent<Image>();

            var buttons = GetComponentsInChildren<Button>();
            resumeGameButton = buttons.WithName(GameObjects.ResumeGame);
            quitGameButton = buttons.WithName(GameObjects.QuitGame);
        }

        private void Start()
        {
            IsPaused = false;
        }
        
        private void OnEnable()
        {
            resumeGameButton.onClick.AddListener(Resume);
            quitGameButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            resumeGameButton.onClick.RemoveListener(Resume);
            quitGameButton.onClick.RemoveListener(QuitGame);
        }

        private void Update()
        {
            if (gameInputs.Pause.triggered || pauseMenuInputs.Exit.triggered)
            {
                if (IsPaused)
                    Resume();
                else
                    Pause();
            }
        }

        private void Resume()
        {
            IsPaused = false;
        }

        private void Pause()
        {
            IsPaused = true;
            resumeGameButton.Select();
        }

        private void QuitGame()
        {
            IsPaused = false;
            main.GoToHomeMenu();
        }
    }
}