using UnityEngine;

public class EnemyShooterSpread : EnemyController
{
    [Header("Spread Pattern")]
    [SerializeField] private int bulletCount = 3;
    [SerializeField] private float spreadAngle = 30f;
    
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
}