using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweeperController : MonoBehaviour
{

    public BoxCollider2D detectionCollider;
    public BoxCollider2D slamCollider;
    public BoxCollider2D spinCollider;
    public BoxCollider2D sweepCollider;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private PlayerController player; // Assuming the player's script is named PlayerController
    private bool playerHit; // Flag to track if the player has been hit

    private float lastAttackTime; // Time of the last attack
    public float attackCooldown = 3f; // Cooldown duration in seconds
    private Health healthComponent;
    private DamageTextManager damageTextManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        healthComponent = GetComponent<Health>();
        damageTextManager = FindObjectOfType<DamageTextManager>();
        
        animator.SetBool("isAlive", true);
        healthComponent.OnDeath += Die;
    }

    void Update()
    {
        if (rb.velocity.x > 0.1f && !isFacingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < -0.1f && isFacingRight)
        {
            Flip();
        }
        animator.SetBool("isMoving", rb.velocity.x != 0);
    }

    void Flip()
    {
        // Toggle the direction the sprite is facing
        isFacingRight = !isFacingRight;
        // Multiply the sprite's x local scale by -1 to flip it
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public void OnDamageTaken(int damage)
    {
            healthComponent.TakeDamage(damage);
            damageTextManager.ShowDamageText(damage, transform.position);
            if (healthComponent.currentHealth <= 0)
            {
                Die();
            }
            else
            {
                animator.SetTrigger("Hit");
                ApplyKnockback();
            }
    }

    private void Die()
    {
        animator.SetBool("isAlive", false);
        Destroy(gameObject, 1f);
    }


    private void ApplyKnockback()
    {
        float knockbackForce = 100f; // Adjust this value to control the strength of the knockback
        float knockbackHeight = 50f; // Adjust this value to control the height of the knockback

        // Determine the direction of the knockback based on the enemy's facing direction
        Vector2 knockbackDirection = isFacingRight ? new Vector2(-knockbackForce, knockbackHeight) : new Vector2(knockbackForce, knockbackHeight);

        // Apply the force to the Rigidbody2D component
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);
    }

    public void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            int attackChoice = Random.Range(0, 3);

            switch (attackChoice)
            {
                case 0:
                    Slam();
                    break;
                case 1:
                    SpinSlam();
                    break;
                case 2:
                    Sweep();
                    break;
            }

            lastAttackTime = Time.time; // Update the time of the last attack
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Attack();
        }
    }

    public void Slam()
    {
        Debug.Log("Performing Slam attack");
        animator.SetTrigger("Slam");
    }

    public void SpinSlam()
    {
        Debug.Log("Performing SpinSlam attack");
        animator.SetTrigger("SpinSlam");
    }

    public void Sweep()
    {
        Debug.Log("Performing Sweep attack");
        animator.SetTrigger("Sweep");
    }

    // These methods will be called by animation events                   
    public void ActivateSlamCollider()
    {
        playerHit = false; // Reset the flag when the attack starts
        slamCollider.gameObject.SetActive(true);
    }

    public void DeactivateSlamCollider()
    {
        slamCollider.gameObject.SetActive(false);
    }

    public void ActivateSpinSlamCollider()
    {
        playerHit = false; // Reset the flag when the attack starts
        spinCollider.gameObject.SetActive(true);
    }

    public void DeactivateSpinSlamCollider()
    {
        spinCollider.gameObject.SetActive(false);
    }

    public void ActivateSweepCollider()
    {
        playerHit = false; // Reset the flag when the attack starts
        sweepCollider.gameObject.SetActive(true);
        
    }

    public void DeactivateSweepCollider()
    {
        sweepCollider.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !playerHit)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                if (slamCollider.gameObject.activeSelf)
                {
                    playerHealth.TakeDamage(20);
                    Debug.Log("Slam attack dealt 20 damage. Player health remaining: " + playerHealth.currentHealth);
                }
                else if (spinCollider.gameObject.activeSelf)
                {
                    playerHealth.TakeDamage(15);
                    Debug.Log("SpinSlam attack dealt 15 damage. Player health remaining: " + playerHealth.currentHealth);
                }
                else if (sweepCollider.gameObject.activeSelf)
                {
                    playerHealth.TakeDamage(20);
                    Debug.Log("Sweep attack dealt 20 damage. Player health remaining: " + playerHealth.currentHealth);
                }
                playerHit = true;
            }
        }
    }
}
