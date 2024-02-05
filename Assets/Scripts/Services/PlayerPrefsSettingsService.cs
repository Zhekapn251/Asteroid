using System;
using Entities;

namespace Services
{
    public class PlayerPrefsSettingsService: ISettingsService
    {
        private const string AudioSettingsKey = "AudioSettings";
        private ISaveService _saveService;
        
        public PlayerPrefsSettingsService(ISaveService saveService)
        {
            _saveService = saveService;
        }


        public event Action OnAudioSettingsChanged;

        public void SaveAudioSettings(AudioFXSettings fxSettings)
        {
            _saveService.Save(AudioSettingsKey, fxSettings);
            OnAudioSettingsChanged?.Invoke();
        }

        public AudioFXSettings LoadAudioSettings()
        {
            return _saveService.Load(AudioSettingsKey, new AudioFXSettings());
        }
    }

}