using System;

namespace Services
{
    public interface IPlayerProgressService
    {
        event Action OnWin;
        int CurrentScore { get; }

        void AddScore(int score);
        void OnAsteroidDestroyed();
        void OnEnemyDestroyed();
        bool IsLevelComplete();
        void ResetProgress();
        void ResetScore();
        void SaveProgress();
        void LoadProgress();

    }
}