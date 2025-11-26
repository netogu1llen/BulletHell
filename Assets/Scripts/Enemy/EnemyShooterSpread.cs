using UnityEngine;

public class EnemyShooterSpread : EnemyController
{
    [Header("Spread Pattern")]
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float spreadAngle = 30f;

    [Header("Movimiento Patrulla Horizontal")]
    [SerializeField] private float leftX = -8f;
    [SerializeField] private float rightX = 8f;
    [SerializeField] private float patrolSpeedX = 3f;
    private int patrolDirectionX = 1; // 1 = derecha, -1 = izquierda
    
    //Movimiento Patrulla Vertical
    [Header("Movimiento Vertical")]
    [SerializeField] private float topY = 4f;
    [SerializeField] private float bottomY = 2f;
    [SerializeField] private float patrolSpeedY = 2f;
    private int patrolDirectionY = -1; // 1 = arriba, -1 = abajo
    
    protected override void Shoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        Vector3 baseDirection;
        
        if (player == null)
        {
            baseDirection = Vector3.down;
        }
        else
        {
            baseDirection = (player.transform.position - transform.position).normalized;
        }
        
        // Calcular ángulo base
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        
        // Disparar múltiples balas en abanico
        for (int i = 0; i < bulletCount; i++)
        {
            // Calcular el ángulo de esta bala
            float angleOffset = spreadAngle * (i - (bulletCount - 1) / 2f) / (bulletCount - 1);
            float finalAngle = (baseAngle + angleOffset) * Mathf.Deg2Rad;
            
            Vector3 direction = new Vector3(Mathf.Cos(finalAngle), Mathf.Sin(finalAngle), 0);
            
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
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // Movimiento horizontal
        pos.x += patrolDirectionX * patrolSpeedX * Time.deltaTime;

        // Cambiar dirección horizontal si alcanzamos los límites
        if (pos.x >= rightX)
        {
            pos.x = rightX;
            patrolDirectionX = -1;
        }
        else if (pos.x <= leftX)
        {
            pos.x = leftX;
            patrolDirectionX = 1;
        }

        // Movimiento vertical
        pos.y += patrolDirectionY * patrolSpeedY * Time.deltaTime;

        // Cambiar dirección vertical si alcanzamos los límites
        if (pos.y >= topY)
        {
            pos.y = topY;
            patrolDirectionY = -1;
        }
        else if (pos.y <= bottomY)
        {
            pos.y = bottomY;
            patrolDirectionY = 1;
        }

        transform.position = pos;
    }
}