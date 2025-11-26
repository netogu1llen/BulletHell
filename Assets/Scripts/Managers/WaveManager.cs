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
    
    private bool waveInProgress = false;
    private bool levelInProgress = false;
    private bool levelCompleted = false;

    void Start()
    {
        StartLevel(0);
    }
    
    void Update()
    {
        // Solo procesar si estamos en gameplay
        if (GameManager.Instance != null && !GameManager.Instance.IsGameplayActive())
        {
            return;
        }
        
        // Si hay una oleada en progreso Y no hay enemigos vivos
        if (waveInProgress && enemiesAlive <= 0)
        {
            Debug.Log($"✓ Oleada {currentWaveIndex + 1}/{levels[currentLevelIndex].waves.Count} completada!");
            
            waveInProgress = false; // Marcar oleada como completada
            
            // VERIFICAR ANTES DE INCREMENTAR
            int nextWaveIndex = currentWaveIndex + 1;
            int totalWaves = levels[currentLevelIndex].waves.Count;
            
            Debug.Log($"   Próxima oleada sería: {nextWaveIndex + 1}");
            Debug.Log($"   Total de oleadas: {totalWaves}");
            
            // ¿Es esta la ÚLTIMA oleada?
            if (nextWaveIndex >= totalWaves)
            {
                // SÍ - Esta era la última oleada
                Debug.Log("→ ¡ERA LA ÚLTIMA OLEADA! Nivel completado.");
                currentWaveIndex = nextWaveIndex; // Actualizar el índice
                OnLevelComplete();
            }
            else
            {
                // NO - Aún hay más oleadas
                currentWaveIndex = nextWaveIndex; // Actualizar el índice
                int remainingWaves = totalWaves - nextWaveIndex;
                Debug.Log($"→ Quedan {remainingWaves} oleadas más. Siguiente en 3s...");
                
                Invoke(nameof(StartNextWave), 3f);
            }
        }
    }
        
    private void StartLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count)
        {
            return;
        }
        
        currentLevelIndex = levelIndex;
        currentWaveIndex = 0;
        levelInProgress = true;
        levelCompleted = false; // NUEVO: Resetear flag
        
        StartNextWave();
    }
    
    public void StartNextLevel(int levelNumber)
    {
        // levelNumber es 1-indexed (Nivel 1, 2, 3...)
        // currentLevelIndex es 0-indexed (0, 1, 2...)
        StartLevel(levelNumber - 1);
    }
    
    private void StartNextWave()
    {
        if (currentWaveIndex >= levels[currentLevelIndex].waves.Count)
        {
            return;
        }
        
        WaveData currentWave = levels[currentLevelIndex].waves[currentWaveIndex];
        Debug.Log($"Iniciando oleada {currentWaveIndex + 1}: {currentWave.waveName}");
        
        StartCoroutine(SpawnWave(currentWave));
    }
    
    private IEnumerator SpawnWave(WaveData wave)
    {
        waveInProgress = true;
        
        yield return new WaitForSeconds(wave.delayBeforeStart);
        
        foreach (var enemySpawn in wave.enemies)
        {
            if (enemySpawn.enemyPrefab != null)
            {
                Instantiate(enemySpawn.enemyPrefab, enemySpawn.spawnPosition, Quaternion.identity);
                enemiesAlive++;
            }
            
            yield return new WaitForSeconds(wave.spawnDelay);
        }
    }
    
    private void OnLevelComplete()
    {
        // VERIFICACIÓN ADICIONAL: ¿Realmente completamos todas las oleadas?
        if (currentWaveIndex < levels[currentLevelIndex].waves.Count)
        {
            return; // NO completar el nivel
        }
        
        // Prevenir llamadas múltiples
        if (levelCompleted) 
        {
            return;
        }
        
        levelCompleted = true;
        levelInProgress = false;
        

        
        // Notificar al GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLevelComplete();
        }
    }
    
    public void OnEnemyKilled()
    {
        enemiesAlive--;
        Debug.Log($"Enemigos restantes: {enemiesAlive}");
    }
    
    public int GetCurrentWave() => currentWaveIndex + 1;
    public int GetTotalWaves() => levels[currentLevelIndex].waves.Count;
}