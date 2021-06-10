using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float tiempoAtaque;
    public GameObject target;
    public GameObject escudo;
    public GameObject[] hitboxAtaque; //0: Arriba, 1: Centro, 2: Abajo
    public Collider[] hitboxPersonaje; //0: De pie, 1: Agachado

    Rigidbody rb;
    Animation animacion;
    bool enElSuelo; //Si esta en contacto con el suelo
    bool atacando; //Si esta atacando
    bool protegido; //Si se esta protegiendo
    float temporizadorAtaque; //Para el ataque

    public float distanciaMin; //Cuanto se puede acercar al jugador
    public float distanciaMax; //Cuanto se puede alejar del jugador
    public float tiempoNextMov; //Tiempo que tarda en pensar el siguiente movimiento
    public float tiempoNextAc; //Tiempo que tarda en pensar la siguiente accion
    float temporizadorMovimiento;
    float temporizadorAccion;
    Movimientos movimientoActual;
    Acciones accionActual;

    enum Movimientos { Acercarse, Alejarse, Quieto, TotalMovimientos }
    enum Acciones { Agacharse, Levantarse, Saltar, Atacar, Protegerse, Nada, TotalAcciones }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animacion = GetComponentInChildren<Animation>();
        atacando = false;
        protegido = false;
        temporizadorAtaque = 0;

        //Estado actual
        temporizadorMovimiento = 0;
        temporizadorAccion = 0;
        movimientoActual = Movimientos.Quieto;
        accionActual = Acciones.Nada;
    }

    void Update() 
    {
        //Elige y realiza siguiente accion
        if (temporizadorAccion == 0)
        {
            SiguienteAccion();
            RealizaAccion();
        }
        temporizadorAccion += Time.deltaTime;
        if (temporizadorAccion >= tiempoNextAc)
        {
            temporizadorAccion = 0;
        }

        //Si esta ejecutando un ataque
        if (atacando) {
            temporizadorAtaque += Time.deltaTime;

            if (temporizadorAtaque >= tiempoAtaque)
            { //Si ha pasado el tiempo de ataque
                atacando = false;
                temporizadorAtaque = 0;

                foreach (GameObject aux in hitboxAtaque)
                { //Desactivar hitbox activas
                    aux.SetActive(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        RealizaMovimiento();

        //Elige siguiente movimiento
        if (temporizadorMovimiento == 0)
            SiguienteMovimiento();

        temporizadorMovimiento += Time.deltaTime;
        if (temporizadorMovimiento >= tiempoNextMov)
        {
            temporizadorMovimiento = 0;
        }
    }


    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    void SiguienteAccion() //TODO cambiar
    {
        //accionActual = (Acciones)Random.Range(0, (int)Acciones.TotalAcciones);
    }

    //Decide cual es la mejor opción de movimiento
    void SiguienteMovimiento()
    {
        int help = Random.Range(0, 100); //Calculo de probabilidades para los casos
        Vector3 miPosicion = transform.position;
        Vector3 posicionAdversario = target.transform.position; //Posicion del jugador (target)

        if (Vector3.Distance(miPosicion, posicionAdversario) <= distanciaMin) //Si esta muy cerca
        {
            if (help < 80) //80%
                movimientoActual = Movimientos.Alejarse;
        }
        else if (Vector3.Distance(miPosicion, posicionAdversario) >= distanciaMax) //Si esta muy lejos
        {
            if (help < 80) //80%
                movimientoActual = Movimientos.Acercarse;
        }
        else
        {
            if(help < 20) //Probabilidad 20% : Se acerca al jugador
            {
                movimientoActual = Movimientos.Acercarse;
            }
            else if (help >= 20 && help < 40) //Probabilidad 20% : Se aleja del jugador
            {
                movimientoActual = Movimientos.Alejarse;
            }
            else //Probabilidad 60% : Se queda quieto en el sitio
            {
                movimientoActual = Movimientos.Quieto;
            }
        }
    }

    void RealizaAccion()
    {
        switch (accionActual) //TODO quitar pruebas
        {
            case Acciones.Agacharse:
                if (enElSuelo) Agacharse();
                Debug.Log("Me agacho");
                break;

            case Acciones.Levantarse:
                Levantarse();
                Debug.Log("Me levanto");
                break;

            case Acciones.Saltar:
                if (enElSuelo) Saltar();
                Debug.Log("Boing!");
                break;

            case Acciones.Atacar:
                if (!atacando) Atacar(Random.Range(0, 3));
                Debug.Log("Ataco");
                break;

            case Acciones.Protegerse:
                if (!atacando && !protegido && enElSuelo) Protegerse();
                Debug.Log("Me protejo");
                break;

            case Acciones.Nada:
                Nada();
                Debug.Log("Nadita");
                break;

            default:
                break;
        }
    }

    void RealizaMovimiento()
    {
        switch (movimientoActual)
        {
            case Movimientos.Acercarse:
                if (!protegido) Acercarse();
                break;

            case Movimientos.Alejarse:
                if (!protegido) Alejarse();
                break;

            case Movimientos.Quieto:
                Quieto();
                break;

            default:
                break;
        }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    void Acercarse() //Se acerca al jugador (target)
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();

        rb.velocity = new Vector3(velocidad * dir.x, rb.velocity.y, rb.velocity.z);
    }

    void Alejarse() //Se aleja del jugador (target)
    {
        Vector3 dir = transform.position - target.transform.position;
        dir.Normalize();

        rb.velocity = new Vector3(velocidad * dir.x, rb.velocity.y, rb.velocity.z);
    }

    void Quieto() //Ponemos nula la velocidad en X para que se detenga
    {
        rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
    }

    void Agacharse() //Cambiamos a la hitbox de personaje agachado
    {
        hitboxPersonaje[0].enabled = false;
        hitboxPersonaje[1].enabled = true;
    }

    void Levantarse() //Cambiamos a la hitbox de personaje de pie
    {
        hitboxPersonaje[0].enabled = true;
        hitboxPersonaje[1].enabled = false;
    }

    void Saltar()
    {
        Nada();
        Levantarse();
        rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
    }

    //0: Arriba, 1: Centro, 2: Abajo
    void Atacar(int ataque)
    {
        Nada();

        switch(ataque)
        {
            case 0:
                AtaqueArriba();
                break;
            case 1:
                AtaqueCentro();
                break;
            case 2:
                AtaqueAbajo();
                break;
            default:
                break;
        }
    }

    void AtaqueArriba()
    {
        hitboxAtaque[0].SetActive(true); //Activar la hitbox del ataque
        atacando = true; //Activar timer de duracion del ataque
    }
    void AtaqueCentro()
    {
        hitboxAtaque[1].SetActive(true); //Activar la hitbox del ataque
        atacando = true; //Activar timer de duracion del ataque
    }
    void AtaqueAbajo()
    {
        hitboxAtaque[2].SetActive(true); //Activar la hitbox del ataque
        atacando = true; //Activar timer de duracion del ataque
    }

    void Protegerse() //Activa el escudo y se protege
    {
        Quieto();

        protegido = true;
        escudo.SetActive(true);
    }

    void Nada() //No esta protegido y ?? TODO
    {
        protegido = false;
        escudo.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            enElSuelo = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Plane")
        {
            enElSuelo = false;
        }
    }
}
