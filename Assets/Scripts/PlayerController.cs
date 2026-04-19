using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
[Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;

    private Rigidbody2D rb;
    private float movimientoHorizontal = 0f;
    private bool enSuelo = true; // Para evitar saltos infinitos en el aire

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movimientoHorizontal = 0f; // Reiniciar el movimiento cada frame

        // 1. Comprobar si hay dedos tocando la pantalla usando Input.touchCount
        if (Input.touchCount > 0)
        {
            // Recorremos todos los toques por si el jugador usa dos dedos a la vez (ej: correr y saltar)
            for (int i = 0; i < Input.touchCount; i++)
            {
                // 2. Obtener la información del toque específico
                Touch toque = Input.GetTouch(i);

                // --- MOVIMIENTO LATERAL ---
                // 3. Usamos Touch.position para saber dónde está tocando
                if (toque.position.x < Screen.width /3)
                {
                    // Si el toque está en la mitad izquierda de la pantalla (en píxeles)
                   // movimientoHorizontal = -1f; 
                }
                else if (toque.position.x > Screen.width / 3)
                {
                    // Si el toque está en la mitad derecha
                    movimientoHorizontal = 1f; 
                }

                // --- SALTO (Swipe hacia arriba) ---
                if (toque.phase == TouchPhase.Moved)
                {
                    // Si el desplazamiento vertical (Y) es mayor que 20 píxeles y está en el suelo
                    if (toque.deltaPosition.y > 10f && enSuelo)
                    {
                        Debug.Log("🚀 ¡Saltando! Fuerza aplicada: " + fuerzaSalto);
                        Saltar();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        // Aplicamos el cálculo del Update a las físicas reales del personaje
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    void Saltar()
    {
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        enSuelo = false;
    }

    // --- DETECCIÓN DE SUELO ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Asegúrate de crear un Tag llamado "Suelo" y ponérselo a tus plataformas del Tilemap
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }
}
