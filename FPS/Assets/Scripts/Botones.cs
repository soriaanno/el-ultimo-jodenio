using UnityEngine;
using UnityEngine.SceneManagement; // para gestionar escenas

public class Botones : MonoBehaviour
{
    //Métodos para botones

    public void ClickVolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void ClickSalir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    public void ClickGameplay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");
    }

    public void ClickCreditos()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Creditos");
    }

    public void ClickSinopsis()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Sinopsis");
    }
}