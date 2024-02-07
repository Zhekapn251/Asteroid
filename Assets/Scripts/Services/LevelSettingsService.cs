using System;
using Misc;
using UnityEngine;

namespace Services
{
    public class LevelSettingsService: ILevelSettingsService
    {
        public event Action OnLevelSettingsChanged;
        private LevelSettings _levelSettings;
        
        public void SetLevelSettings(LevelSettings levelSettings)
        {
            _levelSettings = levelSettings;
            OnLevelSettingsChanged?.Invoke();
        }

        public LevelSettings GetLevelSettings()
        {
           return _levelSettings;
        }
        
    }
}