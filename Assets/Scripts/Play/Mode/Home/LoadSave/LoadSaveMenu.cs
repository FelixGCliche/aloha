using System;
using System.Collections;
using System.Collections.Generic;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Dorion && LouisRD
    public class LoadSaveMenu : MonoBehaviour
    {
        [SerializeField] private GameObject saveGame;
        [SerializeField] private GameObject noSaveGame;
        
        private SaveSystem saveSystem;
        private Main main;
        private GameObject[] saveSpaces;
        private List<SaveDataPanel> foundSaves;
        private ConfirmSaveDelete confirmSaveDelete;
        private int numberOfSaveSlots;
        
        private Button[] buttons;
        private Button backButton;
        
        private HomeController homeController;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;

        public Button BackButton => backButton;

        private void Awake()
        {
            main = Finder.Main;
            
            confirmSaveDelete = GetComponentInChildren<ConfirmSaveDelete>();
            confirmSaveDelete.gameObject.SetActive(false);

            buttons = GetComponentsInChildren<Button>();
            backButton = buttons.WithName(GameObjects.BackButton);

            homeController = Finder.HomeController;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
        }

        private void OnEnable()
        {
            saveSystem = Finder.SaveSystem;

            foundSaves = new List<SaveDataPanel>();
            
            numberOfSaveSlots = Enum.GetNames(typeof(SaveSlot)).Length - 1;
            saveSpaces = new GameObject[numberOfSaveSlots];
            saveSpaces[0] = GameObject.Find(GameObjects.Save1);
            saveSpaces[1] = GameObject.Find(GameObjects.Save2);
            saveSpaces[2] = GameObject.Find(GameObjects.Save3);
            
            PopulateSaveSpaces();
            
            backButton.onClick.AddListener(ReturnToMainMenu);

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
            if(confirmSaveDelete.ConfirmButton != null)
                confirmSaveDelete.ConfirmButton.onClick.RemoveListener(DeleteSaveData);
            
            backButton.onClick.RemoveListener(ReturnToMainMenu);

            foreach (var foundSave in foundSaves)
            {
                foundSave.OnLoadButtonClick -= OnLoadSaveClick;
                foundSave.OnDeleteButtonClick -= OnDeleteSaveClick;
            }
            
            UnpopulateSaveSpaces();
        }

        private void PopulateSaveSpaces()
        {
            for (int i = 0; i < numberOfSaveSlots; i++)
            {
                if (FoundSave((SaveSlot)i))
                {
                    GameObject save = Instantiate(saveGame, saveSpaces[i].transform, false);
                    SaveDataPanel dataPanel = save.GetComponent<SaveDataPanel>();
                    dataPanel.SaveNumber = (SaveSlot) i + 1;
                    dataPanel.SetDataFromSave();
                    dataPanel.OnLoadButtonClick += OnLoadSaveClick;
                    dataPanel.OnDeleteButtonClick += OnDeleteSaveClick;
                    
                    foundSaves.Add(dataPanel);
                }
                else
                {
                    GameObject noSave = Instantiate(noSaveGame, saveSpaces[i].transform, false);
                }
            }
        }

        private void UnpopulateSaveSpaces()
        {
            foreach (var saveSpace in saveSpaces)
            {
                var childs = saveSpace.Children();
                foreach (var child in childs)
                {
                    Destroy(child);
                }
            }
        }

        private bool FoundSave(SaveSlot saveSlot)
        {
            foreach (var slot in saveSystem.GetExistingFiles())
                if (slot.ToString().GetNumbers() == saveSpaces[(int) saveSlot].name.GetNumbers())
                    return true;

            return false;
        }

        private void ReturnToMainMenu()
        {
            StartCoroutine(ReturnToMainMenuRoutine());
        }

        private IEnumerator ReturnToMainMenuRoutine()
        {
            homeController.SetActiveMainMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveLoadSaveMenu(false);
        }

        private void OnLoadSaveClick(SaveDataPanel saveDataPanel)
        {
            homeController.GameMemory.GameData = saveSystem.LoadGameData(saveDataPanel.SaveNumber);
            homeController.GameMemory.SaveSlot = saveDataPanel.SaveNumber;
            
            homeController.SetButtonsInteractable(buttons,false);
            main.GoToScene(homeController.GameMemory.HasCompletedLevel(SceneName.Tutorial) ? SceneName.Hub : SceneName.Tutorial);
            main.LoadGameScenes();
        }

        private void OnDeleteSaveClick(SaveDataPanel saveDataPanel)
        {
            confirmSaveDelete.gameObject.SetActive(true);
            confirmSaveDelete.SetSaveSlot(saveDataPanel.SaveNumber);
            confirmSaveDelete.ConfirmButton.onClick.AddListener(DeleteSaveData);
        }

        private void DeleteSaveData()
        {
            StartCoroutine(confirmSaveDelete.OnDeleteSaveDataRoutine());

            if (homeController.GameMemory.SaveSlot == confirmSaveDelete.SaveSlot)
            {
                homeController.GameMemory.SaveSlot = 0;
            }
            
            saveSystem.DeleteGameData(confirmSaveDelete.SaveSlot);

            foreach (var saveSpace in saveSpaces)
            {
                var childs = saveSpace.Children();
                foreach (var child in childs)
                {
                    Destroy(child);
                }
            }
            
            PopulateSaveSpaces();
        }
    }
}