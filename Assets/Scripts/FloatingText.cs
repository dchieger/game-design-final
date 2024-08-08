using UnityEngine;
using TMPro; // If you're using TextMeshPro

public class FloatingText : MonoBehaviour
{
    public GameObject floatingTextPrefab; // Assign your text prefab here
    private GameObject floatingTextInstance;
    private TextMeshProUGUI textMesh; // Use TextMeshProUGUI if using TextMeshPro, otherwise use Text

    void Start()
    {
        // Instantiate the floating text and set its parent to the canvas
        floatingTextInstance = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        textMesh = floatingTextInstance.GetComponent<TextMeshProUGUI>();
        floatingTextInstance.transform.SetParent(GameObject.Find("Canvas").transform, false); // Ensure it's a child of the Canvas
    }

    void Update()
    {
        // Update the position of the floating text to follow the game object
        if (floatingTextInstance != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            floatingTextInstance.transform.position = screenPosition;
        }

        // Check if there are any enemies left
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        if (enemies.Length == 0 && floatingTextInstance != null)
        {
            Destroy(floatingTextInstance);
        }
    }
}