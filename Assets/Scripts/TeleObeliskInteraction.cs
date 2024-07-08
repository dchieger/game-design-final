using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleObeliskInteraction : MonoBehaviour
{
    Animator animator;
    private bool playerInRange = false;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            ActivateObelisk();
        }
    }

    private void ActivateObelisk()
    {
        // Add your obelisk activation code here
         animator.SetBool(AnimationStrings.isActivated, true);
    }
}



