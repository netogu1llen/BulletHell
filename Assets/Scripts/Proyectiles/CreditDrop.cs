using UnityEngine;

public class CreditDrop : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float fallSpeed = 3f;
    [SerializeField] private float horizontalVariation = 0.5f; // Variación horizontal al caer
    
    [Header("Configuración")]
    [SerializeField] private int creditValue = 1;
    [SerializeField] private float lifetime = 8f; // Tiempo antes de desaparecer
    
    [Header("Visual")]
    [SerializeField] private float rotationSpeed = 100f; // Gira mientras cae
    
    private float spawnTime;
    private Vector3 horizontalDirection;
    
    void Start()
    {
        spawnTime = Time.time;
        
        // Pequeña variación horizontal aleatoria al caer
        horizontalDirection = new Vector3(Random.Range(-horizontalVariation, horizontalVariation), 0, 0);
    }
    
    void Update()
    {
        // Caer hacia abajo con ligero movimiento horizontal
        transform.Translate((Vector3.down * fallSpeed + horizontalDirection) * Time.deltaTime, Space.World);
        
        
        // Desaparecer si pasa mucho tiempo
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
        
        // Desaparecer si sale de pantalla por abajo
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // El jugador recoge el crédito
            CurrencyManager.Instance?.AddCredits(creditValue);
            
            // Efecto visual opcional (después podemos agregar partículas)
            Debug.Log($"¡Crédito recogido! +{creditValue}");
            
            Destroy(gameObject);
        }
    }
}