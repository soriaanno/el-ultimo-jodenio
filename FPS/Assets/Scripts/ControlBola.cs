using UnityEngine;

public class ControlBola : MonoBehaviour
{
    public float maxTiempo = 3f;
    public float danioAtaque = 25f;
    private float tiempoActivacion;

    private void OnEnable()
    {
        tiempoActivacion = Time.time;
    }

    void Update()
    {
        if (Time.time - tiempoActivacion >= maxTiempo)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Buscamos el componente Salud en lo que sea que hayamos golpeado
        Salud objetoConSalud = other.GetComponent<Salud>();

        if (objetoConSalud != null)
        {
            objetoConSalud.RecibirDanio(danioAtaque);
        }

        // La bala siempre se desactiva al chocar con algo
        gameObject.SetActive(false);
    }
}