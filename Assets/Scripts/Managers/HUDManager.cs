using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }
    
    [Header("Textos UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI playerBulletsText;
    [SerializeField] private TextMeshProUGUI enemyBulletsText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI creditsText;
    
    private int playerBulletCount = 0;
    private int enemyBulletCount = 0;
    private int enemyCount = 0;
    private int credits = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateAllUI();
    }
    
    // Actualizar vida del jugador
    public void UpdateHealth(int currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = $"Vida: {currentHealth}";
        }
    }
    
    // Actualizar contador de balas del jugador
    public void UpdatePlayerBullets(int count)
    {
        playerBulletCount = count;
        if (playerBulletsText != null)
        {
            playerBulletsText.text = $"Balas Jugador: {playerBulletCount}";
        }
    }
    
    // Actualizar contador de balas enemigas
    public void UpdateEnemyBullets(int count)
    {
        enemyBulletCount = count;
        if (enemyBulletsText != null)
        {
            enemyBulletsText.text = $"Balas Enemigas: {enemyBulletCount}";
        }
    }
    
    // Actualizar contador de enemigos
    public void UpdateEnemies(int count)
    {
        enemyCount = count;
        if (enemiesText != null)
        {
            enemiesText.text = $"Enemigos: {enemyCount}";
        }
    }
    
    // Actualizar créditos
    public void UpdateCredits(int amount)
    {
        credits = amount;
        if (creditsText != null)
        {
            creditsText.text = $"Créditos: {credits}";
        }
    }
    
    // Métodos para incrementar/decrementar
    public void AddPlayerBullet() => UpdatePlayerBullets(playerBulletCount + 1);
    public void RemovePlayerBullet() => UpdatePlayerBullets(Mathf.Max(0, playerBulletCount - 1));
    
    public void AddEnemyBullet() => UpdateEnemyBullets(enemyBulletCount + 1);
    public void RemoveEnemyBullet() => UpdateEnemyBullets(Mathf.Max(0, enemyBulletCount - 1));
    
    public void AddEnemy() => UpdateEnemies(enemyCount + 1);
    public void RemoveEnemy() => UpdateEnemies(Mathf.Max(0, enemyCount - 1));
    
    public void AddCredits(int amount) => UpdateCredits(credits + amount);
    
    private void UpdateAllUI()
    {
        UpdatePlayerBullets(playerBulletCount);
        UpdateEnemyBullets(enemyBulletCount);
        UpdateEnemies(enemyCount);
        UpdateCredits(credits);
    }
}