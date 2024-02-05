using System;
using Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        private IGameStateService _gameStateService;

        private void Awake()
        {
            _gameStateService = ServiceLocator.Get<IGameStateService>();
        }

        private void Start()
        {
           
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnEnable()
        {
            _gameStateService.CurrentGameState = GameState.Paused;
        }
        
        private void OnDisable()
        {
            _gameStateService.CurrentGameState = GameState.Playing;
        }
    }
}