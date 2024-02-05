using Services;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private Vector3 _movementDirection = Vector3.down;
    
    private IGameStateService gameStateService;
    
    private void Awake()
    {
        gameStateService = ServiceLocator.Get<IGameStateService>();
    }

    void Update()
    {
        if(gameStateService.CurrentGameState != GameState.Playing)
            return;
        transform.Translate(_movementDirection.normalized * _speed * Time.deltaTime);
    }
}