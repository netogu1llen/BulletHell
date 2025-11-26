using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] protected EnemyData enemyData;
    
    private int currentHealth;
    private float nextFireTime;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        currentHealth = enemyData.maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = enemyData.enemyColor;
        }
        
        nextFireTime = Time.time + Random.Range(0.5f, 2f);
        
        // Registrar enemigo creado
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.AddEnemy();
        }
        CreateHealthDisplay();
    }

    private void CreateHealthDisplay()
    {
        GameObject displayObj = new GameObject("HealthDisplay");
    }

    void Update()
    {
        HandleShooting();
    }
    
    private void HandleShooting()
    {
        if (Time.time >= nextFireTime && enemyData.bulletPrefab != null)
        {
            Shoot();
            nextFireTime = Time.time + enemyData.fireRate;
        }
    }
    
    protected virtual void Shoot()
{
    // Encontrar al jugador
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    
    Vector3 direction;
    
    if (player == null)
    {
        Debug.LogError("¡No se encuentra el jugador!");
        direction = Vector3.down;
    }
    else
    {
        // DEBUG: Ver posiciones
        Debug.Log($"Enemigo en: {transform.position}, Jugador en: {player.transform.position}");
        
        // Calcular dirección hacia el jugador
        direction = (player.transform.position - transform.position).normalized;
        
        Debug.Log($"Dirección calculada: {direction}");
    }
    
    // Crear la bala
    GameObject bullet = Instantiate(
        enemyData.bulletPrefab, 
        transform.position, 
        Quaternion.identity
    );
    
    // Asignar dirección
    EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
    if (enemyBullet != null)
    {
        enemyBullet.SetDirection(direction);
        Debug.Log("Dirección asignada a la bala");
    }
    else
    {
        Debug.LogError("¡La bala no tiene el componente EnemyBullet!");
    }
}
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Efecto visual de daño (parpadeo)
        if (currentHealth > 0)
        {
            StartCoroutine(DamageFlash());
        }
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private System.Collections.IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = enemyData.enemyColor;
        }
    }
    
    private void Die()
    {
        DropCredits();
        
        // Quitar del contador de enemigos
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.RemoveEnemy();
        }
        
        WaveManager waveManager = FindObjectOfType<WaveManager>();
        if (waveManager != null)
        {
            waveManager.OnEnemyKilled();
        }
        
        Destroy(gameObject);
    }
        
   private void DropCredits()
    {
        int creditsAmount = Random.Range(enemyData.minCredits, enemyData.maxCredits + 1);
        
        for (int i = 0; i < creditsAmount; i++)
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.SpawnCredit(transform.position);
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("PlayerBullet"))
    {
        TakeDamage(1);
    }
    else if (collision.CompareTag("Player"))
    {
        // Daño por contacto
        PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
        
        // El enemigo también muere al chocar (opcional)
        Die();
    }
}


    // Métodos públicos para acceder a la vida
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return enemyData.maxHealth;
    }
}