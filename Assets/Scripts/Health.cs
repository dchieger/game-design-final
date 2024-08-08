using UnityEngine;
using System;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;
    public event Action<int> OnDamageTaken;

    private bool canBeHit = true;
    private float hitCooldown = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (canBeHit)
        {
            animator.SetTrigger("Hit");
            
            currentHealth -= damage;
            OnHealthChanged?.Invoke(currentHealth);
            OnDamageTaken?.Invoke(damage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                
                OnDeath?.Invoke();
            }

            StartCoroutine(HitCooldown());
        }
    }

    private IEnumerator HitCooldown()
    {
        canBeHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canBeHit = true;
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }
}