using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // LouisRD
    [Findable(Tags.HomeController)]
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject newGameMenu;
        [SerializeField] private GameObject loadSaveMenu;
        [SerializeField] private GameObject achievementMenu;
        [SerializeField] private GameObject optionsMenu;

        private InputActions.MenuActions menuInputs;
        private GameMemory gameMemory;

        public GameMemory GameMemory => gameMemory;

        private void Awake()
        {
            menuInputs = Finder.Inputs.Actions.Menu;
            gameMemory = Finder.GameMemory;
        }

        private void OnEnable()
        {
            menuInputs.Enable();
            SetActiveMainMenu(true);
            SetActiveNewGameMenu(false);
            SetActiveLoadSaveMenu(false);
            SetActiveAchievementMenu(false);
            SetActiveOptionsMenu(false);
        }

        public void SetActiveMainMenu(bool active)
        {
            mainMenu.SetActive(active);
        }

        public void SetActiveNewGameMenu(bool active)
        {
            newGameMenu.SetActive(active);
        }

        public void SetActiveLoadSaveMenu(bool active)
        {
            loadSaveMenu.SetActive(active);
        }

        public void SetActiveAchievementMenu(bool active)
        {
            achievementMenu.SetActive(active);
        }

        public void SetActiveOptionsMenu(bool active)
        {
            optionsMenu.SetActive(active);
        }

        public void SetButtonsInteractable(Button[] buttons, bool interactable)
        {
            foreach (var button in buttons)
            {
                button.interactable = interactable;
            }
        }
    }
}