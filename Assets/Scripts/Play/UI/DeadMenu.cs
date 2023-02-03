using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // LouisRD
    [Findable(Tags.DeadMenu)]
    public class DeadMenu : MonoBehaviour
    {
        private Main main;
        private GameObject deadMenu;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;
        private Button quitGameButton;
        private Button retryGameButton;
        private OnPlayerDeathEventChannel onPlayerDeathEventChannel;

        private void Awake()
        {
            main = Finder.Main;
            onPlayerDeathEventChannel = Finder.OnPlayerDeathEventChannel;

            deadMenu = transform.Find(GameObjects.DeadMenu).gameObject;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
            
            var buttons = GetComponentsInChildren<Button>();
            retryGameButton = buttons.WithName(GameObjects.RetryGame);
            quitGameButton = buttons.WithName(GameObjects.QuitGame);
        }

        private void Start()
        {
            deadMenu.SetActive(false);
            canvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            retryGameButton.onClick.AddListener(Retry);
            quitGameButton.onClick.AddListener(QuitGame);
            onPlayerDeathEventChannel.OnPlayerDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            retryGameButton.onClick.RemoveListener(Retry);
            quitGameButton.onClick.RemoveListener(QuitGame);
            onPlayerDeathEventChannel.OnPlayerDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            deadMenu.SetActive(true);
            retryGameButton.interactable = false;
            quitGameButton.interactable = false;
            StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 1));
            
            IEnumerator DeadMenuActivateDelay()
            {
                yield return new WaitForSeconds(2);
                retryGameButton.interactable = true;
                retryGameButton.Select();
                quitGameButton.interactable = true;
                yield return null;
            }
            
            StartCoroutine(DeadMenuActivateDelay());
        }

        private void Retry()
        {
            var level = Finder.SceneLoadSetting.LastSceneLoaded;

            main.GoToScene(level);
            Time.timeScale = 1;
            deadMenu.SetActive(false);
            canvasGroup.alpha = 0;
        }

        private void QuitGame()
        {
            main.GoToHomeMenu();
        }
    }
}