using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PantallaVIctoriaController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 5f;

    [Header("Efectos de Sonido")]
    public AudioClip coinSoundEffect;  
    public AudioClip jumpSoundEffect; 
    private AudioSource audioSource; 

    private Rigidbody2D rb;
    private float movimientoHorizontal = 0f;
    private bool enSuelo = true; 
    private SpriteRenderer spriteRenderer;
    private Animator animator;
     
    [Header("Interfaz (HUD)")]
    [SerializeField] private TextMeshProUGUI cherryCountText;
    [SerializeField] private TextMeshProUGUI liveCountText;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // --- 1. SINCRONIZAR TEXTOS CON EL GAMEMANAGER ---
        if (GameManager.Instance != null)
        {
            if (cherryCountText != null) 
                cherryCountText.text = GameManager.Instance.coleccionablesRecogidos.ToString();
                
            if (liveCountText != null) 
                liveCountText.text = GameManager.Instance.vidas.ToString();
        }

        // --- 2. LÓGICA DE MOVIMIENTO ---
        float nuevoMovimiento = 0f; 
        bool quiereSaltar = false;

        animator.SetBool("isRunning", rb.linearVelocityX != 0); 
        animator.SetBool("isJumping", !enSuelo && rb.linearVelocity.y > 0.1f);
        animator.SetBool("isFalling", !enSuelo && rb.linearVelocity.y < -0.1f);

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch toque = Input.GetTouch(i);

                if (toque.position.x < Screen.width / 2f)
                {
                    nuevoMovimiento = -1f; 
                    spriteRenderer.flipX = true;
                }
                else if (toque.position.x > Screen.width / 2f) 
                {
                    nuevoMovimiento = 1f;  
                    spriteRenderer.flipX = false;
                }

                if (Input.touchCount > 1) 
                {
                    if (toque.phase == TouchPhase.Moved)
                    {
                        if (toque.deltaPosition.y > 10f)
                        {
                            quiereSaltar = true;
                        }
                    }
                }
            }
        }

        movimientoHorizontal = nuevoMovimiento;

        if (quiereSaltar && enSuelo)
        {
            Saltar();
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidad, rb.linearVelocity.y);
    }

    void Saltar()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        enSuelo = false;
        
        if (jumpSoundEffect != null && audioSource != null)
        {
            audioSource.PlayOneShot(jumpSoundEffect);
        }
    }

    // --- CHOQUES FÍSICOS (Suelo y enemigos sólidos) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) 
        {
            enSuelo = true;
        }

        // NUEVO: Si chocamos físicamente contra un enemigo (como el escarabajo)
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestarVida();
                Debug.Log("¡Colisión con escarabajo! Vida restada.");
            }
		Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) enSuelo = true;
    }

    // --- CHOQUES FANTASMA (Cerezas y enemigos que son Trigger) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("collectible"))
        {
            if (coinSoundEffect != null && audioSource != null)
            {
                audioSource.PlayOneShot(coinSoundEffect);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.coleccionablesRecogidos++;
            }

            Destroy(collision.gameObject);
        }

        // NUEVO: Por si configuras algún enemigo volador como Trigger
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RestarVida();
                Debug.Log("¡Un enemigo me ha tocado! Vida restada.");
		Destroy(collision.gameObject);
            }
        }
    }
}