using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Dorion && LouisRD
    public class NewGameSavePanel : MonoBehaviour
    {
        [SerializeField] private SaveSlot saveSlot = SaveSlot.Slot1;
        [SerializeField] private string defaultString = "Vide";
        [SerializeField] private string hasSaveString = "Sauvegarde existante";
        [SerializeField] private string overrideString = "Écraser?";
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color overrideColor;
        [SerializeField] private Color overrideTextColor = Color.red;
        [SerializeField] private Color normalTextColor = Color.black;
        
        private Button button;
        private TMP_Text saveNameText;
        private SaveSystem saveSystem;
        private NewGameMenu newGameMenu;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            saveSystem = Finder.SaveSystem;
            newGameMenu = GetComponentInParent<NewGameMenu>();
            saveNameText = GetComponentsInChildren<TMP_Text>().WithName(GameObjects.SaveNameText);
            
            ResetTextAndColor();
            
            button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void ResetToDefaultText()
        {
            if (saveSystem.LoadGameData(saveSlot) != null)
                saveNameText.text = hasSaveString;
        }

        public void ResetTextAndColor()
        {
            var buttonColors = button.colors;
            buttonColors.normalColor = normalColor;
            button.colors = buttonColors;
            
            ResetToDefaultText();
            
            saveNameText.color = normalTextColor;
        }

        private void OnClick()
        {
            newGameMenu.ResetAllPanels(this);
            
            if (saveNameText.text == hasSaveString)
            {
                var buttonColors = button.colors;
                buttonColors.normalColor = overrideColor;
                button.colors = buttonColors;

                saveNameText.text = overrideString;
                saveNameText.color = overrideTextColor;
            }
            else if(saveNameText.text == overrideString || saveNameText.text == defaultString)
            {
               newGameMenu.SelectSave(saveSlot);
            }
        }
    }
}