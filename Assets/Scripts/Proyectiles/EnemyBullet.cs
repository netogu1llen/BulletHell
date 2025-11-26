using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private int damage = 1;
    
    private Vector3 direction = Vector3.down;
    
    void Start()
    {
        // Registrar bala creada
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.AddEnemyBullet();
        }
    }
    
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        
        if (transform.position.y < -16f || transform.position.y > 16f ||
            transform.position.x < -16f || transform.position.x > 16f)
        {
            DestroyBullet();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            
            DestroyBullet();
        }
    }
    
    private void DestroyBullet()
    {
        // Quitar del contador
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.RemoveEnemyBullet();
        }
        
        Destroy(gameObject);
    }
}