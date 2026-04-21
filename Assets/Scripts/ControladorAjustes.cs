using UnityEngine;

public class ControladorAjustes : MonoBehaviour
{
    // Este script es solo un "mando a distancia" que se comunica con el GameManager real

    public void BotonEncenderMusica()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.ReproducirMusica();
        }
    }

    public void BotonApagarMusica()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.DetenerMusica();
        }
    }

    public void BotonVolver()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.VolverAlMenuPrincipal();
        }
    }
    public void BotonIniciarJuego()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.IniciarJuego();
        }
    }
}