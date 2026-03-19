using UnityEngine;

public class controlArma : MonoBehaviour
{
    public PoolObjetos poolBola;
    public Transform puntoSalida;

    [Header("Configuración de Munición")]
    public bool municionIlimitada;
    public int capacidadCargador = 25; // Balas que caben en el arma
    public int municionActual;         // Balas que hay ahora mismo en el arma
    public int reservaTotal = 100;     // Balas totales fuera del arma

    [Header("Ajustes de Disparo")]
    public float velocidadBola;
    public float cadenciaDisparo;
    private float tiempoUltimoDisparo;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoDisparo;
    public AudioClip sonidoRecarga;


    private bool soyJugador;

    void Awake()
    {
        soyJugador = GetComponent<ControlJugador>() ? true : false;
        poolBola = GetComponent<PoolObjetos>();

        //el arma está llena cd empiezas
        municionActual = capacidadCargador;
    }

    void Update()
    {
        //si se presiona la R recarga
        if (soyJugador && Input.GetKeyDown(KeyCode.R))
        {
            Recargar();
        }
    }

    public bool PuedeDisparar()
    {
        if (Time.time - tiempoUltimoDisparo >= cadenciaDisparo)
        {
            if (municionIlimitada || municionActual > 0)
            {
                return true;
            }
            else if (soyJugador && municionActual <= 0 && reservaTotal > 0)
            {
                //si t quedas sin balas e intentas disparar la recarga es automatica
                Recargar();
            }
        }
        return false;
    }

    public void Disparar()
    {
        if (!PuedeDisparar()) return;

        tiempoUltimoDisparo = Time.time;

        if (!municionIlimitada)
        {
            municionActual--;
        }

        //sonido d disparo
        if (audioSource != null && sonidoDisparo != null)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f); // variación de tono
            audioSource.PlayOneShot(sonidoDisparo, 0.3f); // volumen más bajo (30%)
        }

        GameObject bola = poolBola.CogerObjeto();
        if (bola != null)
        {
            bola.transform.position = puntoSalida.position;
            bola.transform.rotation = puntoSalida.rotation;

            Rigidbody rb = bola.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = puntoSalida.forward * velocidadBola;
            }
        }
    }

    public void Recargar()
    {
        // Si ya está lleno o no hay reserva, no sale nada
        if (municionActual == capacidadCargador || reservaTotal <= 0) return;

        int balasNecesarias = capacidadCargador - municionActual;

        if (reservaTotal >= balasNecesarias)
        {
            reservaTotal -= balasNecesarias;
            municionActual = capacidadCargador;
        }
        else
        {
            municionActual += reservaTotal;
            reservaTotal = 0;
        }

        // sonido recarga
        if (audioSource != null && sonidoRecarga != null)
        {
            audioSource.PlayOneShot(sonidoRecarga);
        }

        Debug.Log("Recargado. Balas: " + municionActual + " Reserva: " + reservaTotal);
    }

}
