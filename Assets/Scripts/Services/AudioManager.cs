using Entities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class AudioManager: MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Sounds")]
        [SerializeField] private AudioClip[] _hitSounds;
        [SerializeField] private AudioClip[] _shootSounds;
        [SerializeField] private AudioClip[] _enemyExplosionSounds;


        private ISettingsService _settingsService;
        private AudioFXSettings _audioFXSettings;
        private void Awake() => 
            DontDestroyOnLoad(gameObject);

        private void Start()
        {
            _settingsService = ServiceLocator.Get<ISettingsService>();
            _settingsService.OnAudioSettingsChanged += UpdateAudioSettings;            
            UpdateAudioSettings();
        }

        private void OnDestroy()
        {
            _settingsService.OnAudioSettingsChanged -= UpdateAudioSettings;
        }

        private void UpdateAudioSettings()
        {
            _audioFXSettings = _settingsService.LoadAudioSettings();
            SetMusicVolume();
            SetSoundVolume();
        }

        private void SetSoundVolume() => 
            _sfxSource.volume = _audioFXSettings.sfxVolume;

        private void SetMusicVolume() => 
            _musicSource.volume = _audioFXSettings.musicVolume;

        public void PlayHitSound() => 
            _sfxSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Length)]);

        public void PlayShootSound()=> 
            _sfxSource.PlayOneShot(_shootSounds[Random.Range(0, _shootSounds.Length)]);

        public void PlayExplosionSound()=>
            _sfxSource.PlayOneShot(_enemyExplosionSounds[Random.Range(0, _enemyExplosionSounds.Length)]);

       
    }
}