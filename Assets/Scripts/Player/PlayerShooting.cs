using Misc;
using Services;
using UnityEngine;

namespace Player
{
    public class PlayerShooting : MonoBehaviour
    {
        private const string LAYER_NAME = "Bullet";
        [SerializeField] private Transform _bulletSpawnPoint;
        [SerializeField] private float _bulletSpeed = 10f;
        [SerializeField] private float _shootingInterval = 0.1f;
    
        private InputService _inputService;
        private BulletPool _bulletPool;
    
        private float lastShootTime;
        private IGameStateService gameStateService;
        private IAudioService _audioService;

        private void Awake()
        {
            _inputService = new InputService();
            _bulletPool = FindObjectOfType<BulletPool>();
            gameStateService = ServiceLocator.Get<IGameStateService>();
            _audioService = ServiceLocator.Get<IAudioService>();
        }

        private void Update()
        {
            if(gameStateService.CurrentGameState != GameState.Playing)
                return;
            if (_inputService.IsFirePressed() && Time.time > lastShootTime + _shootingInterval)
            {
                lastShootTime = Time.time;
                Shoot();
            }
        }

        private void Shoot()
        {
            Bullet bullet = _bulletPool.GetBullet();
            bullet.gameObject.layer = LayerMask.NameToLayer(LAYER_NAME); 
            bullet.Initialize(_bulletSpawnPoint, _bulletSpeed, BulletType.Player);
            _audioService.PlayShootSound();
        }


    }
}
