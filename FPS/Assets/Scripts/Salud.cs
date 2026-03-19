using UnityEngine;
using UnityEngine.Events;

public class Salud : MonoBehaviour
{
    [Header("PowerUps")]
    public bool invencible = false; // Si true, no recibe daño

    [Header("Configuración de Vida")]
    public float vidaMaxima = 100f;
    public float vidaActual; // Vida actual del personaje

    [Header("Eventos (Opcional)")]
    public UnityEvent alMorir;

    void Awake()
    {
        // Al iniciar la vida es completa
        vidaActual = vidaMaxima;
    }

    // Función para recibir daño
    public void RecibirDanio(float cantidad)
    {
        if (invencible) return; // Si es invencible, no hace nada

        vidaActual -= cantidad; // Resta la vida
        Debug.Log(gameObject.name + " tiene " + vidaActual + " de vida.");

        //Si la vida llega a cero muere
        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    // maneja la muerte 
    private void Morir()
    {
        ControladorHUD hud = FindFirstObjectByType<ControladorHUD>(); // Encuentra el HUD en la escena

        if (CompareTag("Enemigo"))
        {
            if (hud != null)
            {
                hud.EnemigoEliminado(); // Notifica al HUD que un enemigo murió
            }

            gameObject.SetActive(false); // Desactiva al enemigo
        }

        if (CompareTag("Player"))
        {
            if (hud != null)
            {
                hud.FinalizarPartida(false); // Finaliza la partida si es el jugador
            }
        }

        alMorir.Invoke(); 
    }
}
