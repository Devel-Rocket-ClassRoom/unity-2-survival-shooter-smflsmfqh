using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float startingHealth = 100f;    
    public float Health { get; private set; }   
    public bool IsDead { get; private set; }

    public UnityEvent onDead;

    protected virtual void OnEnable()
    {
        IsDead = false;
        Health = startingHealth;
    }
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health -= damage;    
        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }
    }
    public virtual void Die()
    {
        if (IsDead) return; 
        IsDead = true;
        onDead?.Invoke();
    }   
}
