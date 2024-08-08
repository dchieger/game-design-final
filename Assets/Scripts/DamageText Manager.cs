using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Canvas canvas;

    public void ShowDamageText(int damage, Vector3 worldPosition)
    {
        GameObject damageTextObject = Instantiate(damageTextPrefab, worldPosition, Quaternion.identity, canvas.transform);
        
        RectTransform rectTransform = damageTextObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = WorldToCanvasPosition(worldPosition);

        HealthText healthText = damageTextObject.GetComponent<HealthText>();
        healthText.SetDamageText(damage);
    }

    private Vector2 WorldToCanvasPosition(Vector3 worldPosition)
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
        return new Vector2(
            ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
    }
}
