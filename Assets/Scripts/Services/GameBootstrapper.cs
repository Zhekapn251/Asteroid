using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services
{
    public class GameBootstrapper: MonoBehaviour
    {
        private void Awake()
        {
            IAudioService audioManager = FindObjectOfType<AudioManager>();
            ServiceLocator.Register<IAudioService>(audioManager);
        
            ISaveService saveService = new PlayerPrefsSaveService();
            ServiceLocator.Register<ISaveService>(saveService);
       
            IUiUpdateService uiUpdateService = new UiUpdateService();
            ServiceLocator.Register<IUiUpdateService>(uiUpdateService);

            IGameStateService gameStateService = FindObjectOfType<GameStateService>();
            ServiceLocator.Register<IGameStateService>(gameStateService);
        
            ISettingsService settingsService = new PlayerPrefsSettingsService(saveService);
            ServiceLocator.Register<ISettingsService>(settingsService);
        
            ILevelSettingsService levelSettings =  new LevelSettingsService();
            ServiceLocator.Register<ILevelSettingsService>(levelSettings);

            IGameSaveService gameSaveService = new GameSaveService(saveService);
            ServiceLocator.Register<IGameSaveService>(gameSaveService);
        
            IPlayerProgressService playerProgressService = new PlayerProgressService(levelSettings, uiUpdateService, gameSaveService);
            ServiceLocator.Register<IPlayerProgressService>(playerProgressService);

            SceneManager.LoadScene("Game");

        }
    }
}