using UnityEngine;

public class MagicOrb : MonoBehaviour
{
    public int damage = 10;

    void Start()
    {
        // Add CircleCollider2D if it doesn't exist
        if (GetComponent<CircleCollider2D>() == null)
        {
            gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Get the Health component and deal damage
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            DestroyOrb();
        }
        else if (other.CompareTag("Environment"))
        {
            // Just destroy the orb if it hits the environment
            DestroyOrb();
        }
    }

    void DestroyOrb()
    {
        // Destroy all child objects
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Destroy this game object
        Destroy(gameObject);
    }
}
