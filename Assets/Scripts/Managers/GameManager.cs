using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Gameplay,
    Shop,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Estado del Juego")]
    public GameState currentState = GameState.Gameplay;
    
    [Header("Progresión")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int totalLevels = 3;
    
    [Header("Referencias")]
    [SerializeField] private GameObject shopUI;
    [SerializeField] private WaveManager waveManager;
    
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
        // Forzar inicio limpio
        currentState = GameState.Gameplay;
        Time.timeScale = 1f;
        
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        
        Debug.Log("Juego iniciado. TimeScale: 1");
    }
    
    void Update()
    {
        // Pausa con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Gameplay)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }
    
    public void SetState(GameState newState)
    {
        currentState = newState;
        
        // --- CORRECCIÓN CRÍTICA ---
        // SOLO pausar el tiempo real en PAUSA o GAME OVER.
        // En la TIENDA dejamos el tiempo corriendo para que la UI responda bien.
        if (newState == GameState.Paused || newState == GameState.GameOver)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f; // Gameplay y Shop corren a tiempo normal
        }
        
        Debug.Log($"Estado cambiado a: {newState} | TimeScale: {Time.timeScale}");
    }
    
    public void OnLevelComplete()
    {
        Debug.Log($"¡Nivel {currentLevel} completado!");
        
        // Si aún quedan niveles regulares, abre la tienda
        if (currentLevel < totalLevels)
        {
            OpenShop();
        }
        else
        {
            Debug.Log("¡BOSS FINAL!");
            currentLevel++; // Subimos nivel (ej: de 3 a 4)
            
            // CORRECCIÓN: Debes decirle al WaveManager que inicie el nivel del Boss
            // Asegúrate de tener una "Level 4" (o el índice que corresponda) configurada en el inspector del WaveManager
            if (waveManager != null)
            {
                waveManager.StartNextLevel(currentLevel); 
            }
        }
    }
    
    public void OpenShop()
    {
        SetState(GameState.Shop); // Esto pondrá TimeScale a 1f
        
        if (shopUI != null)
        {
            shopUI.SetActive(true);
        }
    }
    
    public void CloseShop()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false);
        }
        
        SetState(GameState.Gameplay);
        
        // Avanzar nivel
        currentLevel++;
        
        // Iniciar siguiente nivel en WaveManager
        if (waveManager != null)
        {
            waveManager.StartNextLevel(currentLevel);
        }
    }
    
    public void PauseGame()
    {
        SetState(GameState.Paused);
    }
    
    public void ResumeGame()
    {
        SetState(GameState.Gameplay);
    }
    
    public void GameOver()
    {
        SetState(GameState.GameOver);
    }
    
    public int GetCurrentLevel() => currentLevel;
    public bool IsGameplayActive() => currentState == GameState.Gameplay;
}