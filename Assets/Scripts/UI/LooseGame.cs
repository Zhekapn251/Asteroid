using System;
using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LooseGame: MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _restartButton;
        
        [SerializeField] private TextMeshProUGUI _scoreText;
        
        private IGameStateService _gameStateService;
        private IPlayerProgressService _playerProgressService;
        private void Awake()
        {
            _gameStateService = ServiceLocator.Get<IGameStateService>();
            _playerProgressService = ServiceLocator.Get<IPlayerProgressService>();
            
            _closeButton.onClick.AddListener(OnRestartButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnEnable()
        {
            _gameStateService.CurrentGameState = GameState.Paused;
            _scoreText.text = _playerProgressService.CurrentScore.ToString();
        }
        
        private void OnDisable() => _gameStateService.CurrentGameState = GameState.Playing;

        private void OnRestartButtonClicked()
        {
            gameObject.SetActive(false);
            _gameStateService.GameReload();
        }
        

    }
}