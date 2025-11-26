using UnityEngine;
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshPro healthText;
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0);
    
    [Header("Fuente")]
    [SerializeField] private TMP_FontAsset minecraftFont; // NUEVO
    
    private EnemyController enemyController;
    
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
        
        if (enemyController == null)
        {
            Debug.LogError("¡No se encontró EnemyController en el padre!");
            Destroy(gameObject);
            return;
        }
        
        if (healthText == null)
        {
            GameObject textObj = new GameObject("HealthText");
            textObj.transform.SetParent(transform);
            textObj.transform.localPosition = offset;
            
            healthText = textObj.AddComponent<TextMeshPro>();
            healthText.fontSize = 3;
            healthText.alignment = TextAlignmentOptions.Center;
            healthText.color = Color.white;
            
            // NUEVO: Aplicar fuente de Minecraft si está asignada
            if (minecraftFont != null)
            {
                healthText.font = minecraftFont;
            }
            
            healthText.outlineWidth = 0.2f;
            healthText.outlineColor = Color.black;
        }
        
        UpdateDisplay();
    }
    
    void Update()
    {
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (enemyController != null && healthText != null)
        {
            int currentHealth = enemyController.GetCurrentHealth();
            healthText.text = currentHealth.ToString();
        }
    }
}