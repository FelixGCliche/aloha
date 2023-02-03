using Harmony;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    //Author : William Lemelin
    public class EndMenu : MonoBehaviour
    {
        private Main main;
        private Button quitButton;

        private void Awake()
        {
            main = Finder.Main;
            var buttons = GetComponentsInChildren<Button>();
            quitButton = buttons.WithName(GameObjects.Quit);
        }

        private void OnEnable()
        {
            quitButton.Select();
            quitButton.onClick.AddListener(QuitGame);
        }

        private void OnDisable()
        {
            quitButton.onClick.RemoveListener(QuitGame);
        }

        private void QuitGame()
        {
            main.GoToHomeMenu();
        }
    }
}