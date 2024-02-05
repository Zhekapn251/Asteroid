using System;

namespace Services
{
    public interface IGameStateService
    {
        public event Action OnLevelWin;
        public event Action OnGameReload;
        public GameState CurrentGameState { get; set; }
        
        public void LoadNextLevel();
        
        public void GameReload();
       
    }
}