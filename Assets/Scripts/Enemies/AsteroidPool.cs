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


    private float timeToCrossScreen;
    private float currentAsteroidSpeed;

    float screenHeightInUnits;
    private Camera _camera;


    private void Awake()
    {
        InitMainCamera();
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
    }

    private void InitMainCamera()
    {
        _camera = Camera.main;
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
        screenHeightInUnits = CalculateScreenHeightInUnits();
        timeToCrossScreen = screenHeightInUnits / currentAsteroidSpeed;
        timeToCrossScreen += timeToCrossScreen * ADDITIVE_TIME_COEFFICIENT;
    }

    private float CalculateScreenHeightInUnits()
    {
        Camera main = _camera;
        Vector3 topRightCorner = main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, main.transform.position.z));
        Vector3 bottomLeftCorner = main.ScreenToWorldPoint(new Vector3(0, 0, main.transform.position.z));
        
        float screenHeight = topRightCorner.y - bottomLeftCorner.y;
        return screenHeight;
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