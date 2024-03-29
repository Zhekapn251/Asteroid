using System.Collections;
using Services;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Pool;

namespace Misc
{
    public class Bullet : MonoBehaviour
    {
        private const float COOLDOWN_DELAY = 0.5f;
        [SerializeField] private float _maxLifetime = 2f;
        [SerializeField] private int _damage = 5;
    
        private ObjectPool<Bullet> _pool;
    
        private BulletType _bulletType;
        private SpriteRenderer _spriteRenderer;
    
        private IGameStateService _gameStateService;
        private ICoroutineService _coroutineService;
        private Coroutine _returnToPoolCoroutine;
        private float _bulletSpeed;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _coroutineService = ServiceLocator.Get<ICoroutineService>();
        }

        private void OnEnable() =>
            EnableAutoreturnBulletIntoPool();

        private void OnTriggerEnter2D(Collider2D other)
        {
            Collider2D col = GetComponent<Collider2D>();
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.TakeDamage(_damage);
            DisableAutoreturnBulletIntoPool();
            ReturnToPool();

        }

        public void SetPool(ObjectPool<Bullet> pool)
        {
            this._pool = pool; 
        }

        public void Initialize(Transform bulletSpawnPoint, float bulletSpeed, BulletType bulletType)
        {
            transform.position = bulletSpawnPoint.position;
            transform.rotation = bulletSpawnPoint.rotation;
            _bulletSpeed = bulletSpeed;
            _bulletType = bulletType;
        }

        private void Update()
        {
            if(_gameStateService.CurrentGameState != GameState.Playing)
                return;
            SetBulletMove();
        }

        private void SetBulletMove()
        {
            Vector3 movementDirection = _bulletType == BulletType.Player ? Vector3.up : Vector3.down;
            _spriteRenderer.color = _bulletType == BulletType.Player ? Color.blue : Color.red;
            transform.Translate(movementDirection * _bulletSpeed * Time.deltaTime, Space.Self);
        }

        private void DisableAutoreturnBulletIntoPool()
        {
            _coroutineService.StopGameCoroutine(_returnToPoolCoroutine);
        }

        private void EnableAutoreturnBulletIntoPool()
        {
            _returnToPoolCoroutine = _coroutineService.ExecuteAfterDelay(_maxLifetime, ReturnToPool);
        }

        private void ReturnToPool()
        {
            _pool.Release(this);
        }
    }
}