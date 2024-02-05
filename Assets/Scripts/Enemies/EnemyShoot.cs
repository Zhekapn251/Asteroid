using Misc;
using Services;
using UnityEngine;

public class EnemyShoot: MonoBehaviour
{
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private float _shootingInterval = 2.0f;
    [SerializeField] float _bulletSpeed = 10f;
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
        bullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullet");
        bullet.Initialize(_bulletSpawnPoint, _bulletSpeed, Bullet.BulletType.Enemy);

        bullet.transform.position = _bulletSpawnPoint.position;
        bullet.transform.rotation = _bulletSpawnPoint.rotation;
        _audioService.PlayShootSound();
        
    }
}