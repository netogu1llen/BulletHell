using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "BulletHell/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Identificación")]
    public string enemyName;
    
    [Header("Estadísticas")]
    public int maxHealth = 1;
    public float moveSpeed = 2f;
    public int damageToPlayer = 1;
    
    [Header("Disparo")]
    public float fireRate = 2f; // Cada cuántos segundos dispara
    public GameObject bulletPrefab;
    
    [Header("Recompensas")]
    public int minCredits = 0;
    public int maxCredits = 2;
    public int scoreValue = 10;
    
    [Header("Visual")]
    public Color enemyColor = Color.red;
}