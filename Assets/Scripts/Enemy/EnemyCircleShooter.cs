using UnityEngine;

public class EnemyCircleShooter : EnemyController
{
    [Header("Patrón Circular")]
    [SerializeField] private int bulletsPerShot = 8; // Balas por ráfaga
    [SerializeField] private float rotationOffset = 0f; // Para rotar el patrón cada disparo
    
    [Header("Movimiento Patrulla")]
    [SerializeField] private float leftX = -12.8f;
    [SerializeField] private float rightX = 12.8f;
    [SerializeField] private float patrolSpeed = 3f;
    private int patrolDirection = 1; // 1 = hacia la derecha, -1 = hacia la izquierda
    
    protected override void Shoot()
    {
        if (enemyData.bulletPrefab == null) return;
        
        float angleStep = 360f / bulletsPerShot;
        
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = (angleStep * i + rotationOffset) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            
            GameObject bullet = Instantiate(
                enemyData.bulletPrefab,
                transform.position,
                Quaternion.identity
            );
            
            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet != null)
            {
                enemyBullet.SetDirection(direction);
            }
        }
        
        // Rotar el patrón para el siguiente disparo
        rotationOffset += 15f;
    }

    // Usamos FixedUpdate para no ocultar el Update() del EnemyController
    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        pos.x += patrolDirection * patrolSpeed * Time.deltaTime;

        // Cambiar dirección si alcanzamos los límites
        if (pos.x >= rightX)
        {
            pos.x = rightX;
            patrolDirection = -1;
        }
        else if (pos.x <= leftX)
        {
            pos.x = leftX;
            patrolDirection = 1;
        }

        transform.position = pos;
    }
}