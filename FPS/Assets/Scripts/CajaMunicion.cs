using UnityEngine;

public class CajaMunicion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Buscamos si el objeto que tocó la caja tiene control de arma
        controlArma armaJugador = other.GetComponentInParent<controlArma>();

        //sumamos 100 mas de municion al arma
        if (armaJugador != null)
        {
            armaJugador.reservaTotal += 100;
            //destruimos la caja de munición después de recogerla
            Destroy(gameObject);
        }

    }
}
