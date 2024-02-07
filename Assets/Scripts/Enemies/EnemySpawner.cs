using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private Vector2 _spawnRangeX = new Vector2(-3, 3);
    private Vector2 _spawnRangeY = new Vector2(5, 6);

    private ILevelSettingsService _levelSettingsService;
    private IGameStateService _gameStateService;
    private IGameSaveService _gameSaveService;
    private ICoroutineService _coroutineService;

    private EnemyPool enemyPool;
    private List<EnemyData> _enemiesData;
    private List<Enemy> _enemies = new List<Enemy>();

    private float _spawnRate;

    private void Start()
    {
        _coroutineService = ServiceLocator.Get<ICoroutineService>();
        enemyPool = GetComponent<EnemyPool>();

        _levelSettingsService = ServiceLocator.Get<ILevelSettingsService>();
        _gameStateService = ServiceLocator.Get<IGameStateService>();
        _gameSaveService = ServiceLocator.Get<IGameSaveService>();
        _levelSettingsService.OnLevelSettingsChanged += SetSpawnRate;
        _gameSaveService.OnGameSave += SaveEnemiesData;
        SetSpawnRate();
        SpawnLoadEnemies();
        _coroutineService.StartGameCoroutine(SpawnEnemyCoroutine());
    }

    private void OnDestroy()
    {
        _levelSettingsService.OnLevelSettingsChanged -= SetSpawnRate;
        _gameSaveService.OnGameSave -= SaveEnemiesData;
    }

    private void SpawnLoadEnemies()
    {
        _enemiesData = _gameSaveService.GetPlayerProgress().enemies;
        foreach (var enemyData in _enemiesData)
        {
            Enemy enemy = enemyPool.GetEnemy();
            enemy.SetEnemyData(enemyData);
            _enemies.Add(enemy);
        }
    }

    private void SaveEnemiesData()
    {
        _enemiesData = new List<EnemyData>();
        foreach (var enemy in _enemies)
        {
            _enemiesData.Add(enemy.GetEnemyData());
        }

        _gameSaveService.SetEnemies(_enemiesData);
    }

    private void SetSpawnRate()
    {
        _spawnRate = _levelSettingsService.GetLevelSettings().EnemySpawnRate;
    }

    private IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnRate);
            if (_gameStateService.CurrentGameState == GameState.Playing)
                SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Enemy enemy = enemyPool.GetEnemy();
        enemy.InitializeSpawner(this);

        Vector3 spawnPosition = new Vector3(
            Random.Range(_spawnRangeX.x, _spawnRangeX.y),
            Random.Range(_spawnRangeY.x, _spawnRangeY.y),
            0
        );

        var enemyTransform = enemy.transform;
        enemyTransform.position = spawnPosition;
        enemyTransform.parent = gameObject.transform;
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        enemyPool.ReturnEnemy(enemy);
    }
}