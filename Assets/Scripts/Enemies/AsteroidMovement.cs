using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;

public class AsteroidMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _direction = Vector3.down;
    [SerializeField] private float _speed = 5.0f;
    
    IGameStateService gameStateService;
    
    private void Awake()
    {
        gameStateService = ServiceLocator.Get<IGameStateService>();
    }
    void Update()
    {
        if(gameStateService.CurrentGameState != GameState.Playing)
            return;
        Move();
    }
    
    private void Move()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }
}
