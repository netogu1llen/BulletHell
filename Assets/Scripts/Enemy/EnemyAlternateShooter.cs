using UnityEngine;

public class EnemyAlternateShooter : EnemyController
{
    [Header("Patrón Alterno")]
    [SerializeField] private float angleOffset = 30f;
    //Movimiento Patrulla Vertical
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
    
    private bool shootLeft = true;
    
    protected override void Shoot()
    {
        if (enemyData.bulletPrefab == null) return;
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 baseDirection;
        
        if (player != null)
        {
            baseDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            baseDirection = Vector3.down;
        }
        
        // Calcular ángulo base
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        
        // Alternar entre izquierda y derecha
        float finalAngle = baseAngle + (shootLeft ? angleOffset : -angleOffset);
        float angleRad = finalAngle * Mathf.Deg2Rad;
        
        Vector3 direction = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0);
        
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
        
        // Alternar para el siguiente disparo
        shootLeft = !shootLeft;
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
            patrolDirectionY = -1; // Bajar
        }
        else if (pos.y <= bottomY)
        {
            pos.y = bottomY;
            patrolDirectionY = 1; // Subir
        }

        transform.position = pos;
    }
}