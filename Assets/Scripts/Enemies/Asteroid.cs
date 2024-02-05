using Entities;
using Misc;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

public class Asteroid : MonoBehaviour
{
     [SerializeField] private Sprite[] _sprites;
     [SerializeField] private int damage = 1;
     
     private SpriteRenderer _spriteRenderer;
     private CapsuleCollider2D _capsuleCollider2D;
     private AsteroidPool _asteroidPool;
     private Death _death;
     private Health _health;
     private AsteroidData _asteroidData;
    
     private IPlayerProgressService _playerProgressService;
     
     private int spriteIndex;
     private AsteroidSpawner _asteroidSpawner;

     private void Awake()
     {
         _spriteRenderer = GetComponent<SpriteRenderer>();
         _playerProgressService = ServiceLocator.Get<IPlayerProgressService>();
            _death = GetComponent<Death>();
            _health = GetComponent<Health>();
            _death.OnRevive += Death_OnRevive;
            _death.OnDie += Death_OnDie;
     }

     public void Initialize(AsteroidPool asteroidPool)
     {
         _asteroidPool = asteroidPool;

         spriteIndex = RandomSpriteIndex();
         _spriteRenderer.sprite = _sprites[spriteIndex];

         EnableSprite(true);
        
         if (_capsuleCollider2D != null)
             Destroy(_capsuleCollider2D);
         
         AddCapsuleCollider();
     }
    public void InitializeAsteroidSpawner(AsteroidSpawner asteroidSpawner)
     {
         _asteroidSpawner = asteroidSpawner;
     }
     public AsteroidPool GetAsteroidPool() => 
         _asteroidPool;
     
     public AsteroidData GetAsteroidData() =>
            new AsteroidData(transform.position, _health.currentHealth, spriteIndex);

     public void SetAsteroidData(AsteroidData asteroidData)
     {
         transform.position = asteroidData.position;
         _health.currentHealth = asteroidData.health;
         spriteIndex = asteroidData.spriteIndex;
         _spriteRenderer.sprite = _sprites[spriteIndex];
     }
     private void OnTriggerEnter2D(Collider2D col)
     {
         if (col.gameObject.TryGetComponent(out IDamageable damageable))
         {
             damageable.TakeDamage(damage);
         }
     }

     private void AddCapsuleCollider()
     {
         _capsuleCollider2D = gameObject.AddComponent<CapsuleCollider2D>();
            _capsuleCollider2D.isTrigger = true;
     }

     public void EnableSprite(bool enable) => 
         _spriteRenderer.enabled = enable;

     private int RandomSpriteIndex() => 
         Random.Range(0, _sprites.Length);

     private void Death_OnRevive()
     {
         _asteroidPool.ReturnAsteroid(this);
         _asteroidSpawner?.RemoveAsteroid(this);
     }

     private void Death_OnDie()
     {
         _playerProgressService.OnAsteroidDestroyed();
         _playerProgressService.AddScore(5);
         _playerProgressService.IsLevelComplete();
     }
}
