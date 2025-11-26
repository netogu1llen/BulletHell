using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }
    
    [Header("Créditos")]
    [SerializeField] private int currentCredits = 0;
    [SerializeField] private GameObject creditPrefab;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateUI();
    }
    
    public void AddCredits(int amount)
    {
        currentCredits += amount;
        UpdateUI();
        Debug.Log($"Créditos totales: {currentCredits}");
    }
    
    public bool SpendCredits(int amount)
    {
        if (currentCredits >= amount)
        {
            currentCredits -= amount;
            UpdateUI();
            Debug.Log($"Gastados {amount} créditos. Restantes: {currentCredits}");
            return true;
        }
        else
        {
            Debug.Log("¡No hay suficientes créditos!");
            return false;
        }
    }
    
    public void SpawnCredit(Vector3 position)
    {
        if (creditPrefab != null)
        {
            // Añadir variación a la posición inicial
            Vector3 spawnPos = position + new Vector3(
                Random.Range(-0.3f, 0.3f),
                Random.Range(-0.2f, 0.2f),
                0
            );
            
            Instantiate(creditPrefab, spawnPos, Quaternion.identity);
        }
    }
    
    private void UpdateUI()
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.UpdateCredits(currentCredits);
        }
    }
    
    public int GetCurrentCredits() => currentCredits;
}