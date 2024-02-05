using Entities;
using Misc;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
        [SerializeField] private Sprite[] _sprites;

        private IPlayerProgressService _playerProgressService;
        
        private EnemyData _enemyData;
        private SpriteRenderer _spriteRenderer;

        private EnemyPool _enemyPool;
        private EnemySpawner _enemySpawner;

        private Death _death;
        private Health _health;

        int _spriteIndex;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerProgressService = ServiceLocator.Get<IPlayerProgressService>();
            
            _death = GetComponent<Death>();
            _health = GetComponent<Health>();

        }

        private void Start()
        {
            _death.OnRevive += Death_OnRevive;
            _death.OnDie += Death_OnDie;
        }

        private void OnDestroy()
        {
            _death.OnRevive -= Death_OnRevive;
            _death.OnDie -= Death_OnDie;
        }

        public void Initialize(EnemyPool enemyPool)
        {
            _spriteIndex = RandomSpriteIndex();
            _spriteRenderer.sprite = _sprites[_spriteIndex];
            _enemyPool = enemyPool;

        }
        
        public void InitializeSpawner(EnemySpawner enemySpawner) => 
            _enemySpawner = enemySpawner;

        private int RandomSpriteIndex() => 
           Random.Range(0, _sprites.Length);
    

        public EnemyData GetEnemyData() => 
            new EnemyData(transform.position, _health.currentHealth, _spriteIndex);


        private void Death_OnRevive()
        {
           _enemyPool.ReturnEnemy(this);
           _enemySpawner?.RemoveEnemy(this);
        }

        private void Death_OnDie()
        {
            _playerProgressService.EnemyDestroyed();
            _playerProgressService.AddScore(10);
            _playerProgressService.IsLevelComplete();
        }

        public void SetEnemyData(EnemyData enemyData1)
        {
            _enemyData = enemyData1;
            transform.position = _enemyData.position;
            _spriteRenderer.sprite = _sprites[_enemyData.spriteIndex];
            _health.currentHealth = _enemyData.health;
        }
}