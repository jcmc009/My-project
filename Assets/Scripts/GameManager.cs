using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    // 1. Crear la instancia estática (Patrón Singleton)
    public static GameManager Instance;

    // 2. Variables Globales
    [Header("Estado del Jugador")]
    public int vidas = 3;
    public int coleccionablesRecogidos = 0;

    [Header("Gestión de Audio")]
    public AudioSource musicaFondo;
    public bool musicaPermitida = true; // Por defecto, la música está permitida al abrir el juego

    [Header("Pantallas de Fin de Juego")]
    public GameObject pantallaDerrota;
    public GameObject pantallaVictoria;
    public TextMeshProUGUI textoResumenVidas;
    public TextMeshProUGUI textoResumenColeccionables;

    private void Awake()
    {
        // 3. Configurar el Singleton y el DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

            // Buscar el reproductor de música automáticamente
            if (musicaFondo == null)
            {
                GameObject objetoMusica = GameObject.Find("Audio Source Menú principal");
                
                if (objetoMusica != null)
                {
                    musicaFondo = objetoMusica.GetComponent<AudioSource>();
                    //Debug.Log("¡Reproductor de música enlazado automáticamente!");
                }
                else
                {
                    Debug.LogError("❌ No encuentro el 'Audio Source Menú principal'. Revisa mayúsculas, minúsculas o espacios extra.");
                }
            }
        }
        else
        {
            // Si ya existe un GameManager, destruye este duplicado
            Destroy(gameObject); 
        }
    }
    // --MÉTODO CARGAR NIVEL--
 public void cargarNivel()
    {
        SceneManager.LoadScene("Juego");
    }

 
    // --- MÉTODOS DE GESTIÓN DE MÚSICA ---

    public void ReproducirMusica()
    {
        musicaPermitida = true; // El jugador SÍ quiere música

        // RED DE SEGURIDAD: Si hemos perdido el altavoz, lo buscamos de nuevo
        if (musicaFondo == null)
        {
            GameObject objetoMusica = GameObject.Find("Audio Source Menú principal");
            if (objetoMusica != null)
            {
                musicaFondo = objetoMusica.GetComponent<AudioSource>();
            }
        }

        // Si por fin tenemos altavoz y no está sonando ya, le damos al Play
        if (musicaFondo != null && !musicaFondo.isPlaying)
        {
            musicaFondo.volume = 1;
            musicaFondo.Play();
            Debug.Log("▶️ Reproduciendo música con volumen al máximo");
        }
    }

    public void DetenerMusica()
    {
        musicaPermitida = false; // El jugador NO quiere música

        // RED DE SEGURIDAD: Lo buscamos por si se ha perdido
        if (musicaFondo == null)
        {
            GameObject objetoMusica = GameObject.Find("Audio Source Menú principal");
            if (objetoMusica != null)
            {
                musicaFondo = objetoMusica.GetComponent<AudioSource>();
            }
        }

        // Si tenemos altavoz, lo callamos
        if (musicaFondo != null)
        {
            musicaFondo.volume = 0; 
            musicaFondo.Pause(); // Usamos Pause en lugar de Stop para que cuando vuelva a sonar, siga por donde iba
            Debug.Log("⏹️ ¡Música detenida con éxito!");
        }
    }

    public void CambiarMusica(AudioClip nuevaPista)
    {
        if (musicaFondo != null)
        {
            musicaFondo.Stop();
            musicaFondo.clip = nuevaPista;
            
            if (musicaPermitida) 
            {
                musicaFondo.volume = 1;
                musicaFondo.Play();
            }
        }
    }

    public void ComprobarEstadoMusica()
    {
        if (musicaFondo == null)
        {
            GameObject objetoMusica = GameObject.Find("Audio Source Menú principal");
            if (objetoMusica != null)
            {
                musicaFondo = objetoMusica.GetComponent<AudioSource>();
            }
        }

        if (musicaFondo != null)
        {
            if (musicaPermitida && !musicaFondo.isPlaying)
            {
                musicaFondo.volume = 1;
                musicaFondo.Play();
                Debug.Log("✅ Comprobación: La música debe sonar y se ha reactivado.");
            }
            else if (!musicaPermitida) 
            {
                musicaFondo.volume = 0;
                musicaFondo.Stop();
                Debug.Log("✅ Comprobación: La música NO debe sonar y se ha silenciado por la fuerza.");
            }
        }
    }

    // --- MÉTODOS DE FIN DE PARTIDA ---

    public void MostrarGameOver()
    {
        pantallaDerrota.SetActive(true); // Enciende la pantalla
        Time.timeScale = 0f; // Congela el tiempo del juego para que los enemigos se paren
    }

    public void MostrarVictoria()
    {
        pantallaVictoria.SetActive(true); // Enciende la pantalla
        Time.timeScale = 0f; // Congela el juego
        
        // Actualiza el resumen de la partida
        textoResumenVidas.text = "Vidas restantes: " + vidas;
        textoResumenColeccionables.text = "Coleccionables: " + coleccionablesRecogidos;
    }

    public void ReintentarNivel()
    {
        Time.timeScale = 1f; // Descongela el tiempo
        vidas = 3; // Resetea las vidas
        coleccionablesRecogidos = 0; // Resetea las monedas
        
        // Recarga la escena en la que estás ahora mismo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void VolverAlMenuPrincipal()
{
    SceneManager.LoadScene("MenuPrincipal"); 
 Debug.Log("Botón pulsado"); 
}

    // --- MÉTODO PARA SALIR DEL JUEGO ---
    public void SalirJuego()
    {
	Application.Quit(); 
        Debug.Log("Cerrando el juego..."); 
        
    }
  public void IniciarJuego()
    {
        Time.timeScale = 1f; // Descongela el tiempo
        SceneManager.LoadScene("Nivel"); 
 Debug.Log("Botón pulsado"); 
    }
  public void cargarCreditos()
    {
//        Time.timeScale = 1f; // Descongela el tiempo
        SceneManager.LoadScene("Creditos"); 
 Debug.Log("Botón pulsado"); 
    }
 public void cargarAjustes()
    {
//        Time.timeScale = 1f; // Descongela el tiempo
        SceneManager.LoadScene("Ajustes"); 
 Debug.Log("Botón pulsado"); 
    }
}