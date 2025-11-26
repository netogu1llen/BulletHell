using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWave", menuName = "BulletHell/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class EnemySpawn
    {
        public GameObject enemyPrefab;
        public Vector3 spawnPosition;
    }
    
    [Header("Configuración de Oleada")]
    public string waveName;
    public List<EnemySpawn> enemies = new List<EnemySpawn>();
    
    [Header("Timing")]
    public float delayBeforeStart = 1f; // Segundos antes de empezar la oleada
    public float spawnDelay = 0.3f; // Delay entre cada enemigo individual
}