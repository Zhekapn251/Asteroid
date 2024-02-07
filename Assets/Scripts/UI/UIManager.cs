using System;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI goalsShipText;
        [SerializeField] private TextMeshProUGUI goalsAsteroidText;
        [SerializeField] private TextMeshProUGUI get3StarsText;

        [SerializeField] private Slider healthSlider;
    
        [Header("Panels")]
        [SerializeField] 
        private GameObject _pauseMenu;
        [SerializeField] 
        private GameObject _gameOverScreen;
        [SerializeField] 
        private GameObject _winScreen;
        [SerializeField] 
        private GameObject _settingsScreen;
    
        [Header("Buttons")] 
        [SerializeField] 
        private Button _pauseButton;
        [SerializeField] 
        private Button _restartButton;
        [SerializeField] 
        private Button _settingsButton;
    
    
        private IUiUpdateService _uiUpdateService;
        private IGameStateService _gameStateService;
        private ILevelSettingsService _levelSettingsService;
    
        private Image _sliderFill;
    
    

        private void Awake()
        {
            _pauseButton.onClick.AddListener(() => ShowPauseMenu(true));
            _settingsButton.onClick.AddListener(() => ShowSettingsScreen(true));
            _restartButton.onClick.AddListener(OnRestartButtonClicked);

            _uiUpdateService = ServiceLocator.Get<IUiUpdateService>();
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _levelSettingsService = ServiceLocator.Get<ILevelSettingsService>();
            healthSlider.gameObject.SetActive(false);
        
            _uiUpdateService.OnHealthChanged += SetHealth;
            _uiUpdateService.OnScoreChanged += UpdateScore;
            _uiUpdateService.OnGoalsDestroyAsteroidChanged += UpdateAsteroidsGoals;
            _uiUpdateService.OnGoalsDestroyShipsChanged += UpdateShipsGoals;
            _uiUpdateService.OnDeath += () => ShowGameOverScreen(true);
            _uiUpdateService.OnWin += () => ShowWinGameScreen(true);
            _uiUpdateService.OnLevelChanged += UpdateLevel;
            _levelSettingsService.OnLevelSettingsChanged += UpdateLevelsGoals;
        
            _sliderFill = healthSlider.fillRect.GetComponent<Image>();
        
        }

        private void UpdateShipsGoals(int number)
        {
            goalsShipText.text = number.ToString();
        }

        private void UpdateAsteroidsGoals(int number)
        {
            goalsAsteroidText.text = number.ToString();
        }

        private void Update3StarsText(int number)
        {
            get3StarsText.text = $"Score {number} points to get 3 stars";
        }
        private void UpdateLevelsGoals()
        {
            
            var levelGoals = _levelSettingsService.GetLevelSettings();
            
            UpdateShipsGoals(levelGoals.EnemiesToDestroy);
            UpdateAsteroidsGoals(levelGoals.AsteroidsToDestroy);
            Update3StarsText(levelGoals.ScoreToGet3Stars);
        }
        

        private void OnRestartButtonClicked()
        {
            _gameStateService.GameReload();
        }

        private void UpdateLevel(int obj)
        {
            levelText.text = obj.ToString();
        }

        private void UpdateScore(int score)
        {
            scoreText.text = "Score: " + score.ToString();
        }

        private void SetHealth(float health)
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = health;
            _sliderFill.color = Color.Lerp(Color.red, Color.green, health);
        }

        private void ShowPauseMenu(bool show)
        {
            _pauseMenu.SetActive(show);
        }

        private void ShowGameOverScreen(bool show)
        {
            _gameOverScreen.SetActive(show);
        }

        private void ShowWinGameScreen(bool show)
        {
            _winScreen.SetActive(show);
        }

        private void ShowSettingsScreen(bool show)
        {
            _settingsScreen.SetActive(show);
        }
    }
}