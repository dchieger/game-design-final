using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinController : MonoBehaviour
{
    public int health = 100;

    public BoxCollider2D detectionCollider;
    public BoxCollider2D attack1Collider;
    public BoxCollider2D attack2Collider;
    public BoxCollider2D sweepCollider;
    public BoxCollider2D crossSliceCollider;

    private Animator animator;
    private Rigidbody2D rb;

    private bool isFacingRight = true;
    private PlayerController player; // Assuming the player's script is named PlayerController
    private bool playerHit; // Flag to track if the player has been hit

    private float lastAttackTime; // Time of the last attack
    public float attackCooldown = 3f; // Cooldown duration in seconds
    private Health healthComponent;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>(); // Find the player in the scene
        healthComponent = GetComponent<Health>();
        
        animator.SetBool("isAlive", true);
        healthComponent.OnDeath += Die;
    }

    void Update()
    {
        if (rb.velocity.x > 0.25f && !isFacingRight)
        {
            Flip();
        }
        else if (rb.velocity.x < -0.25f && isFacingRight)
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
            int attackChoice = Random.Range(0, 4);

            switch (attackChoice)
            {
                case 0:
                    Attack1();
                    break;
                case 1:
                    Attack2();
                    break;
                case 2:
                    CrossSlice();
                    break;
                case 3:
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

    public void Attack1()
    {
        Debug.Log("Performing Attack1");
        animator.SetTrigger("Attack1");
    }

    public void Attack2()
    {
        Debug.Log("Performing Attack2");
        animator.SetTrigger("Attack2");
    }

    public void CrossSlice()
    {
        Debug.Log("Performing CrossSlice");
        animator.SetTrigger("CrossSlice");
    }

    public void Sweep()
    {
        Debug.Log("Performing Sweep attack");
        animator.SetTrigger("Sweep");
    }

    // These methods will be called by animation events                   
    public void ActivateAttack1Collider()
    {
        attack1Collider.gameObject.SetActive(true);
        playerHit = false; // Reset the flag when the attack starts
    }

    public void DeactivateAttack1Collider()
    {
        attack1Collider.gameObject.SetActive(false);
    }

    public void ActivateAttack2Collider()
    {
        attack2Collider.gameObject.SetActive(true);
        playerHit = false; // Reset the flag when the attack starts
    }

    public void DeactivateAttack2Collider()
    {
        attack2Collider.gameObject.SetActive(false);
    }

    public void ActivateCrossSliceCollider()
    {
        crossSliceCollider.gameObject.SetActive(true);
        playerHit = false; // Reset the flag when the attack starts
    }

    public void DeactivateCrossSliceCollider()
    {
        crossSliceCollider.gameObject.SetActive(false);
    }

    public void ActivateSweepCollider()
    {
        sweepCollider.gameObject.SetActive(true);
        playerHit = false; // Reset the flag when the attack starts
    }

    public void DeactivateSweepCollider()
    {
        sweepCollider.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Health playerHealth = player.GetComponent<Health>();
        if (other.CompareTag("Player") && !playerHit)
        {
            if (attack1Collider.gameObject.activeSelf)
            {
                playerHealth.TakeDamage(20);
                Debug.Log("Attack1 dealt 20 damage. Player health remaining: " + playerHealth.currentHealth);
            }
            else if (attack2Collider.gameObject.activeSelf)
            {
                playerHealth.TakeDamage(15);
                Debug.Log("Attack2 dealt 15 damage. Player health remaining: " + playerHealth.currentHealth);
            }
            else if (crossSliceCollider.gameObject.activeSelf)
            {
                playerHealth.TakeDamage(25);
                Debug.Log("CrossSlice dealt 25 damage. Player health remaining: " + playerHealth.currentHealth);
            }
            else if (sweepCollider.gameObject.activeSelf)
            {
                playerHealth.TakeDamage(20);
                Debug.Log("Sweep attack dealt 20 damage. Player health remaining: " + playerHealth.currentHealth);
            }
            playerHit = true; // Set the flag to true after hitting the player
        }   
    }
}
