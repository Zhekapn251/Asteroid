using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    private const int MAX_SIZE = 50;
    private const float ENEMY_RELEASE_TIME = 15F;
    private int _initialPoolSize = 15;

    private ICoroutineService _coroutineService;
    private ObjectPool<Enemy> pool;


    private void Awake()
    {
        _coroutineService = ServiceLocator.Get<ICoroutineService>();
        pool = new ObjectPool<Enemy>(
            createFunc: InstantiateEnemy,
            actionOnGet: OnEnemyGet,
            actionOnRelease: OnEnemyRelease,
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: false,
            defaultCapacity: _initialPoolSize,
            maxSize: MAX_SIZE
        );
    }

    public Enemy GetEnemy()
    {
        return pool.Get();
    }

    public void ReturnEnemy(Enemy enemy)
    {
        pool.Release(enemy);
    }

    private void OnEnemyRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnEnemyGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        _coroutineService.StartGameCoroutine(ReturnInPool(enemy));
    }

    private IEnumerator ReturnInPool(Enemy enemy)
    {
        yield return new WaitForSeconds(ENEMY_RELEASE_TIME);
        OnEnemyRelease(enemy);
    }

    private Enemy InstantiateEnemy()
    {
        Enemy enemy = Instantiate(_enemyPrefab, gameObject.transform);
        enemy.Initialize(this);
        return enemy;
    }
}