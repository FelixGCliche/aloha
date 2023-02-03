using System.Collections;
using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Dorion && LouisRD
    public class ConfirmSaveDelete : MonoBehaviour
    {
        private TMP_Text confirmText;
        
        private string baseText;
        private LoadSaveMenu loadSaveMenu;
        
        private Button[] buttons;
        private Button confirmButton;
        private Button cancelButton;

        private HomeController homeController;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;

        public Button ConfirmButton => confirmButton;
        public SaveSlot SaveSlot { get; private set; }

        private void Awake()
        {
            SaveSlot = SaveSlot.None;
            confirmText = GetComponentInChildren<TMP_Text>();
            baseText = confirmText.text;
            loadSaveMenu = GetComponentInParent<LoadSaveMenu>();
            
            buttons = GetComponentsInChildren<Button>();
            confirmButton = buttons.WithName(GameObjects.ConfirmButton);
            cancelButton = buttons.WithName(GameObjects.CancelButton);

            homeController = Finder.HomeController;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
        }

        private void OnEnable()
        {
            cancelButton.onClick.AddListener(Cancel);

            StartCoroutine(OnEnableRoutine());
        }
        
        private IEnumerator OnEnableRoutine()
        {
            canvasGroup.alpha = 0;
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 1));
            homeController.SetButtonsInteractable(buttons,true);
            cancelButton.Select();
        }

        private void OnDisable()
        {
            cancelButton.onClick.RemoveListener(Cancel);
        }

        public void SetSaveSlot(SaveSlot saveSlot)
        {
            SaveSlot = saveSlot;
            confirmText.text = baseText.Format((int)saveSlot);
        }

        private void Cancel()
        {
            StartCoroutine(OnCancelRoutine());
        }

        private IEnumerator OnCancelRoutine()
        {
            homeController.SetButtonsInteractable(buttons, false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            gameObject.SetActive(false);
            loadSaveMenu.BackButton.Select();
        }

        public IEnumerator OnDeleteSaveDataRoutine()
        {
            homeController.SetButtonsInteractable(buttons, false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            gameObject.SetActive(false);
            loadSaveMenu.BackButton.Select();
        }
    }
}