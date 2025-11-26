using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    
    [Header("Mejoras de Nivel 1 - Armas")]
    public bool hasZigzag = false;
    public bool hasShotgun = false;
    public bool hasRockets = false;
    
    [Header("Mejoras de Nivel 2 - Stats")]
    public int extraLives = 0; // Cantidad de vidas extra compradas (cada una da +10 HP)
    public int damageLevel = 0; // Nivel de daño (0 = base, 1 = x1.3)
    public int fireRateLevel = 0; // Nivel de cadencia (0 = base, 1 = más rápido)
    
    [Header("Mejoras de Nivel 3 - Especiales")]
    public int speedLevel = 0; // Nivel de velocidad (0 = base, 1 = x1.5)
    public bool hasShield = false;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Métodos para obtener valores calculados
    public int GetTotalDamage()
    {
        int baseDamage = 1;
        
        if (damageLevel > 0)
        {
            // x1.3 de daño
            return Mathf.RoundToInt(baseDamage * 1.3f);
        }
        
        return baseDamage;
    }
    
    public float GetFireRate()
    {
        float baseRate = 0.2f;
        
        if (fireRateLevel > 0)
        {
            // x1.5 más rápido = dividir por 1.5
            return baseRate / 1.5f;
        }
        
        return baseRate;
    }
    
    public float GetMoveSpeed()
    {
        float baseSpeed = 8f;
        
        if (speedLevel > 0)
        {
            // x1.5 de velocidad
            return baseSpeed * 1.5f;
        }
        
        return baseSpeed;
    }
    
    public int GetMaxHealth()
    {
        // Vida base 3 + (extraLives * 10)
        // Si compras "ExtraHealth" (+10 HP): extraLives = 1 → 3 + 10 = 13
        // Si compras "MassiveHealth" (+20 HP): extraLives = 2 → 3 + 20 = 23
        return 3 + (extraLives * 10);
    }
    
    // Métodos para comprar mejoras
    public void BuyExtraLife()
    {
        extraLives++; // +1 = +10 HP
        Debug.Log($"Vida extra comprada! Total vidas: {GetMaxHealth()}");
    }
    
    public void BuyDamageUpgrade()
    {
        damageLevel++;
        Debug.Log($"Daño mejorado! Daño actual: {GetTotalDamage()} (x1.3)");
    }
    
    public void BuyFireRateUpgrade()
    {
        fireRateLevel++;
        Debug.Log($"Cadencia mejorada! Fire rate: {GetFireRate()} (x1.5 más rápido)");
    }
    
    public void BuySpeedUpgrade()
    {
        speedLevel++;
        Debug.Log($"Velocidad mejorada! Velocidad: {GetMoveSpeed()} (x1.5)");
    }
    
    public void BuyShield()
    {
        hasShield = true;
        Debug.Log("¡Escudo comprado!");
    }
    
    // Método para resetear mejoras (útil para reiniciar el juego)
    public void ResetUpgrades()
    {
        hasZigzag = false;
        hasShotgun = false;
        hasRockets = false;
        extraLives = 0;
        damageLevel = 0;
        fireRateLevel = 0;
        speedLevel = 0;
        hasShield = false;
        
        Debug.Log("Todas las mejoras reseteadas");
    }
}