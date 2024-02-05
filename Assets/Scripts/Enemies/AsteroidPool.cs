using UnityEngine;
using UnityEngine.Pool;

public class AsteroidPool: MonoBehaviour
{
    [SerializeField] private Asteroid _asteroidPrefab;
    [SerializeField] private int _initialPoolSize = 10;

    private ObjectPool<Asteroid> pool;

    private void Awake()
    {
        pool = new ObjectPool<Asteroid>(
            createFunc: InstantiateAsteroid,
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: _initialPoolSize,
            maxSize: 20
        );
    }

    private Asteroid InstantiateAsteroid()
    {
        Asteroid asteroid = Instantiate(_asteroidPrefab);
        asteroid.Initialize(this);
        return asteroid;
    }

    public Asteroid GetAsteroid()
    {
        return pool.Get();
    }

    public void ReturnAsteroid(Asteroid asteroid)
    {
        pool.Release(asteroid);
    }
}