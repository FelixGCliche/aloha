using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Harmony;
using Application = UnityEngine.Application;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace Game
{
    // Author: David Dorion
    public enum SaveSlot
    {
        None = 0,
        Slot1 = 1,
        Slot2 = 2,
        Slot3 = 3
    }
    
    [Findable(Tags.MainController)]
    public class SaveSystem : MonoBehaviour
    {
        [SerializeField] private string saveFolderPath = "Saves";
        [SerializeField] private string saveFileExtension = "data";
        [SerializeField] private string defaultFileName = "save";
        [SerializeField] private string optionFileName = "options.json";

        private string OptionFolder => Application.persistentDataPath;
        private string OptionFilePath => Path.Combine(OptionFolder, optionFileName);
        private string SaveFilePath => Path.Combine(Application.persistentDataPath,saveFolderPath);

        private string GetSaveFilePath(SaveSlot saveSlot)
        {
            return Path.Combine(SaveFilePath ,$"{defaultFileName}{(int)saveSlot}.{saveFileExtension}");
        }
        
        public void SaveGameData(GameData data, SaveSlot saveSlot)
        {
            CreateFolderIfNotExistant(SaveFilePath);
            
            string json = data.ToJson();
            using (StreamWriter file = File.CreateText(GetSaveFilePath(saveSlot)))
            {
                 file.Write(json);
            }
        }

        public GameData LoadGameData(SaveSlot saveSlot)
        {
            CreateFolderIfNotExistant(SaveFilePath);
            
            string filePath = GetSaveFilePath(saveSlot);
            
            if (!File.Exists(filePath)) return null;
            
            using (StreamReader file = File.OpenText(filePath))
            {
                try
                {
                    return JsonUtility.FromJson<GameData.GameDataJson>(file.ReadToEnd()).FromJson();
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    Debug.LogError(e.Message);
#endif
                    throw;
                }
            }
        }

        public void DeleteGameData(SaveSlot saveSlot)
        {
            if (!File.Exists(GetSaveFilePath(saveSlot))) return;
            
            File.Delete(GetSaveFilePath(saveSlot));
        }

        public List<SaveSlot> GetExistingFiles()
        {
            CreateFolderIfNotExistant(SaveFilePath);
            
            List<SaveSlot> slots = new List<SaveSlot>();
            
            var info = new DirectoryInfo(SaveFilePath);
            var fileInfo = info.GetFiles();
            
            foreach (var file in fileInfo)
            {
                // Le nom du fichier est transformé pour laisser le numéro de sauvegarde du fichier
                string fileNumber = file.Name.RemoveFileType().Remove(0,saveFileExtension.Length);
                int.TryParse(fileNumber,out var i);
                slots.Add((SaveSlot)i);
            }

            return slots;
        }

        private void CreateFolderIfNotExistant(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
        
        public void SaveOptionData(OptionData data)
        {
            CreateFolderIfNotExistant(OptionFolder);

            string json = JsonUtility.ToJson(data);
            using (StreamWriter file = File.CreateText(OptionFilePath))
            {
                file.Write(json);
            }
        }

        public OptionData LoadOptionData()
        {
            CreateFolderIfNotExistant(OptionFolder);
    
            if (!File.Exists(OptionFilePath)) return null;
            
            using (StreamReader file = File.OpenText(OptionFilePath))
            {
                try
                {
                    return JsonUtility.FromJson<OptionData>(file.ReadToEnd());
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    Debug.LogError(e.Message);
#endif
                    return null;
                }
            }
        }
    }
}
