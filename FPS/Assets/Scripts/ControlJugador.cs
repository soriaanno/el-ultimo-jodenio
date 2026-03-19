using UnityEngine;
using System;
//using UnityEngine.UIElements;

public class ControlJugador : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float sensibilidad;

    private Rigidbody rb;
    private controlArma arma;
    private BoxCollider limites;

    [Header("C�mara")]
    private Camera cam;
    private float rotacionHorizontal, rotacionVertical;
    public float limiteVertical;

    [Header("Movimiento")]
    public float velocidad;
    private bool tocandoSuelo = true;
    public float fuerzaSalto;
    
    void Start()
    {
        cam = Camera.main; //se podria poner en Awake al lanzar la escena
        rb = GetComponent<Rigidbody>();
        arma = GetComponent<controlArma>();
        limites = GameObject.FindGameObjectWithTag("Suelo").GetComponent<BoxCollider>();

        //bloqueo cursor
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        VistaCamara();
        Movimiento();
        //una pulsacion = 1 bala, si pngo GetButton se evalua cada frame
        if (Input.GetButton("Fire1") && arma.PuedeDisparar())
        {
            arma.Disparar();
        }
        if (Input.GetKeyDown(KeyCode.Space) && TocandoSuelo())
        {
            Salto();
        }
    }


    private void VistaCamara()
    {
        //leemos los valores de rat�n y le aplicamos sensibilidad (3 � 4)
        //se puede hacert t real multiplicando por Time.deltaTime. Con la conmfiguraci�n actual habr�a que aumentar la sensibilidad
        float x = Input.GetAxis("MouseX") * sensibilidad;
        float y = Input.GetAxis("MouseY") * sensibilidad;

        //calculamos la rotaci�n en funci�n de x e y

        rotacionHorizontal += x;
        rotacionVertical -= y;


        //rotacion vertical. Se limita cu�nto puedo mirar hacia arriba/abajo
        rotacionVertical = Mathf.Clamp(rotacionVertical, -limiteVertical, limiteVertical);
        cam.transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);

        //rotaci�n horizontal
        //transform.localRotation = Quaternion.Euler(0, rotacionHorizontal, 0);
        transform.localEulerAngles = Vector3.up * rotacionHorizontal;



    }

    private void Movimiento()
    {
        float x = Input.GetAxis("Horizontal") * velocidad;
        float z = Input.GetAxis("Vertical") * velocidad;

        //avance y salto
        Vector3 direccion;
        direccion = transform.right * x + transform.forward * z;
        rb.linearVelocity = new Vector3(direccion.x, rb.linearVelocity.y, direccion.z);

        //movimiento solo hasta los limites del suelo
        Vector3 pos = rb.position;
        Bounds b = limites.bounds;
        pos.x= Mathf.Clamp(pos.x, b.min.x, b.max.x);
        pos.z= Mathf.Clamp(pos.z, b.min.z, b.max.z);
        rb.MovePosition (pos);



    }
    private bool TocandoSuelo()
    {
      //hacemos un raycast hacia abajo para comprobar si estamos tocando el suelo
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
    
    private void Salto()
    {
           rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
    }
}
    /*
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Suelo"))
            tocandoSuelo = true;
    }

    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.gameObject.CompareTag("Suelo"))
    //        tocandoSuelo = false;
    //}

}
    */
