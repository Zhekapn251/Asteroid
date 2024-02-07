using Misc;
using Services;
using UnityEngine;

public class EnemyShoot: MonoBehaviour
{
    private const string LAYER_NAME = "EnemyBullet";
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _shootingInterval = 10f;
    [SerializeField] float _bulletSpeed = 8f;
    private float lastShootTime;
    private BulletPool bulletPool; 
    private IGameStateService gameStateService;
    private IAudioService _audioService;
    void Start()
    {
        bulletPool = FindObjectOfType<BulletPool>(); 
        gameStateService = ServiceLocator.Get<IGameStateService>();
        _audioService = ServiceLocator.Get<IAudioService>();
        lastShootTime = -_shootingInterval;
    }

    void Update()
    {
        if(gameStateService.CurrentGameState != GameState.Playing)
            return;
        if (Time.time - lastShootTime > _shootingInterval)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    void Shoot()
    {
        Bullet bullet = bulletPool.GetBullet();
        bullet.gameObject.layer = LayerMask.NameToLayer(LAYER_NAME);
        bullet.Initialize(_bulletSpawnPoint, _bulletSpeed, BulletType.Enemy);
        _audioService.PlayShootSound();
    }
}