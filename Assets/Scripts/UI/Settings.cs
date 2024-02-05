using System;
using Entities;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Settings: MonoBehaviour
    {
        [SerializeField] private Slider _audioVolumeSlider;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Button _closeButton;

        private ISettingsService settingsService;
        private IGameStateService gameStateService;
         
        private AudioFXSettings audioFXSettings;
        
        
        private void Awake()
        {
            settingsService = ServiceLocator.Get<ISettingsService>();
            gameStateService = ServiceLocator.Get<IGameStateService>();
            LoadSettings();
            _audioVolumeSlider.value = audioFXSettings.sfxVolume;
            _musicVolumeSlider.value = audioFXSettings.musicVolume;
            _audioVolumeSlider.onValueChanged.AddListener(OnAudioVolumeChanged);
            _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
        

        private void OnEnable()
        {
            gameStateService.CurrentGameState = GameState.Paused;
        }

        private void OnDisable()
        {
            gameStateService.CurrentGameState = GameState.Playing;
        }

        private void OnMusicVolumeChanged(float arg0)
        {
            audioFXSettings.musicVolume = arg0;
            SaveSettings();
        }

        private void OnAudioVolumeChanged(float arg0)
        {
            audioFXSettings.sfxVolume = arg0;
            SaveSettings();
        }

        private void SaveSettings() => 
            settingsService.SaveAudioSettings(audioFXSettings);

        private void LoadSettings() => 
            audioFXSettings = settingsService.LoadAudioSettings();
    }
}