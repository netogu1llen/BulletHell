using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    
    private Vector3 direction = Vector3.up;
    
    void Start()
    {
        if (HUDManager.Instance != null)
        {
            HUDManager.Instance.AddPlayerBullet();
        }
    }
    
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }
    
    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    void Update()
    {
        // Mover en la dirección asignada
        transform.position += direction * speed * Time.deltaTime;
        
        // Destruir si sale de pantalla
        if (transform.position.y < -16f || transform.position.y > 16f ||
            transform.position.x < -16f || transform.position.x > 16f)
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