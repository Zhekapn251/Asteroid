using System.Collections;
using System.Collections.Generic;
using Entities;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner: MonoBehaviour
{
    private Vector2 _spawnRangeX = new Vector2(-3, 3);
    private Vector2 _spawnRangeY = new Vector2(5, 6);

    private float _spawnRate;
    
    private AsteroidPool asteroidPool;
    private List<Asteroid> _asteroids = new List<Asteroid>();
    private List<AsteroidData> _asteroidsData;

    private ILevelSettingsService _levelSettingsService;
    private IGameStateService _gameStateService;
    private IGameSaveService _gameSaveService;
    private ICoroutineService _coroutineService;

    private void Start()    
    {
        _coroutineService = ServiceLocator.Get<ICoroutineService>();
        asteroidPool = FindObjectOfType<AsteroidPool>();
        InitializeServices();
        _levelSettingsService.OnLevelSettingsChanged += SetSpawnRate;
        _gameSaveService.OnGameSave += SaveAsteroidsData;
        SetSpawnRate();
        SpawnLoadEnemies();
        _coroutineService.StartGameCoroutine(SpawnerRoutine());
    }

    private void OnDestroy()
    {
        _levelSettingsService.OnLevelSettingsChanged -= SetSpawnRate;
        _gameSaveService.OnGameSave -= SaveAsteroidsData;
    }

    public void RemoveAsteroid(Asteroid asteroid)
    {
        _asteroids.Remove(asteroid);
        asteroidPool.ReturnAsteroid(asteroid);
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