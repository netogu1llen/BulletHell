using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Referencias UI")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TextMeshProUGUI creditsText;
    [SerializeField] private TextMeshProUGUI shopTitleText;
    
    [Header("Paneles de Fase")]
    [SerializeField] private GameObject phase1Panel;
    [SerializeField] private GameObject phase2Panel;
    [SerializeField] private GameObject phase3Panel;
    
    [Header("Fase 1 - Armas (Botones)")]
    [SerializeField] private Button btnZigzag;
    [SerializeField] private Button btnShotgun;
    [SerializeField] private Button btnRockets;
    
    [Header("Fase 2 - Stats (Botones)")]
    [SerializeField] private Button btnExtraHealth;
    [SerializeField] private Button btnMoreDamage;
    [SerializeField] private Button btnMoreSpeed;
    
    [Header("Fase 3 - Especiales (Botones)")]
    [SerializeField] private Button btnMassiveHealth;
    [SerializeField] private Button btnShield;
    
    [Header("Navegación")]
    [SerializeField] private Button btnContinuePhase1; 
    [SerializeField] private Button btnContinuePhase2; 
    [SerializeField] private Button btnContinuePhase3; 
    
    [Header("Costos Fase 1")]
    [SerializeField] private int zigzagCost = 15;
    [SerializeField] private int shotgunCost = 15;
    [SerializeField] private int rocketsCost = 18;
    
    [Header("Costos Fase 2")]
    [SerializeField] private int extraHealthCost = 20;
    [SerializeField] private int moreDamageCost = 20;
    [SerializeField] private int moreSpeedCost = 20;
    
    [Header("Costos Fase 3")]
    [SerializeField] private int massiveHealthCost = 30;
    [SerializeField] private int shieldCost = 30;
    
    private int currentPhase = 1;

    void Start()
    {
        // --- Listeners Fase 1 ---
        if (btnZigzag != null) btnZigzag.onClick.AddListener(() => BuyUpgrade("Zigzag", zigzagCost));
        if (btnShotgun != null) btnShotgun.onClick.AddListener(() => BuyUpgrade("Shotgun", shotgunCost));
        if (btnRockets != null) btnRockets.onClick.AddListener(() => BuyUpgrade("Rockets", rocketsCost));
        
        // --- Listeners Fase 2 ---
        if (btnExtraHealth != null) btnExtraHealth.onClick.AddListener(() => BuyUpgrade("ExtraHealth", extraHealthCost));
        if (btnMoreDamage != null) btnMoreDamage.onClick.AddListener(() => BuyUpgrade("MoreDamage", moreDamageCost));
        if (btnMoreSpeed != null) btnMoreSpeed.onClick.AddListener(() => BuyUpgrade("MoreSpeed", moreSpeedCost));
        
        // --- Listeners Fase 3 ---
        if (btnMassiveHealth != null) btnMassiveHealth.onClick.AddListener(() => BuyUpgrade("MassiveHealth", massiveHealthCost));
        if (btnShield != null) btnShield.onClick.AddListener(() => BuyUpgrade("Shield", shieldCost));
        
        // --- Listeners para TODOS los botones de continuar (por si no compras nada) ---
        if (btnContinuePhase1 != null) btnContinuePhase1.onClick.AddListener(CloseShop);
        if (btnContinuePhase2 != null) btnContinuePhase2.onClick.AddListener(CloseShop);
        if (btnContinuePhase3 != null) btnContinuePhase3.onClick.AddListener(CloseShop);
    }
    
    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            // Obtenemos el nivel actual para saber qué fase mostrar
            currentPhase = GameManager.Instance.GetCurrentLevel();
            Debug.Log($"[SHOP] Abriendo tienda. Nivel detectado: {currentPhase}");
        }
        
        RefreshShopUI();
    }
    
    private void RefreshShopUI()
    {
        UpdateCreditsDisplay();
        ShowPhasePanel(currentPhase);
        UpdateButtonStates();
    }
    
    private void ShowPhasePanel(int phase)
    {
        // Ocultar todos los paneles primero
        if (phase1Panel != null) phase1Panel.SetActive(false);
        if (phase2Panel != null) phase2Panel.SetActive(false);
        if (phase3Panel != null) phase3Panel.SetActive(false);
        
        // Lógica de seguridad: Si el nivel es mayor a 3 (ej. nivel 4, 5),
        // seguimos mostrando la Fase 3 (la más avanzada) en lugar de rompernos o volver a la 1.
        int phaseToShow = phase;
        if (phase > 3) 
        {
            phaseToShow = 3;
        }

        Debug.Log($"[SHOP] Mostrando panel de fase: {phaseToShow}");

        switch (phaseToShow)
        {
            case 1:
                if (phase1Panel != null) phase1Panel.SetActive(true);
                if (shopTitleText != null) shopTitleText.text = "TIENDA - ELIGE UN ARMA";
                break;
            case 2:
                if (phase2Panel != null) phase2Panel.SetActive(true);
                if (shopTitleText != null) shopTitleText.text = "TIENDA - MEJORAS DE STATS";
                break;
            case 3:
                if (phase3Panel != null) phase3Panel.SetActive(true);
                if (shopTitleText != null) shopTitleText.text = "TIENDA - HABILIDAD DEFINITIVA";
                break;
            default:
                // Fallback por si acaso
                if (phase1Panel != null) phase1Panel.SetActive(true);
                break;
        }
    }
    
    private void BuyUpgrade(string upgradeName, int cost)
    {
        if (CurrencyManager.Instance == null) return;
        
        if (CurrencyManager.Instance.SpendCredits(cost))
        {
            Debug.Log($"[SHOP] Compra exitosa: {upgradeName}. Cerrando tienda...");
            ApplyUpgrade(upgradeName);
            UpdateCreditsDisplay();
            
            // Cerramos la tienda inmediatamente tras comprar para avanzar
            CloseShop();
        }
        else
        {
            Debug.Log("[SHOP] Créditos insuficientes.");
        }
    }
    
    private void ApplyUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "Zigzag":
                FindObjectOfType<WeaponSystem>()?.ChangeWeapon(WeaponType.Zigzag);
                if (UpgradeManager.Instance) UpgradeManager.Instance.hasZigzag = true;
                break;
            case "Shotgun":
                FindObjectOfType<WeaponSystem>()?.ChangeWeapon(WeaponType.Shotgun);
                if (UpgradeManager.Instance) UpgradeManager.Instance.hasShotgun = true;
                break;
            case "Rockets":
                FindObjectOfType<WeaponSystem>()?.ChangeWeapon(WeaponType.Rockets);
                if (UpgradeManager.Instance) UpgradeManager.Instance.hasRockets = true;
                break;
            case "ExtraHealth":
                if (UpgradeManager.Instance) UpgradeManager.Instance.BuyExtraLife();
                FindObjectOfType<PlayerHealth>()?.Heal(10);
                break;
            case "MoreDamage":
                if (UpgradeManager.Instance) UpgradeManager.Instance.BuyDamageUpgrade();
                break;
            case "MoreSpeed":
                if (UpgradeManager.Instance) UpgradeManager.Instance.BuySpeedUpgrade();
                FindObjectOfType<PlayerController>()?.UpdateSpeed();
                break;
            case "MassiveHealth":
                if (UpgradeManager.Instance) UpgradeManager.Instance.extraLives += 2;
                FindObjectOfType<PlayerHealth>()?.Heal(20);
                break;
            case "Shield":
                if (UpgradeManager.Instance) UpgradeManager.Instance.BuyShield();
                FindObjectOfType<ShieldSystem>()?.ActivateShield();
                break;
        }
    }
    
    private void UpdateCreditsDisplay()
    {
        if (CurrencyManager.Instance != null && creditsText != null)
        {
            creditsText.text = $"Créditos: {CurrencyManager.Instance.GetCurrentCredits()}";
        }
    }
    
    private void UpdateButtonStates()
    {
        if (CurrencyManager.Instance == null) return;
        
        int credits = CurrencyManager.Instance.GetCurrentCredits();
        
        // Solo actualizamos los botones de la fase VISIBLE para ahorrar recursos
        if (currentPhase == 1)
        {
            UpdateButton(btnZigzag, credits >= zigzagCost);
            UpdateButton(btnShotgun, credits >= shotgunCost);
            UpdateButton(btnRockets, credits >= rocketsCost);
        }
        else if (currentPhase == 2)
        {
            UpdateButton(btnExtraHealth, credits >= extraHealthCost);
            UpdateButton(btnMoreDamage, credits >= moreDamageCost);
            UpdateButton(btnMoreSpeed, credits >= moreSpeedCost);
        }
        else if (currentPhase >= 3) // >= 3 para cubrir niveles posteriores
        {
            UpdateButton(btnMassiveHealth, credits >= massiveHealthCost);
            UpdateButton(btnShield, credits >= shieldCost);
        }
    }
    
    private void UpdateButton(Button btn, bool canAfford)
    {
        if (btn != null) btn.interactable = canAfford;
    }
    
    public void CloseShop()
    {
        Debug.Log("[SHOP] Solicitando cierre de tienda al GameManager.");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CloseShop();
        }
    }
}