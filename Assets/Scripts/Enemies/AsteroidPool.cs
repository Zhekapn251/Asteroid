using System.Collections;
using Services;
using UnityEngine;
using UnityEngine.Pool;

public class AsteroidPool : MonoBehaviour
{
    [SerializeField] private Asteroid _asteroidPrefab;
    private const int MAX_SIZE = 50;
    private const float ADDITIVE_TIME_COEFFICIENT = 0.2F;
    private int _initialPoolSize = 15;

    private ObjectPool<Asteroid> pool;
    private ICoroutineService _coroutineService;
    private IScreenSizeProvider _screenSizeProvider;


    private float timeToCrossScreen;
    private float currentAsteroidSpeed;

    float screenHeightInUnits;
    


    private void Awake()
    {
        InitServices();
        InitAsteroidsPool();
    }

    public Asteroid GetAsteroid()
    {
        return pool.Get();
    }

    public void ReturnAsteroid(Asteroid asteroid)
    {
        pool.Release(asteroid);
    }

    private void InitAsteroidsPool()
    {
        pool = new ObjectPool<Asteroid>(
            createFunc: InstantiateAsteroid,
            actionOnGet: OnAsteroidGet,
            actionOnRelease: OnAsteroidRelease,
            actionOnDestroy: (asteroid) => Destroy(asteroid),
            collectionCheck: false,
            defaultCapacity: _initialPoolSize,
            maxSize: MAX_SIZE
        );
    }

    private void InitServices()
    {
        _coroutineService = ServiceLocator.Get<ICoroutineService>();
        _screenSizeProvider = ServiceLocator.Get<IScreenSizeProvider>();
    }
    
    private void OnAsteroidRelease(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(false);
    }

    private void OnAsteroidGet(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        currentAsteroidSpeed = asteroid.AsteroidSpeed;
        CalculateAndSetTimeToCrossScreen();
        _coroutineService.StartGameCoroutine(ReturnInPool(asteroid));
    }

    private void CalculateAndSetTimeToCrossScreen()
    {
        screenHeightInUnits = _screenSizeProvider.ScreenWorldHeight;
        timeToCrossScreen = screenHeightInUnits / currentAsteroidSpeed;
        timeToCrossScreen += timeToCrossScreen * ADDITIVE_TIME_COEFFICIENT;
    }
    

    private IEnumerator ReturnInPool(Asteroid asteroid)
    {
        yield return new WaitForSeconds(timeToCrossScreen);
        OnAsteroidRelease(asteroid);
    }

    private Asteroid InstantiateAsteroid()
    {
        Asteroid asteroid = Instantiate(_asteroidPrefab, gameObject.transform);
        asteroid.Initialize(this);
        return asteroid;
    }
}