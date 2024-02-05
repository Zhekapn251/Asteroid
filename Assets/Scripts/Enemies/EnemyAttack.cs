using Misc;
using UnityEngine;

public class EnemyAttack: MonoBehaviour
{
    [SerializeField] private int _damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damage);
    }
}