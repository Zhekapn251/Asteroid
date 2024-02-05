using System;
using Entities;

namespace Services
{
    public interface ISettingsService
    {
        event Action OnAudioSettingsChanged;
        void SaveAudioSettings(AudioFXSettings fxSettings);
        AudioFXSettings LoadAudioSettings();
    }
}