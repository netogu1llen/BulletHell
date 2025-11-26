using UnityEngine;

public class EnemyVShooter : EnemyController
{
    [Header("Patrón en V")]
    [SerializeField] private int bulletsPerSide = 2; // Balas por cada lado de la V
    [SerializeField] private float vAngle = 45f; // Ángulo de apertura de la V
    
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
        
        // Bala central hacia el jugador
        ShootBullet(baseDirection);
        
        // Balas a la izquierda
        for (int i = 1; i <= bulletsPerSide; i++)
        {
            float leftAngle = (baseAngle + (vAngle * i / bulletsPerSide)) * Mathf.Deg2Rad;
            Vector3 leftDir = new Vector3(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle), 0);
            ShootBullet(leftDir);
        }
        
        // Balas a la derecha
        for (int i = 1; i <= bulletsPerSide; i++)
        {
            float rightAngle = (baseAngle - (vAngle * i / bulletsPerSide)) * Mathf.Deg2Rad;
            Vector3 rightDir = new Vector3(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle), 0);
            ShootBullet(rightDir);
        }
    }
    
    private void ShootBullet(Vector3 direction)
    {
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