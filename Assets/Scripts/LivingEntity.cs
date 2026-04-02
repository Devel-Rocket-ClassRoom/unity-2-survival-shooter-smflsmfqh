using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    private float startingHealth = 100f;    
    public float Health { get; private set; }   
    public bool IsDead { get; private set; }    

    protected virtual void Awake()
    {
        IsDead = false;
        Health = startingHealth;
    }
    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Health = damage;    
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
    }   
}
