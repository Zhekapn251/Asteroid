using System.Collections;
using System.Collections.Generic;
using Entities;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner: MonoBehaviour
{
    private Vector2 _spawnRangeX;
    private Vector2 _spawnRangeY;

    private float _spawnRate;
    
    private AsteroidPool asteroidPool;
    private List<Asteroid> _asteroids = new List<Asteroid>();
    private List<AsteroidData> _asteroidsData;

    private ILevelSettingsService _levelSettingsService;
    private IGameStateService _gameStateService;
    private IGameSaveService _gameSaveService;
    private ICoroutineService _coroutineService;
    private IScreenSizeProvider _screenSizeProvider;

    private void Start()    
    {
        InitializeServices();
        _levelSettingsService.OnLevelSettingsChanged += SetSpawnRate;
        _gameSaveService.OnGameSave += SaveAsteroidsData;
        InitializeAsteroidPool();
        SetSpawnBoundaries();
        SetSpawnRate();
        SpawnLoadEnemies();
        StartEndlessAsteroidSpawning();
    }

    private void OnDestroy()
    {
        _levelSettingsService.OnLevelSettingsChanged -= SetSpawnRate;
        _gameSaveService.OnGameSave -= SaveAsteroidsData;
    }

    public void RemoveAsteroidFromList(Asteroid asteroid)
    {
        _asteroids.Remove(asteroid);
    }

    private void StartEndlessAsteroidSpawning()
    {
        _coroutineService.StartGameCoroutine(SpawnerRoutine());
    }

    private void InitializeAsteroidPool()
    {
        asteroidPool = FindObjectOfType<AsteroidPool>();
    }

    private void SetSpawnBoundaries()
    {
        float xBound = _screenSizeProvider.ScreenWorldWidth / 2;
        _spawnRangeX = new Vector2(-xBound, xBound);
        float yBound = _screenSizeProvider.ScreenWorldHeight / 2;
        _spawnRangeY = new Vector2(yBound, yBound + 1);
    }

    private void SpawnLoadEnemies()
    {
        _asteroidsData = _gameSaveService.GetPlayerProgress().asteroids;
        foreach (var asteroidData in _asteroidsData)
        {
            Asteroid asteroid = asteroidPool.GetAsteroid();
            asteroid.SetAsteroidData(asteroidData);
            _asteroids.Add(asteroid);
        }
    }

    private void SaveAsteroidsData()
    {
        _asteroidsData = new List<AsteroidData>();
        foreach (var asteroid in _asteroids)
        {
            _asteroidsData.Add(asteroid.GetAsteroidData());
        }
        _gameSaveService.SetAsteroids(_asteroidsData);
    }

    private void InitializeServices()
    {
        _levelSettingsService = ServiceLocator.Get<ILevelSettingsService>();
        _gameStateService = ServiceLocator.Get<IGameStateService>();
        _gameSaveService = ServiceLocator.Get<IGameSaveService>();
        _coroutineService = ServiceLocator.Get<ICoroutineService>();
        _screenSizeProvider = ServiceLocator.Get<IScreenSizeProvider>();
    }

    private void SetSpawnRate()
    {
        _spawnRate = _levelSettingsService.GetLevelSettings().AsteroidSpawnRate;
    }

    private IEnumerator SpawnerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            if(_gameStateService.CurrentGameState == GameState.Playing)
                SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        Asteroid asteroid = asteroidPool.GetAsteroid();

        asteroid.InitializeAsteroidSpawner(this);
        Vector3 spawnPosition = new Vector3(
            Random.Range(_spawnRangeX.x, _spawnRangeX.y),
            Random.Range(_spawnRangeY.x, _spawnRangeY.y),
            0 
        );

        asteroid.transform.position = spawnPosition;
        _asteroids.Add(asteroid);
    }
}