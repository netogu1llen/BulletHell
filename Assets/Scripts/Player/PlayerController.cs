using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    private float moveSpeed = 8f;
    [SerializeField] private float slowModeMultiplier = 0.4f; // 40% de velocidad en modo lento
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    
    [Header("Modo Preciso")]
    [SerializeField] private KeyCode slowModeKey = KeyCode.LeftShift;
    private bool isSlowMode = false;
    
    [Header("Visual (Opcional)")]
    [SerializeField] private GameObject slowModeIndicator; // Visual opcional cuando está en modo lento
    
    void Start()
    {
        // Aplicar velocidad desde UpgradeManager
        if (UpgradeManager.Instance != null)
        {
            moveSpeed = UpgradeManager.Instance.GetMoveSpeed();
        }
        
        Camera mainCamera = Camera.main;
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z)
        );
        
        float margin = 0.5f;
        minX = -screenBounds.x + margin;
        maxX = screenBounds.x - margin;
        minY = -screenBounds.y + margin;
        maxY = screenBounds.y - margin;

        // Desactivar indicador visual si existe
        if (slowModeIndicator != null)
        {
            slowModeIndicator.SetActive(false);
        }
        
        Debug.Log($"Velocidad del jugador: {moveSpeed}");
    }
    
    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.IsGameplayActive())
        {
            return;
        }
        
        HandleSlowMode();
        HandleMovement();
    }
    
    private void HandleSlowMode()
    {
        // Detectar si se mantiene presionado Shift
        isSlowMode = Input.GetKey(slowModeKey);
        
        // Activar/desactivar indicador visual
        if (slowModeIndicator != null)
        {
            slowModeIndicator.SetActive(isSlowMode);
        }
    }
    
    private void HandleMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        // Calcular velocidad actual (normal o lenta)
        float currentSpeed = isSlowMode ? moveSpeed * slowModeMultiplier : moveSpeed;
        
        Vector3 newPosition = transform.position;
        newPosition.x += horizontalInput * currentSpeed * Time.deltaTime;
        newPosition.y += verticalInput * currentSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        
        transform.position = newPosition;
    }

    // Método público para actualizar velocidad desde upgrades
    public void UpdateSpeed()
    {
        if (UpgradeManager.Instance != null)
        {
            moveSpeed = UpgradeManager.Instance.GetMoveSpeed();
            Debug.Log($"Velocidad del jugador actualizada a: {moveSpeed}");
        }
    }
}