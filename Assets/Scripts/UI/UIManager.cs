using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField] private Slider _healthSlider;
    
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
    
        private Image _sliderFill;
    
    

        private void Awake()
        {
            _pauseButton.onClick.AddListener(() => ShowPauseMenu(true));
            _settingsButton.onClick.AddListener(() => _settingsScreen.SetActive(true));
            _restartButton.onClick.AddListener(OnRestartButtonClicked);

            _uiUpdateService = ServiceLocator.Get<IUiUpdateService>();
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _healthSlider.gameObject.SetActive(false);
        
            _uiUpdateService.OnHealthChanged += SetHealth;
            _uiUpdateService.OnScoreChanged += UpdateScore;
            _uiUpdateService.OnDeath += () => ShowGameOverScreen(true);
            _uiUpdateService.OnWin += () => _winScreen.SetActive(true);
            _uiUpdateService.OnLevelChanged += UpdateLevel;
        
            _sliderFill = _healthSlider.fillRect.GetComponent<Image>();
        
        }

        private void OnRestartButtonClicked()
        {
            _gameStateService.GameReload();
        }

        private void UpdateLevel(int obj)
        {
            _levelText.text = obj.ToString();
        }

        private void UpdateScore(int score)
        {
            _scoreText.text = "Score: " + score.ToString();
        }

        private void SetHealth(float health)
        {
            _healthSlider.gameObject.SetActive(true);
            _healthSlider.value = health;
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
    }
}