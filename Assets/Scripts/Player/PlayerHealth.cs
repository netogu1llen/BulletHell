using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    
    [Header("Invulnerabilidad")]
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    private float invulnerabilityTimer = 0f;
    private bool isInvulnerable = false;
    
    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashSpeed = 10f;
    
    [Header("Eventos")]
    public UnityEvent<int> OnHealthChanged; // Para actualizar UI
    public UnityEvent OnPlayerDeath;
    
    void Start()
    {
        // Si UpgradeManager tiene mejoras, úsalas
        if (UpgradeManager.Instance != null)
        {
            int upgradeHealth = UpgradeManager.Instance.GetMaxHealth();
            // Usar el mayor entre el valor del Inspector y las mejoras
            maxHealth = Mathf.Max(maxHealth, upgradeHealth);
        }
        
        currentHealth = maxHealth;
        
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth);
        }
        
        OnHealthChanged?.Invoke(currentHealth);
        
        Debug.Log($"Jugador iniciado con {currentHealth}/{maxHealth} de vida");
    }
    
    void Update()
    {
        // Manejar invulnerabilidad
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            
            // Parpadeo visual
            float alpha = Mathf.PingPong(Time.time * flashSpeed, 1f);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
            
            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                // Restaurar alpha completo
                Color finalColor = spriteRenderer.color;
                finalColor.a = 1f;
                spriteRenderer.color = finalColor;
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
    Debug.Log($"Player recibió daño. Invulnerable: {isInvulnerable}");
    
    if (isInvulnerable) return;
    
    currentHealth -= damage;
    currentHealth = Mathf.Max(currentHealth, 0);
    
    // Actualizar HUD
    if (HUDManager.Instance != null)
    {
        HUDManager.Instance.UpdateHealth(currentHealth);
    }
    
    Debug.Log($"Jugador recibió {damage} de daño. Vida restante: {currentHealth}");
    
    OnHealthChanged?.Invoke(currentHealth);
    
    if (currentHealth <= 0)
    {
        Die();
    }
    else
    {
        ActivateInvulnerability();
        Debug.Log("Invulnerabilidad activada!");
    }
    }
    
    public void Heal(int amount)
    {
        // Actualizar vida máxima si hay upgrades
        if (UpgradeManager.Instance != null)
        {
            maxHealth = UpgradeManager.Instance.GetMaxHealth();
        }
        
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateHealth(currentHealth);
        }
        
        OnHealthChanged?.Invoke(currentHealth);
        
        Debug.Log($"Jugador curado {amount}. Vida actual: {currentHealth}/{maxHealth}");
    }
    
    private void ActivateInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;
    }
    
    private void Die()
    {
        Debug.Log("¡El jugador ha muerto!");
        
        OnPlayerDeath?.Invoke();
        
        // Por ahora solo desactivamos el objeto
        // Después implementaremos Game Over screen
        gameObject.SetActive(false);
    }
    
    // Getters públicos
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsInvulnerable() => isInvulnerable;
}