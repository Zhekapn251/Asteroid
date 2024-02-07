using Services;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private readonly Vector3 _direction = Vector3.down;
    public float speed = 1.0f;
    
    private IGameStateService gameStateService;
    
    private void Awake()
    {
        gameStateService = ServiceLocator.Get<IGameStateService>();
    }

    private void Update()
    {
        if(gameStateService.CurrentGameState != GameState.Playing)
            return;
        Move();
    }
    
    private void Move()
    {
        transform.Translate(_direction * speed * Time.deltaTime);
    }
}