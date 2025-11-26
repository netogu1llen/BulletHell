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
        
        // Forzar estado inicial
        currentState = GameState.Gameplay;
        Time.timeScale = 1f;
        
        if (shopUI != null)
        {
            shopUI.SetActive(false);
            Debug.Log("Tienda forzada a cerrar");
        }
        
        Debug.Log("Juego iniciado correctamente");
    }
    
    void Update()
    {
        // Pausa con ESC (opcional)
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
        
        // Congelar tiempo según el estado
        if (newState == GameState.Shop || newState == GameState.Paused || newState == GameState.GameOver)
        {
            Time.timeScale = 0f; // Pausar el juego
        }
        else
        {
            Time.timeScale = 1f; // Reanudar el juego
        }
        
        Debug.Log($"Estado del juego: {newState}");
    }
    
    public void OnLevelComplete()
    {
        Debug.Log($"¡Nivel {currentLevel} completado!");
        
        if (currentLevel < totalLevels)
        {
            // Abrir tienda
            OpenShop();
        }
        else
        {
            // Última fase - Boss Final
            Debug.Log("¡Preparándose para el Boss Final!");
            currentLevel++;
            // Aquí cargaríamos la escena del boss final
        }
    }
    
    public void OpenShop()
    {
        SetState(GameState.Shop);
        
        // TEMPORAL: No pausar el juego
        Time.timeScale = 0f; // ← Cambiar de 0 a 1 temporalmente
        
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
        
        // Avanzar al siguiente nivel
        currentLevel++;
        
        // Reiniciar el WaveManager para el siguiente nivel
        if (waveManager != null)
        {
            waveManager.StartNextLevel(currentLevel);
        }
    }
    
    public void PauseGame()
    {
        SetState(GameState.Paused);
        Debug.Log("Juego pausado");
    }
    
    public void ResumeGame()
    {
        SetState(GameState.Gameplay);
        Debug.Log("Juego reanudado");
    }
    
    public void GameOver()
    {
        SetState(GameState.GameOver);
        Debug.Log("GAME OVER");
        // Aquí mostrarías la pantalla de Game Over
    }
    
    public int GetCurrentLevel() => currentLevel;
    public bool IsGameplayActive() => currentState == GameState.Gameplay;
}