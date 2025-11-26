using UnityEngine;

public class EnemyWaveShooter : EnemyController
{
    [Header("Patrón de Onda")]
    [SerializeField] private int wavePoints = 7; // Puntos de la onda
    [SerializeField] private float waveAmplitude = 60f; // Amplitud de la onda
    
    private float wavePhase = 0f;
    
    protected override void Shoot()
    {
        if (enemyData.bulletPrefab == null) return;
        
        for (int i = 0; i < wavePoints; i++)
        {
            // Calcular posición en la onda usando seno
            float t = (float)i / (wavePoints - 1); // 0 a 1
            float angle = -90f + (Mathf.Sin((t * Mathf.PI * 2) + wavePhase) * waveAmplitude);
            
            float angleRad = angle * Mathf.Deg2Rad;
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
        }
        
        // Cambiar fase para siguiente disparo
        wavePhase += 0.5f;
    }
}