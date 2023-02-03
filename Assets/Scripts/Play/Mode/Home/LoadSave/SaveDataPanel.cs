using System;
using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    // Author: David Dorion
    public class SaveDataPanel : MonoBehaviour
    {
        [SerializeField] private string corruptedFile = "Corrompue";
        [SerializeField] private string dateFormat = "yyyy-MM-dd";
        [SerializeField] private string gameTimeFormat = "hh\\:mm\\:ss";
        
        private TMP_Text saveName;
        private TMP_Text gameTime;
        private TMP_Text saveDate;

        private Button[] buttons;
        private Button loadButton;
        private Button deleteButton;
        
        private SaveSystem saveSystem;
        private SaveSlot saveSlot;
        
        public SaveSlot SaveNumber
        {
            get => saveSlot;
            set => saveSlot = value;
        }

        public event OnLoadSaveEvent OnLoadButtonClick;
        public event OnDeleteSaveEvent OnDeleteButtonClick;

        private void OnEnable()
        {
            saveSystem = Finder.SaveSystem;

            TMP_Text[] textChilds = GetComponentsInChildren<TMP_Text>();
            saveName = textChilds.WithName(GameObjects.SaveName);
            gameTime = textChilds.WithName(GameObjects.SaveGameTime);
            saveDate = textChilds.WithName(GameObjects.SaveDate);
            
            buttons = GetComponentsInChildren<Button>();
            loadButton = buttons.WithName(GameObjects.Load);
            deleteButton = buttons.WithName(GameObjects.Delete);
            
            loadButton.onClick.AddListener(OnLoadBtnClick);
            deleteButton.onClick.AddListener(OnDeleteBtnClick);
        }

        private void OnDisable()
        {
            loadButton.onClick.RemoveListener(OnLoadBtnClick);
            deleteButton.onClick.RemoveListener(OnDeleteBtnClick);
        }

        public void SetDataFromSave()
        {
            GameData data = saveSystem.LoadGameData(saveSlot);
            if(data == null)
            {
                saveName.text = corruptedFile;
                loadButton.gameObject.SetActive(false);
                return;
            }

            saveName.text += " " + saveSlot;
            
            TimeSpan time = TimeSpan.FromSeconds(data.GameTimeInSeconds);
            gameTime.text = time.ToString(gameTimeFormat);

            saveDate.text = data.SaveDate.ToString(dateFormat);
        }

        private void OnLoadBtnClick()
        {
            OnLoadButtonClick?.Invoke(this);
        }
        
        private void OnDeleteBtnClick()
        {
            OnDeleteButtonClick?.Invoke(this);
        }
    }

    public delegate void OnLoadSaveEvent(SaveDataPanel saveDataPanel);
    public delegate void OnDeleteSaveEvent(SaveDataPanel saveDataPanel);
}