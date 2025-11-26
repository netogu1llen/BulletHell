using UnityEngine;

public enum WeaponType
{
    Normal,
    Zigzag,
    Shotgun,
    Rockets
}

public class WeaponSystem : MonoBehaviour
{
    [Header("Tipos de Bala")]
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private GameObject zigzagBulletPrefab;
    [SerializeField] private GameObject rocketBulletPrefab;
    
    [Header("Configuración Actual")]
    [SerializeField] private WeaponType currentWeapon = WeaponType.Normal;
    
    [Header("Estadísticas")]
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private int damage = 1;
    
    [Header("Referencias")]
    [SerializeField] private Transform firePoint;
    
    private float nextFireTime = 0f;
    
    void Update()
    {
        // Solo disparar durante gameplay
        if (GameManager.Instance != null && !GameManager.Instance.IsGameplayActive())
        {
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    private void Shoot()
    {
        switch (currentWeapon)
        {
            case WeaponType.Normal:
                ShootNormal();
                break;
            case WeaponType.Zigzag:
                ShootZigzag();
                break;
            case WeaponType.Shotgun:
                ShootShotgun();
                break;
            case WeaponType.Rockets:
                ShootRockets();
                break;
        }
    }
    
    private void ShootNormal()
    {
        if (normalBulletPrefab != null && firePoint != null)
        {
            Instantiate(normalBulletPrefab, firePoint.position, Quaternion.identity);
        }
    }
    
    private void ShootZigzag()
    {
        if (zigzagBulletPrefab != null && firePoint != null)
        {
            // Dispara 1 bala que zigzaguea
            Instantiate(zigzagBulletPrefab, firePoint.position, Quaternion.identity);
        }
    }
    
    private void ShootShotgun()
    {
        if (normalBulletPrefab != null && firePoint != null)
        {
            // Dispara 5 balas en abanico
            int bulletCount = 5;
            float spreadAngle = 40f; // Ángulo total del abanico
            
            for (int i = 0; i < bulletCount; i++)
            {
                // Calcular ángulo de cada bala
                float angle = -spreadAngle / 2 + (spreadAngle / (bulletCount - 1)) * i;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                
                GameObject bullet = Instantiate(normalBulletPrefab, firePoint.position, rotation);
                
                // Ajustar dirección
                PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
                if (bulletScript != null)
                {
                    Vector3 direction = rotation * Vector3.up;
                    bulletScript.SetDirection(direction);
                }
            }
        }
    }
    
    private void ShootRockets()
    {
        if (rocketBulletPrefab != null && firePoint != null)
        {
            // Dispara 1 cohete (más lento pero más daño)
            GameObject rocket = Instantiate(rocketBulletPrefab, firePoint.position, Quaternion.identity);
            
            // Los cohetes hacen más daño
            PlayerBullet bulletScript = rocket.GetComponent<PlayerBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDamage(damage + 1); // +1 de daño extra
            }
        }
    }
    
    // Método público para cambiar arma
    public void ChangeWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"Arma cambiada a: {newWeapon}");
    }
    
    public WeaponType GetCurrentWeapon() => currentWeapon;
}