using Misc;
using Services;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : Health
    {
        [SerializeField] private Color _hitColor;
        [SerializeField] private float _hitDuration;
    
        private SpriteRenderer _spriteRenderer;
        private IUiUpdateService _uiUpdateService;
        private IGameStateService _gameStateService;
        private IGameSaveService _gameSaveService;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        
            InitializeServices(); 
        
            _gameStateService.OnGameReload += GameStateServiceOnGameReload;
            _gameStateService.OnLevelWin += GameStateServiceOnGameReload;
            _gameSaveService.OnGameSave += GameSaveServiceOnGameSave;

            LoadPlayerHealth();
            UpdateHealth();
        }

        private void OnDestroy()
        {
            _gameStateService.OnGameReload -= GameStateServiceOnGameReload;
            _gameStateService.OnLevelWin -= GameStateServiceOnGameReload;
            _gameSaveService.OnGameSave -= GameSaveServiceOnGameSave;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            _uiUpdateService.HealthChanged((float) currentHealth / _maxHealth);
            if (currentHealth <= 0) _uiUpdateService.Death();
        }

        private void InitializeServices()
        {
            _uiUpdateService = ServiceLocator.Get<IUiUpdateService>();
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _gameSaveService = ServiceLocator.Get<IGameSaveService>();
        }

        private void LoadPlayerHealth() => 
            currentHealth = _gameSaveService.GetPlayerProgress().playerHealth;

        private void GameSaveServiceOnGameSave() => 
            _gameSaveService.SetPlayerHealth(currentHealth);

        private void GameStateServiceOnGameReload()
        {
            currentHealth = _maxHealth;
            UpdateHealth();
        }

        protected override void HitEffect()
        {
            _spriteRenderer.color = _hitColor;
            UpdateHealth();
            Invoke(nameof(ResetColor), _hitDuration);
        }

        private void UpdateHealth() => 
            _uiUpdateService.HealthChanged((float) currentHealth / _maxHealth);

        private void ResetColor() => 
            _spriteRenderer.color = Color.white;
    }
}