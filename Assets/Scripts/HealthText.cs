using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
    public float timeToLive = 0.5f;
    public float floatSpeed = 300f;
    public Vector3 floatDirection = new Vector3(0, 1, 0);
    public TextMeshProUGUI textMesh;

    private RectTransform rTransform;
    private Color startingColor;
    private float timeElapsed = 0.0f;

    public void SetDamageText(int damage)
    {
        textMesh.text = damage.ToString();
        if (damage > 0)
            textMesh.color = Color.red;
        else
            textMesh.color = Color.green;
        
        startingColor = textMesh.color;
    }

    void Start()
    {
        rTransform = GetComponent<RectTransform>();
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        rTransform.position += floatDirection * floatSpeed * Time.deltaTime;

        textMesh.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1 - (timeElapsed / timeToLive));

        if (timeElapsed > timeToLive)
        {
            Destroy(gameObject);
        }
    }
}
