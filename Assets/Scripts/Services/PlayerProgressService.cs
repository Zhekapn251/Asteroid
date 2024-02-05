using System;

namespace Services
{
    public class PlayerProgressService : IPlayerProgressService
    {
        private ILevelSettingsService _levelSettingsService;
        private IUiUpdateService _uiUpdateService;
        private IGameSaveService _gameSaveService;

        public PlayerProgressService(ILevelSettingsService levelSettingsService, IUiUpdateService uiUpdateService,
            IGameSaveService gameSaveService)
        {
            _levelSettingsService = levelSettingsService;
            _uiUpdateService = uiUpdateService;
            _gameSaveService = gameSaveService;
        }

        public event Action OnWin;
        private int _currentScore;

        private int _currentLevel;
        private int _asteroidsDestroyed;
        private int _enemiesDestroyed;

        public void AddScore(int score)
        {
            _currentScore += score;
            UpdateScore();
        }

        private void UpdateScore() => 
            _uiUpdateService.ScoreChanged(_currentScore);

        public void OnAsteroidDestroyed() => 
            _asteroidsDestroyed++;

        public void EnemyDestroyed() => 
            _enemiesDestroyed++;

        public void LoadProgress()
        {
            _currentLevel = _gameSaveService.GetPlayerProgress().currentLevel;
            _currentScore = _gameSaveService.GetPlayerProgress().playerScore;
            _asteroidsDestroyed = _gameSaveService.GetPlayerProgress().asteroidsDestroyed;
            _enemiesDestroyed = _gameSaveService.GetPlayerProgress().enemiesDestroyed;
        }

        public void SaveProgress()
        {
            _gameSaveService.SetCurrentLevel(_currentLevel);
            _gameSaveService.SetScore(_currentScore);
            _gameSaveService.SetAsteroidsDestroyed(_asteroidsDestroyed);
            _gameSaveService.SetEnemiesDestroyed(_enemiesDestroyed);
        }

        public bool IsLevelComplete()
        {
            bool isLevelComplete = CheckLevelCondition();

            if (isLevelComplete)
            {
                OnWin?.Invoke();
                ResetProgress();
            }

            return isLevelComplete;
        }

        public void ResetProgress()
        {
            _asteroidsDestroyed = 0;
            _enemiesDestroyed = 0;
        }

        public void ResetScore() =>
            _currentScore = 0;


        public int CurrentScore => 
            _currentScore;

        private bool CheckLevelCondition() =>
            _asteroidsDestroyed >= 
            _levelSettingsService.GetLevelSettings().AsteroidsToDestroy &&
            _enemiesDestroyed >= 
            _levelSettingsService.GetLevelSettings().EnemiesToDestroy;
    }
}