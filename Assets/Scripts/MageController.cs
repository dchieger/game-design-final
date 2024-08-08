    using System.Collections;                                                 
    using System.Collections.Generic;                                         
    using UnityEngine;                                                        
                                                                              
public class MageController : MonoBehaviour                               
{                                                                         
    public BoxCollider2D meleDetectionCollider;                           
    public BoxCollider2D rangeDetectionCollider;                          
    public BoxCollider2D sweepCollider;                                   
    public BoxCollider2D healCollider;                                    
                                                                            
    private Animator animator;                                            
    private Rigidbody2D rb;                                               
    private bool isFacingRight = true;                                    
    private PlayerController player; // Assuming the player's script is                                                 
    private bool playerHit; // Flag to track if the player has been hit   
                                                                            // Cooldown duration in seconds     
    private Health healthComponent;                                       

    public GameObject magicOrbPrefab; // Public variable to assign the Magic Orb prefab in the editor
    public Transform magicOrbSpawnPoint; // Public variable to assign the spawn point for the Magic Orb
    bool isMoving;
                            
    void Awake()                                                          
    {                                                                     
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>();
        healthComponent = GetComponent<Health>();

        healthComponent.OnHealthChanged += OnHealthChanged;
        healthComponent.OnDeath += Die;                    
                                                                            
        isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;
        animator.SetBool("isMoving", isMoving);
        Debug.Log($"Velocity: {rb.velocity.x}, isMoving: {isMoving}");                                  
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
    private void OnHealthChanged(int currentHealth)
    {
        if (currentHealth <= 0)
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
        float knockbackForce = 2f; // Adjust this value to control the strength of the knockback
        float knockbackHeight = 2f; // Adjust this value to control the height of the knockback

        // Determine the direction of the knockback based on the enemy's facing direction
        Vector2 knockbackDirection = isFacingRight ? new Vector2(knockbackForce, knockbackHeight) : new Vector2(knockbackForce, knockbackHeight);

        // Apply the force to the Rigidbody2D component
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);
    }                                                                 
                                                                
    private void SpawnMagicOrb()
    {
    Instantiate(magicOrbPrefab, magicOrbSpawnPoint.position, magicOrbSpawnPoint.rotation);
    }                                                                                                                           
    public void MeleeAttack()                                             
    {                                                                     
                                        
        animator.SetTrigger("Sweep");                                     
    }                                                                     
                                                                            
    public void RangeAttack()                                             
    {                                                                     
        Debug.Log("Performing Range attack");                           
        animator.SetTrigger("Attack");
        
    }                                                                     
                                                                            
    public void Heal()                                                    
    {                                                                     
        Debug.Log("Performing Heal");                                     
        animator.SetTrigger("Heal");                                      
    }                                                                     
                                                                                            
    public void ActivateMeleeCollider()                                   
    {       
        playerHit = false; // Reset the flag when the attack starts                                                                    
        sweepCollider.gameObject.SetActive(true);                  
    }                                                                     
                                                                            
    public void DeactivateMeleeCollider()                                 
    {                                                                     
        sweepCollider.gameObject.SetActive(false);                
    }
    public void ActivateRangeCollider()                                   
    {                                                                     
        rangeDetectionCollider.gameObject.SetActive(true);                
        playerHit = false; // Reset the flag when the attack starts       
    }                                                                     
                                                                            
    public void DeactivateRangeCollider()                                 
    {                                                                     
        rangeDetectionCollider.gameObject.SetActive(false);               
    }                                                                     
                                                                            
    public void ActivateHealCollider()                                    
    {                                                                     
        healCollider.gameObject.SetActive(true);                          
        playerHit = false; // Reset the flag when the attack starts       
    }                                                                     
                                                                            
    public void DeactivateHealCollider()                                  
    {                                                                     
        healCollider.gameObject.SetActive(false);                         
    }                                                                     
                                                                            
                                                                                                                                
                                                                            
                                                                
    private void OnTriggerStay2D(Collider2D other)
    {   
    if (other.CompareTag("Player") && !playerHit)
    {
        Health playerHealth = player.GetComponent<Health>();
        if (playerHealth != null)
        {
            if (sweepCollider.gameObject.activeSelf)
            {
                playerHealth.TakeDamage(20);
                Debug.Log("Melee attack dealt 20 damage. Player health remaining: " + playerHealth.currentHealth);
            }
            else if (other.CompareTag("Player"))
            {
                RangeAttack();
                SpawnMagicOrb();
            }
            else if (healCollider.gameObject.activeSelf)
            {
                // Heal enemies tagged with "Enemy"
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    Health enemyHealth = enemy.GetComponent<Health>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.Heal(10);
                        Debug.Log("Healed enemy by 10 health. Enemy health: " + enemyHealth.currentHealth);
                    }
                }
            }
            playerHit = true;
        }
    }
    }                                                                            
}