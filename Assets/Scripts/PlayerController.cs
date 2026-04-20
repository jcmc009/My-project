using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 5f;
[Header("Efectos de Sonido")]
    public AudioClip coinSoundEffect;  
    private AudioSource audioSource; // El "altavoz" del zorro

    private Rigidbody2D rb;
    private float movimientoHorizontal = 0f;
    private bool enSuelo = true; 
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private int cherriesCount=0;
[Header("Cherries count")]
    [SerializeField] private TextMeshProUGUI cherryCountText;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
	audioSource = GetComponent<AudioSource>();
	cherryCountText.text=cherriesCount.ToString();
    }

    void Update()
    {
        // Variables temporales para este frame
        float nuevoMovimiento = 0f; 
        bool quiereSaltar = false;

        // --- GESTIÓN DE ANIMACIONES ---
        animator.SetBool("isRunning", rb.linearVelocityX != 0); 
        
        // SALTO: No está en el suelo y su velocidad Y es positiva (va hacia arriba)
        animator.SetBool("isJumping", !enSuelo && rb.linearVelocity.y > 0.1f);
        
        // CAÍDA: No está en el suelo y su velocidad Y es negativa (va hacia abajo)
        animator.SetBool("isFalling", !enSuelo && rb.linearVelocity.y < -0.1f);


        // 1. Comprobar si hay dedos tocando la pantalla
        if (Input.touchCount > 0)
        {
            // Recorremos todos los dedos en la pantalla
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch toque = Input.GetTouch(i);

                // --- MOVIMIENTO LATERAL (Dividimos la pantalla por la mitad) ---
                if (toque.position.x < Screen.width / 2f)
                {
                    nuevoMovimiento = -1f; // Mitad izquierda
                    spriteRenderer.flipX = true;
                }
                else if (toque.position.x > Screen.width / 2f) 
                {
                    nuevoMovimiento = 1f;  // Mitad derecha
                    spriteRenderer.flipX = false;
                }

                // --- SALTO (Swipe hacia arriba con 2 dedos) ---
                if (Input.touchCount > 1) 
                {
                    if (toque.phase == TouchPhase.Moved)
                    {
                        // Si cualquiera de los dedos desliza hacia arriba rápidamente
                        if (toque.deltaPosition.y > 10f)
                        {
                            quiereSaltar = true;
                        }
                    }
                }
            }
        }

        // Asignamos el movimiento calculado (evita el bug de sobrescritura multitáctil)
        movimientoHorizontal = nuevoMovimiento;

        // Ejecutamos el salto si deslizó el dedo Y está tocando el suelo
        if (quiereSaltar && enSuelo)
        {
            Debug.Log("🚀 ¡Saltando con swipe! Fuerza aplicada: " + fuerzaSalto);
            Saltar();
        }
    }

    void FixedUpdate()
    {
        // Aplicamos el cálculo a las físicas reales del personaje
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    void Saltar()
    {
        // Reseteamos la Y a 0 por seguridad y aplicamos el impulso
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        enSuelo = false;
    }

    // --- DETECCIÓN DE SUELO ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }

    }
// --- DETECCIÓN DE COLECCIONABLES Y DAÑO (Triggers) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si tocamos un coleccionable
        if (collision.gameObject.CompareTag("collectible"))
        {
            if (coinSoundEffect!= null)
          {
                audioSource.PlayOneShot(coinSoundEffect);
		
            }
	    Debug.Log("cereza conseguida"); 
		cherriesCount++;
		cherryCountText.text=cherriesCount.ToString();

            // 2. Destruimos el objeto de la escena
            Destroy(collision.gameObject);
        }
    }
}