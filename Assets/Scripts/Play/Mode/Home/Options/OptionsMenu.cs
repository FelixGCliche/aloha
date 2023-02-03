using System.Collections;
using System.Collections.Generic;
using Harmony;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Game
{
    // David Dorion && LouisRD
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private string volumeVariableName = "Volume";
        [SerializeField] private string baseVolumeText = "{0}";
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private int minVolume = -80;
        [SerializeField] private int maxVolume = 20;

        private Button[] buttons;
        private Button backButton;
        private TMP_Text volumeValueText;
        private Slider volumeSlider;
        private TMP_Dropdown resolutionDropdown;
        private Toggle fullscreenToggle;
        private OptionData optionData;
        private HomeController homeController;
        private CanvasGroup canvasGroup;
        private CanvasGroupFader canvasGroupFader;

        private void Awake()
        {
            homeController = Finder.HomeController;
            
            buttons = GetComponentsInChildren<Button>();
            backButton = buttons.WithName(GameObjects.BackButton);

            volumeValueText = GetComponentsInChildren<TMP_Text>().WithName(GameObjects.VolumeValue);
            volumeSlider = GetComponentInChildren<Slider>();
            resolutionDropdown = GetComponentInChildren<TMP_Dropdown>();
            fullscreenToggle = GetComponentInChildren<Toggle>();

            volumeSlider.minValue = minVolume;
            volumeSlider.maxValue = maxVolume;

            optionData = homeController.GameMemory.OptionData;

            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            canvasGroupFader = gameObject.GetComponent<CanvasGroupFader>();

            SetOptionsFromData();
        }

        private void OnEnable()
        {
            backButton.onClick.AddListener(QuitToMainMenu);
            volumeSlider.onValueChanged.AddListener(SetVolume);
            fullscreenToggle.onValueChanged.AddListener(UpdateFullscreen);
            resolutionDropdown.onValueChanged.AddListener(SetResolution);

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
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
            fullscreenToggle.onValueChanged.RemoveListener(UpdateFullscreen);
            resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
        }

        private void QuitToMainMenu()
        {
            StartCoroutine(QuitToMainMenuRoutine());
        }

        private IEnumerator QuitToMainMenuRoutine()
        {
            homeController.GameMemory.OptionData = optionData;
            homeController.SetActiveMainMenu(true);
            homeController.SetButtonsInteractable(buttons,false);
            yield return StartCoroutine(canvasGroupFader.FadeRoutine(canvasGroup, canvasGroup.alpha, 0));
            homeController.SetActiveOptionsMenu(false);
        }

        private void SetResolution(int value)
        {
            Resolution res = homeController.GameMemory.Resolutions[value];
            optionData.width = res.width;
            optionData.height= res.height;
            optionData.refreshRate = res.refreshRate;
            Screen.SetResolution(res.width,res.height,fullscreenToggle.isOn,res.refreshRate);
        }

        private void SetVolume(float volume)
        {
            optionData.volume = volume.RoundToInt();
            float volumePercent = volume  + -minVolume;
            volumeValueText.text = baseVolumeText.Format(volumePercent.RoundToInt().ToString());
            audioMixer.SetFloat(volumeVariableName, volume);
        }

        private void UpdateFullscreen(bool isFullscreen)
        {
            optionData.fullscreen = isFullscreen;
            Screen.fullScreen = isFullscreen;
        }

        private void SetOptionsFromData()
        {
            volumeSlider.value = optionData.volume;
            SetVolume(volumeSlider.value);

            fullscreenToggle.isOn = optionData.fullscreen;
            
            resolutionDropdown.ClearOptions();
            
            var resolutionOptions = new List<string>();

            var resolutions = homeController.GameMemory.Resolutions;
            
            int currentResolution = 0;
            for (int i = 0; i<resolutions.Length ; i++)
            {
                resolutionOptions.Add(resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate + "hz");
                
                if(optionData.width == 0 || optionData.height == 0 || optionData.refreshRate == 0)
                {
                    if (resolutions[i].width == Screen.currentResolution.width &&
                        resolutions[i].height == Screen.currentResolution.height)
                        currentResolution = i;
                }
                else if (resolutions[i].width == optionData.width &&
                         resolutions[i].height == optionData.height &&
                         resolutions[i].refreshRate == optionData.refreshRate)
                {
                    currentResolution = i;
                }
            }

            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = currentResolution;
            resolutionDropdown.RefreshShownValue();
        }
    }
}