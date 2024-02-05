using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [Serializable]
    public class PlayerProgress
    {
        public Vector3 playerPosition = new Vector3(0, -2, 0);
        public int playerHealth = 100;
        public int playerScore = 0;
        public int currentLevel = 0;
        public int asteroidsDestroyed = 0;
        public int enemiesDestroyed = 0;
        public List<AsteroidData> asteroids;
        public List<EnemyData> enemies;
        
    }
}