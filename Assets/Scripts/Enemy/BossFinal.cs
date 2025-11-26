using UnityEngine;

public class BossFinal : EnemyController
{
    [Header("Patrón 1 - Círculo")]
    [SerializeField] private int circleCount = 8;
    
    [Header("Patrón 2 - Hacia Jugador")]
    [SerializeField] private int targetedBullets = 3;
    [SerializeField] private float spreadAngle = 15f;
    
    [Header("Alternancia")]
    [SerializeField] private float patternDuration = 5f; // Tiempo que dura cada patrón
    //Movimiento Patrulla horizontal
    [Header("Movimiento Patrulla Horizontal")]
    [SerializeField] private float leftX = -8f;
    [SerializeField] private float rightX = 8f;
    [SerializeField] private float patrolSpeedX = 3f;
    private int patrolDirectionX = 1; // 1 = derecha, -1 = izquierda
    
    private float patternTimer = 0f;
    private bool usePattern1 = true; // true = círculo, false = hacia jugador
    private float siguienteAtaque = 0f;
    
    // Sobrescribimos Start para inicializar el timer y llamar al base
    protected override void Start()
    {
        base.Start(); // Importante: Inicializa vida, renderer, etc.
        patternTimer = patternDuration;
    }
    
    // Sobrescribimos la lógica de disparo para controlar la alternancia
    protected override void HandleShooting()
    {
        // 1. Gestionar el cambio de patrón
        patternTimer -= Time.deltaTime;
        
        if (patternTimer <= 0f)
        {
            usePattern1 = !usePattern1; // Alternar
            patternTimer = patternDuration; // Reiniciar timer
            
            string nombrePatron = usePattern1 ? "Círculo Expansivo" : "Ataque Dirigido";
            Debug.Log($"<color=yellow>BOSS cambió a patrón: {nombrePatron}</color>");
        }
        
        // 2. Gestionar el disparo según la cadencia (FireRate)
        if (Time.time >= siguienteAtaque && enemyData.bulletPrefab != null)
        {
            Shoot();
            siguienteAtaque = Time.time + enemyData.fireRate;
        }
    }
    
    // Ejecuta el patrón actual
    protected override void Shoot()
    {
        if (enemyData.bulletPrefab == null) return;
        
        if (usePattern1)
        {
            ShootCirclePattern();
        }
        else
        {
            ShootTargetedPattern();
        }
    }
    
    private void ShootCirclePattern()
    {
        float angleStep = 360f / circleCount;
        
        for (int i = 0; i < circleCount; i++)
        {
            float angle = (angleStep * i) * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            
            CreateBullet(direction);
        }
    }
    
    private void ShootTargetedPattern()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 baseDirection = Vector3.down;
        
        if (player != null)
        {
            baseDirection = (player.transform.position - transform.position).normalized;
        }
        
        // Calcular ángulo base hacia el jugador
        float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;
        
        // 1. Bala central
        CreateBullet(baseDirection);
        
        // 2. Balas laterales (Spread)
        for (int i = 1; i <= targetedBullets; i++)
        {
            // Derecha
            float angleRight = (baseAngle - (spreadAngle * i)) * Mathf.Deg2Rad;
            Vector3 dirRight = new Vector3(Mathf.Cos(angleRight), Mathf.Sin(angleRight), 0);
            CreateBullet(dirRight);
            
            // Izquierda
            float angleLeft = (baseAngle + (spreadAngle * i)) * Mathf.Deg2Rad;
            Vector3 dirLeft = new Vector3(Mathf.Cos(angleLeft), Mathf.Sin(angleLeft), 0);
            CreateBullet(dirLeft);
        }
    }
    
    private void CreateBullet(Vector3 direction)
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

        transform.position = pos;
    }
}