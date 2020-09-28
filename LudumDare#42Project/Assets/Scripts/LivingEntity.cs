using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public event System.Action OnDeath;
    public float startingHealth = 100;

    public float currentHealth;
    protected bool dead;

    protected virtual void Start()
    {
        currentHealth = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitDirection)
    {
        //Do something else

        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !dead)

            Die();
    }


    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
