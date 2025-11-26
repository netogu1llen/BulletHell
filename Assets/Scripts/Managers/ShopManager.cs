using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI shopTitleText; // NUEVO
    
    [Header("Paneles de Tienda por Fase")]
    [SerializeField] private GameObject phase1Panel; // Armas
    [SerializeField] private GameObject phase2Panel; // Stats
    [SerializeField] private GameObject phase3Panel; // Especiales
    
    [Header("Fase 1 - Botones de Armas")]
    [SerializeField] private Button btnZigzag;
    [SerializeField] private Button btnShotgun;
    [SerializeField] private Button btnRockets;
    
    [Header("Fase 2 - Botones de Stats")]
    [SerializeField] private Button btnExtraHealth;
    [SerializeField] private Button btnMoreDamage;
    [SerializeField] private Button btnMoreSpeed;
    
    [Header("Fase 3 - Botones Especiales")]
    [SerializeField] private Button btnMassiveHealth;
    [SerializeField] private Button btnShield;
    
    [Header("Botón Continuar")]
    [SerializeField] private Button btnContinue;
    
    [Header("Costos Fase 1 - Armas")]
    [SerializeField] private int zigzagCost = 15;
    [SerializeField] private int shotgunCost = 15;
    [SerializeField] private int rocketsCost = 18;
    
    [Header("Costos Fase 2 - Stats")]
    [SerializeField] private int extraHealthCost = 20;
    [SerializeField] private int moreDamageCost = 20;
    [SerializeField] private int moreSpeedCost = 20;
    
    [Header("Costos Fase 3 - Especiales")]
    [SerializeField] private int massiveHealthCost = 30;
    [SerializeField] private int shieldCost = 30;
    
    private int currentPhase = 1;


    void Update()
{
    // Debug temporal - quitar después
    if (Input.GetKeyDown(KeyCode.D))
    {
        
        if (CurrencyManager.Instance != null)
        {
            Debug.Log($"Créditos actuales: {CurrencyManager.Instance.GetCurrentCredits()}");
        }
        
        Debug.Log($"btnZigzag existe: {btnZigzag != null}");
        if (btnZigzag != null)
        {
            Debug.Log($"btnZigzag interactable: {btnZigzag.interactable}");
            Debug.Log($"btnZigzag tiene listeners: {btnZigzag.onClick.GetPersistentEventCount()}");
        }
    }
}
    
    void Start()
    {
        Debug.Log("=== ShopManager Start ===");
        
        // FASE 1 - Armas
        if (btnZigzag != null)
        {
            btnZigzag.onClick.AddListener(() => BuyUpgrade("Zigzag", zigzagCost, 1));
            Debug.Log("Listener Zigzag añadido");
        }
        else Debug.LogError("btnZigzag es NULL!");
        
        if (btnShotgun != null)
        {
            btnShotgun.onClick.AddListener(() => BuyUpgrade("Shotgun", shotgunCost, 1));
            Debug.Log("Listener Shotgun añadido");
        }
        else Debug.LogError("btnShotgun es NULL!");
        
        if (btnRockets != null)
        {
            btnRockets.onClick.AddListener(() => BuyUpgrade("Rockets", rocketsCost, 1));
            Debug.Log("Listener Rockets añadido");
        }
        else Debug.LogError("btnRockets es NULL!");
        
        // FASE 2 - Stats
        if (btnExtraHealth != null)
            btnExtraHealth.onClick.AddListener(() => BuyUpgrade("ExtraHealth", extraHealthCost, 2));
        
        if (btnMoreDamage != null)
            btnMoreDamage.onClick.AddListener(() => BuyUpgrade("MoreDamage", moreDamageCost, 2));
        
        if (btnMoreSpeed != null)
            btnMoreSpeed.onClick.AddListener(() => BuyUpgrade("MoreSpeed", moreSpeedCost, 2));
        
        // FASE 3 - Especiales
        if (btnMassiveHealth != null)
            btnMassiveHealth.onClick.AddListener(() => BuyUpgrade("MassiveHealth", massiveHealthCost, 3));
        
        if (btnShield != null)
            btnShield.onClick.AddListener(() => BuyUpgrade("Shield", shieldCost, 3));
        
        // Continuar
        if (btnContinue != null)
        {
            btnContinue.onClick.AddListener(ContinueWithoutUpgrade);
            Debug.Log("Listener Continuar añadido");
        }
        else Debug.LogError("btnContinue es NULL!");
    }
    
    void OnEnable()
    {
        Debug.Log("=== ShopManager OnEnable ===");
        
        // DIAGNÓSTICO
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("¡CurrencyManager.Instance es NULL!");
        }
        else
        {
            int credits = CurrencyManager.Instance.GetCurrentCredits();
            Debug.Log($"CurrencyManager existe. Créditos: {credits}");
        }
        
        // Determinar fase según el nivel actual
        if (GameManager.Instance != null)
        {
            int currentLevel = GameManager.Instance.GetCurrentLevel();
            currentPhase = currentLevel;
            Debug.Log($"Nivel actual: {currentLevel}, Fase de tienda: {currentPhase}");
        }
        
        ShowPhasePanel(currentPhase);
        UpdateCreditsDisplay();
        UpdateButtonStates();
    }
    
    private void ShowPhasePanel(int phase)
    {
        Debug.Log($"Mostrando panel de fase {phase}");
        
        // Desactivar todos los paneles
        if (phase1Panel != null) phase1Panel.SetActive(false);
        if (phase2Panel != null) phase2Panel.SetActive(false);
        if (phase3Panel != null) phase3Panel.SetActive(false);
        
        // Activar el panel correspondiente
        switch (phase)
        {
            case 1:
                if (phase1Panel != null) phase1Panel.SetActive(true);
                if (shopTitleText != null) shopTitleText.text = "TIENDA - Elige tu Arma";
                break;
            case 2:
                if (phase2Panel != null) phase2Panel.SetActive(true);
                if (shopTitleText != null) shopTitleText.text = "TIENDA - Mejora tus Stats";
                break;
            case 3:
                if (phase3Panel != null) phase3Panel.SetActive(true);
                if (shopTitleText != null) shopTitleText.text = "TIENDA - Habilidades Especiales";
                break;
        }
    }
    
    private void BuyUpgrade(string upgradeName, int cost, int phase)
    {
        Debug.Log($"═══ BuyUpgrade llamado ═══");
        Debug.Log($"Mejora: {upgradeName}");
        Debug.Log($"Costo: {cost}");
        Debug.Log($"Fase: {phase}");
        
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance es NULL!");
            return;
        }
        
        int currentCredits = CurrencyManager.Instance.GetCurrentCredits();
        Debug.Log($"Créditos actuales: {currentCredits}");
        
        if (currentCredits >= cost)
        {
            Debug.Log("Suficientes créditos, intentando gastar...");
            
            if (CurrencyManager.Instance.SpendCredits(cost))
            {
                Debug.Log($"✓ Créditos gastados exitosamente");
                ApplyUpgrade(upgradeName);
                UpdateCreditsDisplay();
                CloseShop();
            }
            else
            {
                Debug.LogError("SpendCredits retornó false!");
            }
        }
        else
        {
            Debug.LogWarning($"No hay suficientes créditos! Necesitas {cost}, tienes {currentCredits}");
        }
    }
    
    private void ApplyUpgrade(string upgradeName)
    {
        Debug.Log($"Aplicando mejora: {upgradeName}");
        
        switch (upgradeName)
        {
            // FASE 1 - Armas
            case "Zigzag":
                WeaponSystem weaponSystem = FindObjectOfType<WeaponSystem>();
                if (weaponSystem != null)
                {
                    weaponSystem.ChangeWeapon(WeaponType.Zigzag);
                    if (UpgradeManager.Instance != null)
                        UpgradeManager.Instance.hasZigzag = true;
                }
                break;
                
            case "Shotgun":
                weaponSystem = FindObjectOfType<WeaponSystem>();
                if (weaponSystem != null)
                {
                    weaponSystem.ChangeWeapon(WeaponType.Shotgun);
                    if (UpgradeManager.Instance != null)
                        UpgradeManager.Instance.hasShotgun = true;
                }
                break;
                
            case "Rockets":
                weaponSystem = FindObjectOfType<WeaponSystem>();
                if (weaponSystem != null)
                {
                    weaponSystem.ChangeWeapon(WeaponType.Rockets);
                    if (UpgradeManager.Instance != null)
                        UpgradeManager.Instance.hasRockets = true;
                }
                break;
            
            // FASE 2 - Stats
            case "ExtraHealth":
                if (UpgradeManager.Instance != null)
                {
                    UpgradeManager.Instance.BuyExtraLife();
                    
                    // Aplicar vida extra al jugador inmediatamente
                    PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.Heal(10); // +10 de vida
                    }
                }
                break;
                
            case "MoreDamage":
                if (UpgradeManager.Instance != null)
                {
                    UpgradeManager.Instance.BuyDamageUpgrade();
                }
                break;
                
            case "MoreSpeed":
                if (UpgradeManager.Instance != null)
                {
                    UpgradeManager.Instance.BuySpeedUpgrade();
                    
                    // Aplicar velocidad al jugador inmediatamente
                    PlayerController playerController = FindObjectOfType<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.UpdateSpeed();
                    }
                }
                break;
            
            // FASE 3 - Especiales
            case "MassiveHealth":
                if (UpgradeManager.Instance != null)
                {
                    UpgradeManager.Instance.extraLives += 2; // +20 de vida (2 vidas extra)
                    
                    PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.Heal(20);
                    }
                }
                break;
                
            case "Shield":
                if (UpgradeManager.Instance != null)
                {
                    UpgradeManager.Instance.BuyShield();
                    
                    // Activar escudo inmediatamente
                    ShieldSystem shield = FindObjectOfType<ShieldSystem>();
                    if (shield != null)
                    {
                        shield.ActivateShield();
                    }
                }
                break;
                
            default:
                Debug.LogWarning($"Mejora desconocida: {upgradeName}");
                break;
        }
    }
    
    private void ContinueWithoutUpgrade()
    {
        Debug.Log("Continuando sin comprar mejoras...");
        CloseShop();
    }
    
    private void UpdateCreditsDisplay()
    {
        if (creditsText == null)
        {
            Debug.LogError("creditsText es NULL!");
            return;
        }
        
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("CurrencyManager.Instance es NULL en UpdateCreditsDisplay!");
            creditsText.text = "Créditos: ERROR";
            return;
        }
        
        int credits = CurrencyManager.Instance.GetCurrentCredits();
        creditsText.text = $"Créditos: {credits}";
        Debug.Log($"Créditos mostrados en UI: {credits}");
    }
    
    private void UpdateButtonStates()
    {
        if (CurrencyManager.Instance == null)
        {
            Debug.LogError("No se puede actualizar botones, CurrencyManager es NULL");
            return;
        }
        
        int credits = CurrencyManager.Instance.GetCurrentCredits();
        Debug.Log($"Actualizando estado de botones con {credits} créditos");
        
        // Actualizar según la fase actual
        switch (currentPhase)
        {
            case 1:
                UpdateButton(btnZigzag, credits >= zigzagCost, "Zigzag", zigzagCost);
                UpdateButton(btnShotgun, credits >= shotgunCost, "Shotgun", shotgunCost);
                UpdateButton(btnRockets, credits >= rocketsCost, "Rockets", rocketsCost);
                break;
                
            case 2:
                UpdateButton(btnExtraHealth, credits >= extraHealthCost, "ExtraHealth", extraHealthCost);
                UpdateButton(btnMoreDamage, credits >= moreDamageCost, "MoreDamage", moreDamageCost);
                UpdateButton(btnMoreSpeed, credits >= moreSpeedCost, "MoreSpeed", moreSpeedCost);
                break;
                
            case 3:
                UpdateButton(btnMassiveHealth, credits >= massiveHealthCost, "MassiveHealth", massiveHealthCost);
                UpdateButton(btnShield, credits >= shieldCost, "Shield", shieldCost);
                break;
        }
        
        // El botón de continuar siempre está activo
        if (btnContinue != null)
        {
            btnContinue.interactable = true;
        }
    }
    
    private void UpdateButton(Button button, bool canAfford, string buttonName, int cost)
    {
        if (button == null)
        {
            Debug.LogWarning($"Botón {buttonName} es NULL");
            return;
        }
        
        button.interactable = canAfford;
        Debug.Log($"Botón {buttonName}: {(canAfford ? "ACTIVO" : "INACTIVO")} (Costo: {cost})");
        
    }
    
    private void CloseShop()
    {
        Debug.Log("CloseShop llamado");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CloseShop();
        }
        else
        {
            Debug.LogError("¡GameManager.Instance es NULL!");
        }
    }
}