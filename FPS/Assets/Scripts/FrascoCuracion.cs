using UnityEngine;

public class FrascoCuracion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //buscamos si lo que tocó el frasco tiene el componente Salud
        Salud saludEntidad = other.GetComponent<Salud>();

        //si lo tiene...
        if (saludEntidad != null)
        {
            //...reiniciamos la vida al máximo
            saludEntidad.vidaActual = saludEntidad.vidaMaxima;

            Debug.Log(other.name + " ha recuperado su vida por completo.");

            // Destruimos el frasco para que no se pueda usar d nuevo
            Destroy(gameObject);
        }
    }
}