using UnityEngine;

public class EnemySpiralShooter : EnemyController
{
    [Header("Patrón Espiral")]
    [SerializeField] private float bulletsPerBurst = 30f; // Balas por ráfaga
    [SerializeField] private float spiralSpeed = 30f; // Velocidad de rotación de la espiral


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
    
    private float angleRight = 0f;  // Ángulo para espiral derecha
    private float angleLeft = 0f;   // Ángulo para espiral izquierda    
    
    protected override void Shoot()
    {
        if (enemyData.bulletPrefab == null) return;
        
        // Espiral rotando a la DERECHA (sentido horario)
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float angle = (angleRight + (i * 120f)) * Mathf.Deg2Rad;
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
        
        // Espiral rotando a la IZQUIERDA (sentido antihorario)
        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float angle = (angleLeft + (i * 120f)) * Mathf.Deg2Rad;
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
        
        // Rotar los ángulos en direcciones opuestas
        angleRight += spiralSpeed;      // Gira en sentido horario
        angleLeft -= spiralSpeed;       // Gira en sentido antihorario
        
        // Mantener los ángulos en el rango [0, 360)
        if (angleRight >= 360f) angleRight -= 360f;
        if (angleLeft < 0f) angleLeft += 360f;
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