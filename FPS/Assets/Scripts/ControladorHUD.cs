using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Controla todo el HUD: salud, munición, pausa, fin de partida y botones
public class ControladorHUD : MonoBehaviour
{
    [Header("Configuración de Referencias")]
    public GameObject jugador; // Referencia al jugador

    [Header("Configuración de Pausa y Fin de Juego")]
    public GameObject objetoMenuPausa;    // Panel de pausa
    public GameObject objetoPantallaFinal; // Panel de fin de partida
    private bool estaPausado = false;
    private bool juegoTerminado = false;

    [Header("Textos Pantalla Final")]
    public Text textoEstado;   // "MISIÓN CUMPLIDA" o "FALLIDO"
    public Text textoSector;   // Nombre del sector
    public Text textoBajas;    // Bajas y puntos del jugador

    [Header("Configuración de Corazones")]
    public GameObject[] contenedoresCorazones; // Corazones en el HUD
    public Sprite corazonLleno;
    public Sprite corazonMitad;
    public Sprite corazonVacio;
    public Salud scriptSaludJugador; // Script de salud del jugador

    [Header("Configuración de Munición")]
    public Text textoMunicion;
    public controlArma scriptArmaJugador; // Script del arma del jugador

    [Header("Puntuación")]
    public int enemigosEliminados = 0; // Conteo de enemigos eliminados
    private int enemigosTotales;

    void Start()
    {
        // Inicializa estado del HUD
        estaPausado = false;
        juegoTerminado = false;

        if (objetoMenuPausa != null) objetoMenuPausa.SetActive(false);
        if (objetoPantallaFinal != null) objetoPantallaFinal.SetActive(false);

        Time.timeScale = 1f; // Juego activo
        Cursor.lockState = CursorLockMode.Locked; // Oculta cursor
        Cursor.visible = false;

        enemigosTotales = GameObject.FindGameObjectsWithTag("Enemigo").Length; // Cuenta enemigos al inicio
    }

    void Update()
    {
        // pausa con Escape
        if (Input.GetKeyDown(KeyCode.Escape) && !juegoTerminado)
            AlternarPausa();

        if (!estaPausado && !juegoTerminado)
        {
            ActualizarCorazones(); // Actualiza HUD de vida
            ActualizarMunicion();   // Actualiza HUD de munición
        }
    }

    // Se llama cuando un enemigo muere
    public void EnemigoEliminado()
    {
        enemigosEliminados++;

        // Si matas a todos los enemigos se finaliza la partida con victoria
        if (enemigosEliminados >= enemigosTotales)
            FinalizarPartida(true);
    }

    // Finaliza la partida, victoria o derrota
    public void FinalizarPartida(bool victoria)
    {
        juegoTerminado = true;
        Time.timeScale = 0f; // Pausa el juego

        objetoPantallaFinal.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (jugador != null)
            jugador.GetComponent<ControlJugador>().enabled = false; // Desactiva control del jugador

        int puntos = enemigosEliminados * 10;

        if (victoria)
        {
            if (MusicManager.instancia != null)
                MusicManager.instancia.ReproducirVictoria();

            textoEstado.text = "ESTADO: MISIÓN CUMPLIDA";
            textoSector.text = "SECTOR JÓDAR: ASEGURADO";
            textoBajas.text = "HAS ANIQUILADO A " + enemigosEliminados + " SIONISTAS\n" +
                              "PUNTOS: " + puntos;
        }
        else
        {
            if (MusicManager.instancia != null)
                MusicManager.instancia.ReproducirDerrota();

            textoEstado.text = "ESTADO: FALLIDO";
            textoSector.text = "SECTOR JÓDAR: BAJO CONTROL SIONISTA";
            textoBajas.text = "FUISTE ELIMINADO EN COMBATE\n" +
                              "BAJAS: " + enemigosEliminados + " (PUNTOS: " + puntos + ")";
        }
    }

    // Alterna pausa
    public void AlternarPausa()
    {
        // Cambia el valor de "estaPausado": si estaba true, pasa a false y viceversa
        estaPausado = !estaPausado;
        Time.timeScale = estaPausado ? 0f : 1f;
        objetoMenuPausa.SetActive(estaPausado);

        //Control del cursor: visible y libre en pausa, oculto y bloqueado en gameplay
        Cursor.lockState = estaPausado ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = estaPausado;

        // desactiva el control del jugador para que no pueda moverse mientras está pausado
        if (jugador != null)
            jugador.GetComponent<ControlJugador>().enabled = !estaPausado;


        // Música de pausa o gameplay sin música
        if (MusicManager.instancia != null)
        {
            if (estaPausado)
                MusicManager.instancia.ReproducirPausa();
            else
                MusicManager.instancia.audioSource.Stop(); // Detiene música al reanudar
        }
    }

    // Actualiza los corazones del HUD según la vida del jugador
    void ActualizarCorazones()
    {
        if (scriptSaludJugador == null) return;

        //recorre todos los contenedores de corazones del HUD
        for (int i = 0; i < contenedoresCorazones.Length; i++)
        {
            // Obtiene la imagen del corazón dentro del contenedor
            RawImage imagenCorazon = contenedoresCorazones[i].GetComponentInChildren<RawImage>();
            if (imagenCorazon == null) continue; // Si no hay imagen, salta al siguiente

            // Cada corazón representa 2 de vida
            float valorCorazon = (i * 2) + 1; // ejemplo: corazón 0 = 1, corazón 1 = 3, etc.

            // Determina qué sprite mostrar según la vida actual del jugador
            if (scriptSaludJugador.vidaActual >= valorCorazon + 1)
                imagenCorazon.texture = corazonLleno.texture; // Vida completa: corazón lleno
            else if (scriptSaludJugador.vidaActual == valorCorazon)
                imagenCorazon.texture = corazonMitad.texture; // Vida a la mitad: corazón medio
            else
                imagenCorazon.texture = corazonVacio.texture; // Sin vida: corazón vacío
        }
    }


    // Actualiza la munición 
    void ActualizarMunicion()
    {
        if (scriptArmaJugador == null || textoMunicion == null) return;

        textoMunicion.text = !scriptArmaJugador.municionIlimitada
            ? scriptArmaJugador.municionActual + "/" + scriptArmaJugador.reservaTotal
            : "∞ / ∞";
    }

    // Funciones para botones del HUD
    public void ClickReiniciar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reinicia la escena
    }

    public void ClickReanudar()
    {
        AlternarPausa(); //quita la pausa
    }

    public void ClickVolverAlMenu()
    {
        Time.timeScale = 1f;
        if (MusicManager.instancia != null)
            MusicManager.instancia.ReproducirMenu();

        SceneManager.LoadScene("Menu");
    }

    public void ClickSalir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
