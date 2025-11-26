using UnityEngine;

public class EnemyWaveShooter : EnemyController
{
    [Header("Patrón de Onda")]
    [SerializeField] private int wavePoints = 7; // Puntos de la onda
    [SerializeField] private float waveAmplitude = 60f; // Amplitud de la onda

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