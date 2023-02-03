using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // LouisRD
    public class MainMenu : MonoBehaviour
    {
        private Button[] buttons;
        private Button newGameButton;
        private Button loadButton;
        private Button achievementButton;
        private Button optionsButton;
        private Button quitButton;

        private HomeController homeController;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();
            newGameButton = buttons.WithName(GameObjects.NewGame);
            loadButton = buttons.WithName(GameObjects.Load);
            achievementButton = buttons.WithName(GameObjects.Achievement);
            optionsButton = buttons.WithName(GameObjects.Options);
            quitButton = buttons.WithName(GameObjects.Quit);

            homeController = Finder.HomeController;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
        }

        private void OnEnable()
        {
            newGameButton.onClick.AddListener(NewGame);
            loadButton.onClick.AddListener(LoadSaveMenu);
            achievementButton.onClick.AddListener(LoadAchievementMenu);
            optionsButton.onClick.AddListener(LoadOptionsMenu);
            quitButton.onClick.AddListener(QuitApplication);

            StartCoroutine(OnEnableRoutine());
        }


        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(NewGame);
            loadButton.onClick.RemoveListener(LoadSaveMenu);
            achievementButton.onClick.RemoveListener(LoadAchievementMenu);
            optionsButton.onClick.RemoveListener(LoadOptionsMenu);
            quitButton.onClick.RemoveListener(QuitApplication);
        }

        private void NewGame()
        {
            StartCoroutine(NewGameRoutine());
        }

        private void LoadSaveMenu()
        {
            StartCoroutine(LoadSaveMenuRoutine());
        }
        
        private void LoadAchievementMenu()
        {
            StartCoroutine(LoadAchievementMenuRoutine());
        }

        private void LoadOptionsMenu()
        {
            StartCoroutine(LoadOptionsMenuRoutine());
        }
        
        private void QuitApplication()
        {
            ApplicationExtensions.Quit();
        }
        
        private IEnumerator OnEnableRoutine()
        {
            canvasGroup.alpha = 0;
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 1));
            homeController.SetButtonsInteractable(buttons,true);
            newGameButton.Select();
        }
        
        private IEnumerator NewGameRoutine()
        {
            homeController.SetActiveNewGameMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveMainMenu(false);
        }
        
        private IEnumerator LoadSaveMenuRoutine()
        {
            homeController.SetActiveLoadSaveMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveMainMenu(false);
        }

        private IEnumerator LoadAchievementMenuRoutine()
        {
            homeController.SetActiveAchievementMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveMainMenu(false);
        }

        private IEnumerator LoadOptionsMenuRoutine()
        {
            homeController.SetActiveOptionsMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveMainMenu(false);
        }
    }
}