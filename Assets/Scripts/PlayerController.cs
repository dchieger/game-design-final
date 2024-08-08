using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public int health = 100;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public BoxCollider2D attack1Collider;
    public BoxCollider2D attack2Collider;
    public BoxCollider2D glitchSweepCollider;
    public BoxCollider2D glitchSlicesCollider;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isFacingRight = true;
    private bool isInvincible = false;
    private int hitCounter = 0;
    private float lastAttackTime = 0f;
    private float attackCooldown = 0.25f; // Cooldown duration in seconds
    private Health healthComponent;
    private bool enemyHit = false;
    private DamageTextManager damageTextManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageTextManager = FindObjectOfType<DamageTextManager>();
        healthComponent = GetComponent<Health>();
        healthComponent.OnDeath += Die;
    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        // Reset double jump when grounded
        if (isGrounded)
        {
            canDoubleJump = true;
        }

        // Horizontal movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (canDoubleJump)
            {
                DoubleJump();
            }
        }

        // Flip the player sprite if moving in opposite direction
        if (moveHorizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && isFacingRight)
        {
            Flip();
        }

        // Attack inputs
        if (Input.GetButtonDown("Fire1") && Time.time - lastAttackTime >= attackCooldown)
        {
            if (hitCounter >= 5)
            {
                GlitchSlices();
                hitCounter = 0; // Reset the hit counter after performing Glitch Slices
            }
            else
            {
                PerformRandomAttack();
            }
            lastAttackTime = Time.time; // Update the time of the last attack
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            GlitchSweep();
        }

        // Update animator
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(moveHorizontal));
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        canDoubleJump = false;
        animator.SetTrigger("DoubleJump");
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void OnDamageTaken(int damage)
    {
        if (!isInvincible)
        {
            SoundManager.Instance.PlayHurtSound();
            healthComponent.TakeDamage(damage);
            damageTextManager.ShowDamageText(damage,transform.position);
            if (healthComponent.currentHealth <= 0)
            {
                Die();
                SceneManager.LoadScene(5);
            }
            else
            {
                animator.SetTrigger("Hit");
                ApplyKnockback();
            }
        }
    }

    private void ApplyKnockback()
    {
        float knockbackForce = 5f;
        float knockbackHeight = 2f;
        Vector2 knockbackDirection = isFacingRight ? new Vector2(-knockbackForce, knockbackHeight) : new Vector2(knockbackForce, knockbackHeight);
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);
    }

    private void Die()
    {
        animator.SetBool("isAlive", false);
        SceneManager.LoadScene(5);
        Destroy(gameObject, 1f);
    }

    private void PerformRandomAttack()
    {
        int attackChoice = Random.Range(0, 2);
        if (attackChoice == 0)
        {
            Attack1();
        }
        else
        {
            Attack2();
        }
    }

    void Attack1()
    {
        animator.SetTrigger("Attack1");
        isInvincible = true;
    }

    void Attack2()
    {
        animator.SetTrigger("Attack2");
        isInvincible = true;
    }

    void GlitchSweep()
    {
        animator.SetTrigger("GlitchSweep");
        isInvincible = true;
    }

    void GlitchSlices()
    {
        animator.SetTrigger("GlitchSlices");
        isInvincible = true;
    }

    // Animation event methods
    public void ActivateAttack1Collider()
    {
        attack1Collider.gameObject.SetActive(true);
    }

    public void DeactivateAttack1Collider()
    {
        attack1Collider.gameObject.SetActive(false);
        isInvincible = false; // Disable invincibility after the attack
    }

    public void ActivateAttack2Collider()
    {
        attack2Collider.gameObject.SetActive(true);
    }

    public void DeactivateAttack2Collider()
    {
        attack2Collider.gameObject.SetActive(false);
        isInvincible = false; // Disable invincibility after the attack
    }

    public void ActivateGlitchSweepCollider()
    {
        glitchSweepCollider.gameObject.SetActive(true);
    }

    public void DeactivateGlitchSweepCollider()
    {
        glitchSweepCollider.gameObject.SetActive(false);
        isInvincible = false; // Disable invincibility after the attack
    }

    public void ActivateGlitchSlicesCollider()
    {
        glitchSlicesCollider.gameObject.SetActive(true);
    }

    public void DeactivateGlitchSlicesCollider()
    {
        glitchSlicesCollider.gameObject.SetActive(false);
        isInvincible = false; // Disable invincibility after the attack
    }

    // Method to increment the hit counter
    public void IncrementHitCounter()
    {
        hitCounter++;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                if (attack1Collider.gameObject.activeSelf || attack2Collider.gameObject.activeSelf)
                {
                    SoundManager.Instance.PlayHitSound();
                    enemyHealth.TakeDamage(10);
                    Debug.Log("Player dealt 10 damage. Enemy health remaining: " + enemyHealth.currentHealth);
                    IncrementHitCounter();
                    
                }
                else if (glitchSweepCollider.gameObject.activeSelf)
                {
                    SoundManager.Instance.PlayHitSound();
                    enemyHealth.TakeDamage(15);
                    Debug.Log("Glitch Sweep dealt 15 damage. Enemy health remaining: " + enemyHealth.currentHealth);
                    IncrementHitCounter();
                }
                else if (glitchSlicesCollider.gameObject.activeSelf)
                {
                    SoundManager.Instance.PlayHitSound();
                    enemyHealth.TakeDamage(25);
                    Debug.Log("Glitch Slices dealt 25 damage. Enemy health remaining: " + enemyHealth.currentHealth);
                    IncrementHitCounter();
                }
            }
        }
    }
}