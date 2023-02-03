using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Dorion && LouisRD
    public class NewGameMenu : MonoBehaviour
    {
        private Main main;
        private HomeController homeController;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;
        private Button[] buttons;
        private Button backButton;
        private NewGameSavePanel[] savePanels;
        private SaveSystem saveSystem;
        private GameMemory gameMemory;

        private void Awake()
        {
            main = Finder.Main;

            buttons = GetComponentsInChildren<Button>();
            backButton = buttons.WithName(GameObjects.BackButton);
            
            homeController = Finder.HomeController;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
        }
        private void OnEnable()
        {
            saveSystem = Finder.SaveSystem;
            gameMemory = Finder.GameMemory;
            savePanels = GetComponentsInChildren<NewGameSavePanel>();
            backButton.onClick.AddListener(QuitToMainMenu);
            
            StartCoroutine(OnEnableRoutine());
        }        
        
        private IEnumerator OnEnableRoutine()
        {
            canvasGroup.alpha = 0;
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 1));
            homeController.SetButtonsInteractable(buttons,true);
            backButton.Select();
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveListener(QuitToMainMenu);
        }

        private void QuitToMainMenu()
        {
            StartCoroutine(QuitToMainMenuRoutine());
        }

        private IEnumerator QuitToMainMenuRoutine()
        {
            homeController.SetActiveMainMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveNewGameMenu(false);
        }

        public void ResetAllPanels(NewGameSavePanel sender)
        {
            foreach (var panel in savePanels)
            {
                if(sender != panel)
                    panel.ResetTextAndColor();
            }
        }

        public void SelectSave(SaveSlot saveSlot)
        {
            saveSystem.DeleteGameData(saveSlot);
            
            gameMemory.NewGameMemory();
            gameMemory.SaveSlot = saveSlot;

            main.GoToScene(SceneName.Tutorial);
            main.LoadGameScenes();
        }
    }
}