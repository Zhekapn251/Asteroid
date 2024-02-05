using System;

namespace Services
{
    public class UiUpdateService : IUiUpdateService
    {
        public event Action<float> OnHealthChanged;
        public event Action<int> OnScoreChanged;
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