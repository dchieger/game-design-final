using UnityEngine;

public class WheelBot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    private Animator animator;
    private Damageable damageable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        damageable.damageableHit.AddListener(OnDamageTaken);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Time.time > nextFireTime)
        {
            FireProjectile(collision.transform);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void FireProjectile(Transform target)
    {
        animator.SetTrigger("bot_weapon_charge");
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Projectile>().Initialize(target);
        animator.SetTrigger("shoot");
    }

    private void OnDamageTaken(int damage, Vector2 knockback)
    {
        animator.SetTrigger("hit");
        if (!damageable.IsAlive)
        {
            animator.SetBool("isAlive", false);
            animator.SetTrigger("bot_death");
        }
    }
}
