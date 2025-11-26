using UnityEngine;
using System.Collections;

public class ShieldSystem : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private bool isActive = false;
    [SerializeField] private float rechargeTime = 10f;
    [SerializeField] private int shieldHealth = 1;
    
    [Header("Visual")]
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private SpriteRenderer shieldRenderer;
    
    private int currentShieldHealth;
    private bool isRecharging = false;
    
    void Start()
    {
        // Verificar si el jugador compró el escudo
        if (UpgradeManager.Instance != null)
        {
            isActive = UpgradeManager.Instance.hasShield;
        }
        
        if (isActive)
        {
            currentShieldHealth = shieldHealth;
            if (shieldVisual != null)
            {
                shieldVisual.SetActive(true);
            }
            Debug.Log("Escudo activado desde el inicio!");
        }
        else
        {
            if (shieldVisual != null)
            {
                shieldVisual.SetActive(false);
            }
        }
    }
    
    public bool TryAbsorbDamage()
    {
        if (!isActive || isRecharging || currentShieldHealth <= 0)
        {
            return false; // No hay escudo disponible
        }
        
        // Escudo absorbe el daño
        currentShieldHealth--;
        Debug.Log($"¡Escudo absorbió daño! Salud restante: {currentShieldHealth}");
        
        if (currentShieldHealth <= 0)
        {
            // Escudo destruido, empezar recarga
            StartCoroutine(RechargeShield());
        }
        
        return true; // Daño absorbido
    }
    
    private IEnumerator RechargeShield()
    {
        isRecharging = true;
        
        // Desactivar visual
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
        
        Debug.Log($"Escudo recargando... {rechargeTime} segundos");
        
        yield return new WaitForSeconds(rechargeTime);
        
        // Restaurar escudo
        currentShieldHealth = shieldHealth;
        isRecharging = false;
        
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(true);
        }
        
        Debug.Log("¡Escudo recargado!");
    }
    
    public void ActivateShield()
    {
        isActive = true;
        currentShieldHealth = shieldHealth;
        isRecharging = false;
        
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(true);
        }
        
        Debug.Log("¡Escudo activado desde la tienda!");
    }
    
    public bool HasShield() => isActive && currentShieldHealth > 0 && !isRecharging;
}