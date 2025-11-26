using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string levelName;
        public List<WaveData> waves;
    }
    
    [Header("Niveles")]
    [SerializeField] private List<Level> levels = new List<Level>();
    
    [Header("Estado Actual")]
    [SerializeField] private int currentLevelIndex = 0;
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private int enemiesAlive = 0;
    
    // Banderas de control
    private bool waveInProgress = false;
    private bool isSpawning = false; // ¡NUEVO! Evita que la oleada termine mientras aparecen enemigos
    private bool levelCompleted = false;

    void Start()
    {
        // Asegurarse de empezar limpio
        enemiesAlive = 0;
        StartLevel(0);
    }
    
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameplayActive()) return;
        
        // CONDICIÓN CORREGIDA:
        // Solo completar si:
        // 1. La oleada está activa
        // 2. NO se están generando enemigos actualmente (!isSpawning)
        // 3. No quedan enemigos vivos
        if (waveInProgress && !isSpawning && enemiesAlive <= 0)
        {
            CompleteWave();
        }
    }
    
    private void CompleteWave()
    {
        Debug.Log($"✓ Oleada {currentWaveIndex + 1} completada");
        
        waveInProgress = false;
        
        int nextWaveIndex = currentWaveIndex + 1;
        int totalWaves = levels[currentLevelIndex].waves.Count;
        
        if (nextWaveIndex >= totalWaves)
        {
            OnLevelComplete();
        }
        else
        {
            currentWaveIndex = nextWaveIndex;
            Debug.Log("Siguiente oleada en 3s...");
            Invoke(nameof(StartNextWave), 3f);
        }
    }
        
    private void StartLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count)
        {
            Debug.LogError("Intentando cargar un nivel que no existe: " + levelIndex);
            return;
        }
        
        currentLevelIndex = levelIndex;
        currentWaveIndex = 0;
        levelCompleted = false;
        
        Debug.Log($"=== INICIANDO NIVEL {levelIndex + 1} ===");
        StartNextWave();
    }
    
    public void StartNextLevel(int levelNumber)
    {
        // levelNumber viene del GameManager (1, 2, 3...)
        // Convertimos a índice (0, 1, 2...)
        StartLevel(levelNumber - 1);
    }
    
    private void StartNextWave()
    {
        if (currentLevelIndex >= levels.Count || currentWaveIndex >= levels[currentLevelIndex].waves.Count) return;
        
        WaveData currentWave = levels[currentLevelIndex].waves[currentWaveIndex];
        StartCoroutine(SpawnWave(currentWave));
    }
    
    private IEnumerator SpawnWave(WaveData wave)
    {
        waveInProgress = true;
        isSpawning = true; // ¡IMPORTANTE! Bloqueamos la condición de victoria
        
        yield return new WaitForSeconds(wave.delayBeforeStart);
        
        foreach (var enemySpawn in wave.enemies)
        {
            if (enemySpawn.enemyPrefab != null)
            {
                Instantiate(enemySpawn.enemyPrefab, enemySpawn.spawnPosition, Quaternion.identity);
                enemiesAlive++; // Incrementamos ANTES de esperar
            }
            
            yield return new WaitForSeconds(wave.spawnDelay);
        }
        
        isSpawning = false; // ¡Ahora sí! Si matas a todos, ganas
    }
    
    private void OnLevelComplete()
    {
        if (levelCompleted) return;
        
        Debug.Log("=== NIVEL COMPLETADO ===");
        levelCompleted = true;
        waveInProgress = false;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelComplete();
        }
    }
    
    public void OnEnemyKilled()
    {
        enemiesAlive--;
        // Seguridad para que nunca sea negativo
        if (enemiesAlive < 0) enemiesAlive = 0; 
    }
    
    public int GetCurrentWave() => currentWaveIndex + 1;
}