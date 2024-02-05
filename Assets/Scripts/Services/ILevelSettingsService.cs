using System;
using Misc;

namespace Services
{
    public interface ILevelSettingsService
    {
        void SetLevelSettings(LevelSettings levelSettings);
        LevelSettings GetLevelSettings();

        event Action OnLevelSettingsChanged;

    }
}