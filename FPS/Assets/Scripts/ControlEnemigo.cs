using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ControlEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    public float rangoPersecucion; //distancia a la q empieza a seguir 
    public float rangoAtaque; //distancia a la q dispara
    public bool siemprePersigue; // Si true, persigue siempre aunque esté fuera del rangoPersecucion

    private NavMeshAgent agente;
    private controlArma arma;
    private ControlJugador objetivo;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        arma = GetComponent<controlArma>();
        objetivo = FindFirstObjectByType<ControlJugador>();

        agente.updateRotation = true; //el agente rota automaticamente
    }

    void Update()
    {
        if (objetivo == null) return;

        float distancia = Vector3.Distance(transform.position, objetivo.transform.position);

        if (distancia > rangoAtaque)
        {
            if (distancia < rangoPersecucion || siemprePersigue)
            {
                agente.SetDestination(objetivo.transform.position);
            }
        }
        else
        {
            agente.ResetPath();

            if (arma.PuedeDisparar())
            {
                arma.Disparar();
            }

            Vector3 direccion = objetivo.transform.position - transform.position;
            direccion.y = 0;

            float angulo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * angulo;
        }
    }
}
