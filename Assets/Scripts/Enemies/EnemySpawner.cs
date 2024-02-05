using System.Collections;
using System.Collections.Generic;
using Entities;
using Services;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnRangeX = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 _spawnRangeY = new Vector2(5f, 10f);

    private ILevelSettingsService _levelSettingsService;
    private IGameStateService _gameStateService;
    private IGameSaveService _gameSaveService;

    private EnemyPool enemyPool;
    private List<EnemyData> _enemiesData;
    private List<Enemy> _enemies = new List<Enemy>();

    private float _spawnRate = 2f;

    private void Start()
    {
        enemyPool = GetComponent<EnemyPool>();

        _levelSettingsService = ServiceLocator.Get<ILevelSettingsService>();
        _gameStateService = ServiceLocator.Get<IGameStateService>();
        _gameSaveService = ServiceLocator.Get<IGameSaveService>();
        _levelSettingsService.OnLevelSettingsChanged += SetSpawnRate;
        _gameSaveService.OnGameSave += GameSaveServiceOnGameSave;
        SetSpawnRate();
        SpawnLoadEnemies();
        StartCoroutine(SpawnEnemyCoroutine());
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

    private void GameSaveServiceOnGameSave()
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

        _enemies.Add(enemy);
        enemy.transform.position = spawnPosition;
    }

    public void RemoveEnemy(Enemy enemy) =>
        _enemies.Remove(enemy);
}