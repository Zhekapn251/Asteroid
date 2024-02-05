using Services;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
    
        private IGameStateService _gameStateService;
        private IGameSaveService _gameSaveService;
        private InputService _inputService = new InputService();
        private Vector2 screenBounds;
    
        private void Start()
        {
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _gameSaveService = ServiceLocator.Get<IGameSaveService>();
            _gameSaveService.OnGameSave += GameSaveServiceOnGameSave;
            LoadPlayerPosition();
            CalculateScreenBounds();
        }

        private void Update()
        {
            if(_gameStateService.CurrentGameState != GameState.Playing)
                return;
            MovePlayer(_inputService.GetMovement().x, _inputService.GetMovement().y);

            CalculateClampPosition();
        }

        private void LoadPlayerPosition()
        {
            Vector3 playerPosition = _gameSaveService.GetPlayerProgress().playerPosition;
            transform.position = playerPosition;
        }

        private void MovePlayer(float horizontalInput, float verticalInput)
        {
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }

        private void GameSaveServiceOnGameSave()
        {
            _gameSaveService.SetPlayerPosition(transform.position);
        }

        private void CalculateClampPosition()
        {
            Vector3 pos = transform.position;


            pos.x = Mathf.Clamp(pos.x, -screenBounds.x / 2, screenBounds.x / 2);
            pos.y = Mathf.Clamp(pos.y, -screenBounds.y / 2, screenBounds.y / 2);

            transform.position = pos;
        }

        private void CalculateScreenBounds()
        {
        
            float cameraDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
            Vector2 screenBottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraDistance));
            Vector2 screenTopRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, cameraDistance));

            screenBounds = screenTopRight - screenBottomLeft;
        }
    }
}
 