using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;
using UnityEngine;

namespace Services
{
    public class GameSaveService: IGameSaveService
    {
        public event Action OnGameSave;
        private int _pendingSaves;
        private bool _isSaveRequested;

        private const string _saveKey = "playerProgress";

        private PlayerProgress _playerProgress;

        private ISaveService _saveService;


        public GameSaveService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void BeginSave()
        {
            OnGameSave?.Invoke();
        }

        public void SaveGame()
        {
            if (_pendingSaves == 0)
            {
                PerformSave();
            }
            else
            {
                _isSaveRequested = true;
            }
            
        }

        public PlayerProgress LoadGame() =>
            _saveService.Load(_saveKey, defaultValue: new PlayerProgress());


        public PlayerProgress GetPlayerProgress() =>
            _playerProgress ??= LoadGame();

        public void SetPlayerPosition(Vector3 position)
        {
            RegisterPendingSave();
            _playerProgress.playerPosition = position;
            NotifySaveComplete();
        }

        public void SetPlayerHealth(int health)
        {
            RegisterPendingSave();
            _playerProgress.playerHealth = health;
            NotifySaveComplete();
        }

        public void SetAsteroids(List<AsteroidData> asteroidsData)
        {
            RegisterPendingSave();
            _playerProgress.asteroids = asteroidsData;
            NotifySaveComplete();
        }


        public void SetEnemies(List<EnemyData> enemiesData)
        {
            RegisterPendingSave();
            _playerProgress.enemies = enemiesData;
            NotifySaveComplete();
        }

        public void SetScore(int score)
        {
            RegisterPendingSave();
            _playerProgress.playerScore = score;
            NotifySaveComplete();
        }

        public void SetAsteroidsDestroyed(int asteroidsDestroyed)
        {
            RegisterPendingSave();
            _playerProgress.asteroidsDestroyed = asteroidsDestroyed;
            NotifySaveComplete();
        }

        public void SetEnemiesDestroyed(int enemiesDestroyed)
        {
            RegisterPendingSave();
            _playerProgress.enemiesDestroyed = enemiesDestroyed;
            NotifySaveComplete();
        }

        public void SetCurrentLevel(int level)
        {
            RegisterPendingSave();
            _playerProgress.currentLevel = level;
            NotifySaveComplete();
        }

        private void RegisterPendingSave()
        {
            _pendingSaves++;
        }

        private void NotifySaveComplete()
        {
            _pendingSaves--;
            
            if (_pendingSaves == 0 && _isSaveRequested)
            {
                PerformSave();
            }
        }

        private void PerformSave()
        {
            _saveService.Save(_saveKey, _playerProgress);
            _pendingSaves = 0;
            _isSaveRequested = false;
        }
        
        public async Task StartLongProcessAsync(int delayIn_ms)
        {
            await Task.Delay(delayIn_ms);
            NotifySaveComplete();
        }

        
    }
}