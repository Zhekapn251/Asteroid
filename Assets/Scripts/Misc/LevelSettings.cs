using UnityEngine;

namespace Misc
{
    [CreateAssetMenu(fileName = "NewLevelSettings", menuName = "Level Settings", order = 51)]
    public class LevelSettings : ScriptableObject
    {
        [SerializeField]
        private int levelNumber;
        [SerializeField]
        private float enemySpawnRate;
        [SerializeField]
        private float asteroidSpawnRate;
        [SerializeField]
        private int enemiesToDestroy;
        [SerializeField]
        private int asteroidsToDestroy;
        [SerializeField] 
        private int scoreToGet3Stars;
    
    
        public int LevelNumber => levelNumber;
        public float EnemySpawnRate => enemySpawnRate;
        public float AsteroidSpawnRate => asteroidSpawnRate;
        public int EnemiesToDestroy => enemiesToDestroy;
        public int AsteroidsToDestroy => asteroidsToDestroy;
    
        public int ScoreToGet3Stars => scoreToGet3Stars;
    }
}