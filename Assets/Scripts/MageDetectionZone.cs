using UnityEngine;

public class MageDetectionZone : MonoBehaviour
{
    // Reference to the parent object

    public BoxCollider2D meleDetectionCollider;                           
    public BoxCollider2D rangeDetectionCollider;      
    public MageController mageController;
    private float lastAttackTime; // Time of the last attack              
    public float attackCooldown = 5f; 

    // Tag of the player GameObject
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                
                if (other == meleDetectionCollider)
                {
                    mageController.MeleeAttack();
                }
                else if (other == rangeDetectionCollider)
                {
                    int attackChoice = Random.Range(0, 2);
                    switch (attackChoice)
                    {
                    case 0:
                        mageController.Heal();
                        break;
                    case 1:
                        mageController.RangeAttack();
                        break;
                    }
                }
            }
            lastAttackTime = Time.time;
    }
}
}


