using System;
using UnityEngine;

namespace Services
{
    public class UiUpdateService : IUiUpdateService
    {
        public event Action<float> OnHealthChanged;
        public event Action<int> OnScoreChanged;
        public event Action<int> OnGoalsDestroyShipsChanged;
        public event Action<int> OnGoalsDestroyAsteroidChanged;
        public event Action<int> OnLevelChanged;
        public event Action OnDeath;
        public event Action OnWin;

        public void HealthChanged(float health)
        {
            OnHealthChanged?.Invoke(health);
        }

        public void ScoreChanged(int score)
        {
            OnScoreChanged?.Invoke(score);
        }

        public void LevelChanged(int level)
        {
            OnLevelChanged?.Invoke(level);
        }

        public void GoalsDestroyShipsChanged(int ships)
        {
            OnGoalsDestroyShipsChanged?.Invoke(ships);
        }

        public void GoalsDestroyAsteroidChanged(int asteroid)
        {
            OnGoalsDestroyAsteroidChanged?.Invoke(asteroid);
        }

        public void Death()
        {
            OnDeath?.Invoke();
        }
        
        public void Win()
        {
            OnWin?.Invoke();
        }

    }
}