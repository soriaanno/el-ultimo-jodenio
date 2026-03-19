using UnityEngine;
using System.Collections;

public class FrascoRojoPowerUp : MonoBehaviour
{
    public float duracion = 7f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //iniciamos corrutina
            StartCoroutine(ActivarPowerUp(other.gameObject));

            //eliminamosla línea de SetActive(false) de aquí.
            // gameObject.SetActive(false); <etsa linea da error
        }
    }

    IEnumerator ActivarPowerUp(GameObject jugador)
    {
        Salud salud = jugador.GetComponent<Salud>();
        controlArma arma = jugador.GetComponent<controlArma>();

        //se activan los efectos
        if (salud != null) salud.invencible = true;
        if (arma != null) arma.municionIlimitada = true;

        Debug.Log("¡MODO DIOS INICIADO!");

        // el frasco desaparece y no se pueda tocar de nuevo
        // Usamos TryGetComponent por seguridad
        if (TryGetComponent<MeshRenderer>(out MeshRenderer render)) render.enabled = false;
        if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;

        //tiempo exacto
        yield return new WaitForSeconds(duracion);

        // quitar efectos
        if (salud != null) salud.invencible = false;
        if (arma != null) arma.municionIlimitada = false;

        Debug.Log("¡MODO DIOS TERMINADO!");

        //destruimos el objeto
        Destroy(gameObject);
    }
}