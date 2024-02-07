using System;
using Services;
using UnityEngine;

namespace Misc
{
    public class Death: MonoBehaviour
    {
        public Action OnRevive;
        public Action OnDie;
        
        [SerializeField] private ParticleSystem _deathVFX;
        
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;
        private IAudioService _audioService;
        
        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _audioService = ServiceLocator.Get<IAudioService>();
            
        }

        public void Die()
        {
            EnableSpriteRendererAndCollider(false);
            OnDie?.Invoke();
            DeathEffect();
            
            var totalTime = GetVFXFullDuration();
            Invoke(nameof(Revive), totalTime);
        }

        private float GetVFXFullDuration()
        {
            var mainModule = _deathVFX.main;
            float totalTime = mainModule.duration + mainModule.startLifetime.constantMax;
            return totalTime;
        }

        private void Revive()
        {
            EnableSpriteRendererAndCollider(true);
            OnRevive?.Invoke();
        }

        private void DeathEffect()
        {
            _deathVFX.Play();
            _audioService.PlayExplosionSound();
        }
        
        private void EnableSpriteRendererAndCollider(bool enable)
        {
            if (_collider2D != null)_collider2D.enabled = enable;
            _spriteRenderer.enabled = enable;
        }

    }
}
