using System;
using System.Collections;
using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : William Lemelin && LouisRD
    public class AchievementMenu : MonoBehaviour
    {
        [SerializeField] private AchievementData[] achievements = { };
        [SerializeField] private GameObject achievementPrefabs;
        [SerializeField] private GameObject achievementContainer;

        private Button[] buttons;
        private Button quitButton;
        
        private HomeController homeController;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>();
            quitButton = buttons.WithName(GameObjects.Quit);

            homeController = Finder.HomeController;
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();
        }

        private void Start()
        {
            foreach (var achievement in achievements)
            {
                var achievementView = Instantiate(achievementPrefabs,achievementContainer.transform,false).GetComponent<AchievementView>();

                achievementView.Title = achievement.Title;
                achievementView.Description = achievement.Description;
                achievementView.Image = achievement.Image;
                achievementView.IsUnlocked = homeController.GameMemory.HasUnlockedAchievement(achievement.Type);
            }
        }

        private void OnEnable()
        {
            quitButton.onClick.AddListener(QuitToMainMenu);

            StartCoroutine(OnEnableRoutine());
        }
        
        private IEnumerator OnEnableRoutine()
        {
            canvasGroup.alpha = 0;
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 1));
            homeController.SetButtonsInteractable(buttons,true);
            quitButton.Select();
        }

        private void OnDisable()
        {
            quitButton.onClick.RemoveListener(QuitToMainMenu);
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
            homeController.SetActiveAchievementMenu(false);
        }
    }

    [Serializable]
    public struct AchievementData
    {
        public AchievementType Type;
        public string Title;
        public string Description;
        public Sprite Image;
    }
}