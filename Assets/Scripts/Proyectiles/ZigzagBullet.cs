using UnityEngine;

public class ZigzagBullet : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float zigzagAmplitude = 2f; // Qué tan ancho zigzaguea
    [SerializeField] private float zigzagFrequency = 3f; // Qué tan rápido zigzaguea
    
    private float timeAlive = 0f;
    private float startX;
    
    void Start()
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.AddPlayerBullet();
        }
        
        startX = transform.position.x;
    }
    
    void Update()
    {
        timeAlive += Time.deltaTime;
        
        // Movimiento hacia arriba
        float newY = transform.position.y + speed * Time.deltaTime;
        
        // Movimiento zigzag en X
        float newX = startX + Mathf.Sin(timeAlive * zigzagFrequency) * zigzagAmplitude;
        
        transform.position = new Vector3(newX, newY, transform.position.z);
        
        // Destruir si sale de pantalla
        if (transform.position.y > 10f || transform.position.x < -12f || transform.position.x > 12f)
        {
            DestroyBullet();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            
            DestroyBullet();
        }
    }
    
    private void DestroyBullet()
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.RemovePlayerBullet();
        }
        
        Destroy(gameObject);
    }
}