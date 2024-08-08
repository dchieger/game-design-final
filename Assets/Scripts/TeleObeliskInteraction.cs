using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleObeliskInteraction : MonoBehaviour
{
    Animator animator;
    private bool playerInRange = false;
    public int sceneToLoad;
    private void Awake()
    {
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
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !AreEnemiesPresent())
        {
            ActivateObelisk();
        }
    }

    private bool AreEnemiesPresent()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length > 0;
    }

    private void ActivateObelisk()
    {
        // Add your obelisk activation code here
        SceneManager.LoadScene(sceneToLoad);
        animator.SetBool("isActivated", true);
    }
}
