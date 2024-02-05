using System;

namespace Services
{
    public interface IUiUpdateService
    {
        event Action<float> OnHealthChanged;
        event Action<int> OnScoreChanged;
        event Action<int> OnLevelChanged;
        event Action OnDeath;
        event Action OnWin;


        void HealthChanged(float health);
        void ScoreChanged(int score);
        void LevelChanged(int level);
        
        void Death();
        
        void Win();
       
    }
}