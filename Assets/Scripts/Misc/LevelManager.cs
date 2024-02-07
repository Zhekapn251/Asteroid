using System.Collections;
using Services;
using UnityEngine;

namespace Misc
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelSettings[] _levels;

        private IPlayerProgressService _playerProgressService;
        private IGameStateService _gameStateService;
        private IUiUpdateService _uiUpdateService;
        private IGameSaveService _gameSaveService;
        private ILevelSettingsService _levelSettingsService;
        private ICoroutineService _coroutineService;

        private int _currentLevelIndex;
 
        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        void Start()
        {
            _coroutineService = ServiceLocator.Get<ICoroutineService>();
            InitializeServices();
       
            _playerProgressService.OnWin += PlayerProgressService_OnWin;
            _gameStateService.OnLevelWin += GameStateServiceOnLevelWin;
            _gameStateService.OnGameReload += GameStateServiceOnGameReload;
            _gameSaveService.OnGameSave += GameStateServiceOnGameSave;

            _currentLevelIndex = _gameSaveService.GetPlayerProgress().currentLevel;
            LoadLevel(_currentLevelIndex);
            _coroutineService.StartGameCoroutine(SaveRoutine());
        }

        private void OnDestroy()
        {
            _playerProgressService.OnWin -= PlayerProgressService_OnWin;
            _gameStateService.OnLevelWin -= GameStateServiceOnLevelWin;
            _gameStateService.OnGameReload -= GameStateServiceOnGameReload;
            _gameSaveService.OnGameSave -= GameStateServiceOnGameSave;
        }

        private void InitializeServices()
        {
            _playerProgressService = ServiceLocator.Get<IPlayerProgressService>();
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _uiUpdateService = ServiceLocator.Get<IUiUpdateService>();
            _gameSaveService = ServiceLocator.Get<IGameSaveService>();
            _levelSettingsService = ServiceLocator.Get<ILevelSettingsService>();
        }

        private void GameStateServiceOnGameSave()
        {
            _gameSaveService.SetCurrentLevel(_currentLevelIndex);
            _playerProgressService.SaveProgress();
        }


        private void GameStateServiceOnLevelWin()
        {
            _currentLevelIndex++;
            _playerProgressService.ResetProgress();
            LoadLevel(_currentLevelIndex, true);
        }

        private void GameStateServiceOnGameReload()
        {
            _currentLevelIndex = 0;
            _playerProgressService.ResetProgress();
            _playerProgressService.ResetScore();
            LoadLevel(0, true);
        }

        private void PlayerProgressService_OnWin()
        {
            _uiUpdateService.Win();
        }

        private void LoadLevel(int levelIndex, bool isReload = false)
        {
            _uiUpdateService.LevelChanged(levelIndex + 1);

            if(!isReload)
                _playerProgressService.LoadProgress();     
        
            _uiUpdateService.ScoreChanged(_playerProgressService.CurrentScore); ;
        
            if (NotValidLevel(levelIndex))
            {
                Debug.LogError("Level index out of bounds");
                return;
            }

            _currentLevelIndex = levelIndex;
            ApplyLevelSettings(_levels[levelIndex]);
        }

        private IEnumerator SaveRoutine()
        {
            yield return new WaitForSeconds(IGameSaveService.AUTO_SAVE_INTERVAL);
            Debug.Log("SaveRoutine started");
            _gameSaveService.BeginSave();
            _gameSaveService.SaveGame();
        }

        private bool NotValidLevel(int levelIndex) => 
            levelIndex < 0 || levelIndex >= _levels.Length;

        private void ApplyLevelSettings(LevelSettings settings) =>
            _levelSettingsService.SetLevelSettings(settings);
    }
}