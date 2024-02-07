using System;
using Services;
using UnityEngine;

namespace Misc
{
    public class Health : MonoBehaviour, IDamageable
    {
        public int _maxHealth = 100;
        public ParticleSystem _damageEffect;
       
        private IAudioService _audioService;
        
        [NonSerialized] public int currentHealth;
        [NonSerialized] public Death _death;
        
        void Start()
        {
            currentHealth = _maxHealth;
            _audioService = ServiceLocator.Get<IAudioService>();
            _death = GetComponent<Death>();
        }
        public virtual void TakeDamage(int damage)
        {
            currentHealth -= damage;
            HitEffect();
            
            if (currentHealth <= 0)
            {
                _death.Die();
            }
        }

        public virtual void HitEffect()
        {
            _damageEffect.Play();
            _audioService.PlayHitSound();
        }


    }
}
