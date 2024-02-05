using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Services
{
    public class GameSaveService: IGameSaveService
    {
        public event Action OnGameSave;
        
        private const string _saveKey = "playerProgress";

        private PlayerProgress _playerProgress;

        private ISaveService _saveService;


        public GameSaveService(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void BeginSave() => 
            OnGameSave?.Invoke();

        public void SaveGame() => 
            _saveService.Save(_saveKey, _playerProgress);

        public PlayerProgress LoadGame() => 
            _saveService.Load(_saveKey, defaultValue: new PlayerProgress());


        public void SetPlayerPosition(Vector3 position) => 
            _playerProgress.playerPosition = position;

        public void SetPlayerHealth(int health) 
            => _playerProgress.playerHealth = health;

        public void SetAsteroids(List<AsteroidData> asteroidsData) => 
            _playerProgress.asteroids = asteroidsData;

        public void SetEnemies(List<EnemyData> enemiesData) 
            => _playerProgress.enemies = enemiesData;

        public void SetScore(int score) => 
            _playerProgress.playerScore = score;

        public void SetAsteroidsDestroyed(int asteroidsDestroyed) => 
            _playerProgress.asteroidsDestroyed = asteroidsDestroyed;

        public void SetEnemiesDestroyed(int enemiesDestroyed) => 
            _playerProgress.enemiesDestroyed = enemiesDestroyed;

        public void SetCurrentLevel(int level) => 
            _playerProgress.currentLevel = level;

        public PlayerProgress GetPlayerProgress() => 
            _playerProgress ??= LoadGame();
    }
}