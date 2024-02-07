using UnityEngine;
using UnityEngine.Pool;

namespace Misc
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        private int _poolSize = 100;
        private ObjectPool<Bullet> _bulletPool;
        private const int MAX_POOL_SIZE = 150;


        private void Awake()
        {
            _bulletPool = new ObjectPool<Bullet>(
                createFunc: CreateBullet,
                actionOnGet: OnGetBullet,
                actionOnRelease: OnReleaseBullet,
                actionOnDestroy: OnDestroyBullet,
                collectionCheck: false,
                defaultCapacity: _poolSize,
                maxSize: MAX_POOL_SIZE
            );
        }

        private Bullet CreateBullet()
        {
            Bullet bullet = Instantiate(_bulletPrefab, gameObject.transform, true);
            bullet.SetPool(_bulletPool);
            bullet.gameObject.SetActive(false);
            return bullet;
        }

        private void OnGetBullet(Bullet bullet) =>
            bullet.gameObject.SetActive(true);

        private void OnReleaseBullet(Bullet bullet) =>
            bullet.gameObject.SetActive(false);

        private void OnDestroyBullet(Bullet bullet) => 
            Destroy(bullet.gameObject);


        public Bullet GetBullet() =>
            _bulletPool.Get();

        public void ReturnBullet(Bullet bullet) =>
            _bulletPool.Release(bullet);
    }
}