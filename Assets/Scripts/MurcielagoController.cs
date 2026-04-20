using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MurcielagoController : MonoBehaviour
{
    [Header("Ajustes de Vuelo")]
    public float speed = 3f;
    public float distance = 4f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    
    // Aquí guardaremos las "paredes invisibles" de su patrulla
    private float leftLimit;
    private float rightLimit;
    
    // 1 = Derecha, -1 = Izquierda
    private int direction = 1; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Al nacer, el murciélago calcula hasta dónde puede llegar
        leftLimit = transform.position.x - distance;
        rightLimit = transform.position.x + distance;
    }

    void FixedUpdate()
    {
        // 1. Volar a velocidad constante en el eje X
        // Mantenemos la Y intacta por si tiene gravedad, o se quedará en 0 si es un murciélago volador sin gravedad
        rb.linearVelocity = new Vector2(speed * direction, rb.linearVelocity.y);

        // 2. Si choca contra su límite derecho, le decimos que vaya a la izquierda
        if (transform.position.x >= rightLimit)
        {
            direction = -1;
        }
        // 3. Si choca contra su límite izquierdo, le decimos que vaya a la derecha
        else if (transform.position.x <= leftLimit)
        {
            direction = 1;
        }

        // 4. Girar el dibujo del murciélago según hacia dónde vuela
        if (direction == 1)
        {
            spriteRenderer.flipX = false; // Mirar a la derecha
        }
        else
        {
            spriteRenderer.flipX = true;  // Mirar a la izquierda
        }
    }
// --- DETECCIÓN DE CHOQUE CON EL JUGADOR ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificamos si lo que hemos tocado es el Jugador (el zorro)
        if (collision.CompareTag("Player"))
        {
            // 2. Le decimos al GameManager que reste una vida
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestarVida();
            }

            // 3. El murciélago desaparece (se destruye a sí mismo)
            Destroy(gameObject);
            
            Debug.Log("¡El murciélago ha golpeado al zorro!");
        }
    }
}