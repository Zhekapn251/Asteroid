using System;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class VictoryPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _restartButton;

        [Header("Stars")]
        [SerializeField] private Image[] _stars;
        [SerializeField] private Sprite _activeStar;
        [SerializeField] private Sprite _inactiveStar;

        IPlayerProgressService _playerProgressService;
        ILevelSettingsService _levelSettingsService;
        IGameStateService _gameStateService;
        private void Awake()
        {
            InitializeServices();
            InitializeButtons();
            ResetStars();
            gameObject.SetActive(false);
        }

        private void InitializeServices()
        {
            _playerProgressService = ServiceLocator.Get<IPlayerProgressService>();
            _levelSettingsService = ServiceLocator.Get<ILevelSettingsService>();
            _gameStateService = ServiceLocator.Get<IGameStateService>();
        }

        private void InitializeButtons()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnEnable()
        {
            _gameStateService.CurrentGameState = GameState.Paused;
            UpdateScore();
            CalculateStars(out var starsCount);
            UpdateStars(starsCount);
        }

        private void UpdateScore()
        {
            _scoreText.text = _playerProgressService.CurrentScore.ToString();
        }

        private void CalculateStars(out int starsCount)
        {
            starsCount = 0;
            int currentScore = _playerProgressService.CurrentScore;
            int maxScore = _levelSettingsService.GetLevelSettings().ScoreToWin;
            if (currentScore >= maxScore)
            {
                starsCount = 3;
            }
            else if (currentScore >= maxScore * 0.75f)
            {
                starsCount = 2;
            }
            else if (currentScore >= maxScore * 0.5f)
            {
                starsCount = 1;
            }
        }


        private void UpdateStars(int score)
        {
            ResetStars();

            for (int i = 0; i < score; i++)
            {
                if (i < _stars.Length)
                {
                    _stars[i].sprite = _activeStar;
                }
            }
        }

        private void ResetStars()
        {
            foreach (var star in _stars)
            {
                star.sprite = _inactiveStar;
            }
        }

        private void OnRestartButtonClicked()
        {
            _gameStateService.GameReload();
            gameObject.SetActive(false);
        }

        private void OnNextLevelButtonClicked()
        {
            _gameStateService.LoadNextLevel();
            gameObject.SetActive(false);
        }

        private void OnCloseButtonClicked() => 
            gameObject.SetActive(false);

        private void OnDisable() => 
            _gameStateService.CurrentGameState = GameState.Playing;
    }
}