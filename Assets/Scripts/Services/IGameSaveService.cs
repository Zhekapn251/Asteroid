using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

namespace Services
{
    public interface IGameSaveService
    {
        event Action OnGameSave;
        void BeginSave();
        void SaveGame();
        PlayerProgress LoadGame();

        public void SetPlayerPosition(Vector3 position);

        public void SetPlayerHealth(int health);

        public void SetAsteroids(List<AsteroidData> asteroidsData);

        public void SetEnemies(List<EnemyData> enemiesData);
        
        public void SetScore(int score);
        public void SetAsteroidsDestroyed(int asteroidsDestroyed);
        public void SetEnemiesDestroyed(int enemiesDestroyed);
        
        public void SetCurrentLevel(int level);
        
        public PlayerProgress GetPlayerProgress();
    }
}