using System;
using UnityEngine;

namespace Services
{
    public class GameStateService : MonoBehaviour, IGameStateService
    {
        public event Action OnLevelWin;
        public event Action OnGameReload;
        
        public GameState CurrentGameState { get; set; } = GameState.Playing;

        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        public void LoadNextLevel() => 
            OnLevelWin?.Invoke();

        public void GameReload() =>
            OnGameReload?.Invoke();
    }
}
