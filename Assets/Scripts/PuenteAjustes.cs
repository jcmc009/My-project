using UnityEngine;

public class PuenteAjustes : MonoBehaviour
{
    public void ClickActivarMusica() { GameManager.Instance.ReproducirMusica(); }
    public void ClickDesactivarMusica() { GameManager.Instance.DetenerMusica(); }
    
    public void ClickActivarEfectos() { GameManager.Instance.ActivarEfectos(); }
    public void ClickDesactivarEfectos() { GameManager.Instance.DesactivarEfectos(); }

    public void ClickVolverMenu() { GameManager.Instance.VolverAlMenuPrincipal(); }
}