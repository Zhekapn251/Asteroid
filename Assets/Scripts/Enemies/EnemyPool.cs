using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private int _initialPoolSize = 10;

    private ObjectPool<Enemy> pool;
    

    private void Awake()
    {
        pool = new ObjectPool<Enemy>(
            createFunc: InstantiateEnemy,
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: _initialPoolSize,
            maxSize: 20
        );
    }

    private Enemy InstantiateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab);
        enemy.Initialize(this);
        return enemy;
    }
    public Enemy GetEnemy()
    {
        return pool.Get();
    }

    public void ReturnEnemy(Enemy enemy)
    {
        pool.Release(enemy);
    }
}