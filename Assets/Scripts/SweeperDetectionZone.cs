using UnityEngine;

public class SweeperDetectionZone : MonoBehaviour

{

    public BoxCollider2D detectionCollider;
    public BoxCollider2D slamCollider;
    public BoxCollider2D spinCollider;
    public BoxCollider2D sweepCollider;

    public SweeperController sweeperController;


    // Tag of the player GameObject

    public string playerTag = "Player";


    private void OnTriggerEnter(Collider other)

    {

        // Check if the entering GameObject has the player tag

        if (other.gameObject.tag == playerTag)

        {

            // Notify the parent object that a player has entered the collider

            sweeperController.Attack();

        }

    }


    private void OnTriggerExit(Collider other)

    {

        // Check if the exiting GameObject has the player tag

        if (other.gameObject.tag == playerTag)

        {

            // Notify the parent object that a player has exited the collider

            // You might want to add a method to stop the attack here

        }

    }

}