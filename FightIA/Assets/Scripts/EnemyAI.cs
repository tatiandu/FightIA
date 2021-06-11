using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float tiempoAtaque;
    public GameObject escudo;
    public GameObject[] hitboxAtaque; //0: Arriba, 1: Centro, 2: Abajo
    public Collider[] hitboxPersonaje; //0: De pie, 1: Agachado

    Rigidbody rb;
    bool enElSuelo; //Si esta en contacto con el suelo
    bool atacando; //Si esta atacando
    bool agachado; //Si esta agachado
    bool protegido; //Si se esta protegiendo
    float temporizadorAtaque; //Para el ataque

    public float distanciaMin; //Cuanto se puede acercar al jugador
    public float distanciaMax; //Cuanto se puede alejar del jugador
    public float distanciaAccion;
    public float tiempoNextMov; //Tiempo que tarda en pensar el siguiente movimiento
    public float tiempoNextAc; //Tiempo que tarda en pensar la siguiente accion
    float temporizadorMovimiento;
    float temporizadorAccion;
    Movimientos movimientoActual;
    Acciones accionActual;

    enum Movimientos { Acercarse, Alejarse, Quieto }
    enum Acciones { Agacharse, Levantarse, Saltar, AtacarArriba, AtacarCentro, AtacarAbajo, Protegerse, Nada }

    public Estado estadoActual;
    public enum Estado { Saltando, Agachado, Protegido, Atacando, Nada }

    AudioSource emisor;
    //Sonidos de salto, ataque, bloqueo, etc
    public AudioClip[] sonidos;  //0: Salto 1: Bloqueo 2: Daño recibido 3: Protegerse 4: Agacharse

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        emisor = GetComponent<AudioSource>();
        atacando = false;
        agachado = false;
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
        if (GameManager.Instance.sigueEnJuego())
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

            ActualizaEstado();

            //Si esta ejecutando un ataque
            if (atacando)
            {
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
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.sigueEnJuego())
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
    }

    void ActualizaEstado()
    {
        if (protegido) //Protegido
            estadoActual = Estado.Protegido;
        else if (enElSuelo) //Atacando, Agachado, Nada
        {
            if (atacando) estadoActual = Estado.Atacando;
            else if (agachado) estadoActual = Estado.Agachado;
            else estadoActual = Estado.Nada;
        }
        else
        { //Saltando, Atacando
            if (atacando) estadoActual = Estado.Atacando;
            else estadoActual = Estado.Saltando;
        }
    }

    //Gestionar el daño recibido de un ataque
    public void gestionaDaño()
    {
        if (estadoActual != Estado.Protegido)   //Si no está protegido es daño maximo
        {
            emisor.PlayOneShot(sonidos[2]);
            GameManager.Instance.decrementaVidaEnemigo(0.15f);
        }
        else    //Si esta protegido el daño se reduce a un tercio
        {
            emisor.PlayOneShot(sonidos[1]);
            GameManager.Instance.decrementaVidaEnemigo(0.05f);
        }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    void SiguienteAccion()
    {
        int probabilidad = Random.Range(0, 100);

        float vidaJugador = GameManager.Instance.GetVidaJugador();
        float vidaIA = GameManager.Instance.GetVidaEnemigo();
        float distancia = Vector3.Distance(GameManager.Instance.GetPosJugador(), transform.position);
        PlayerController.Estado estadoJugador = GameManager.Instance.GetEstadoJugador();

        switch (estadoJugador) //Actua segun que este haciendo el jugador
        {
            case PlayerController.Estado.Nada:
                if(distancia < distanciaAccion) //Cerca
                {
                    //Probabilidad dependiente de la vida restante de cada jugador
                    if (vidaIA > vidaJugador)
                    {
                        if (probabilidad < 40)
                            accionActual = Acciones.AtacarCentro;
                        else if (probabilidad >= 40 && probabilidad < 60)
                            accionActual = Acciones.AtacarAbajo;
                        else if (probabilidad >= 60 && probabilidad < 80)
                            accionActual = Acciones.Protegerse;
                        else
                            accionActual = Acciones.Nada;
                    }
                    else
                    {
                        if (probabilidad < 50)
                            accionActual = Acciones.Protegerse;
                        else if (probabilidad >= 50 && probabilidad < 80)
                            accionActual = Acciones.AtacarCentro;
                        else if (probabilidad >= 80 && probabilidad < 90)
                            accionActual = Acciones.Agacharse;
                        else
                            accionActual = Acciones.Nada;
                    }
                }
                else //Lejos
                {
                    //Probabilidad random
                    if (probabilidad < 50)
                        accionActual = Acciones.Nada;
                    else if (probabilidad >= 50 && probabilidad < 65)
                        accionActual = Acciones.Saltar;
                    else if (probabilidad >= 65 && probabilidad < 80)
                        accionActual = Acciones.Protegerse;
                    else
                        accionActual = Acciones.Agacharse;
                }
                break;

            case PlayerController.Estado.Atacando:
                if (distancia < distanciaAccion) //Cerca
                {
                    //Pequeña probabilidad de atacar de vuelta y mayor de saltar y protegerse
                    if (probabilidad < 50)
                        accionActual = Acciones.Protegerse;
                    else if (probabilidad >= 50 && probabilidad < 70)
                        accionActual = Acciones.Saltar;
                    else if (probabilidad >= 70 && probabilidad < 80)
                        accionActual = Acciones.Agacharse;
                    else
                        accionActual = Acciones.AtacarCentro;
                }
                else //Lejos
                {
                    //Probabilidad random pero un poco mas de protegerse
                    if (probabilidad < 60)
                        accionActual = Acciones.Protegerse;
                    else if (probabilidad >= 60 && probabilidad < 80)
                        accionActual = Acciones.Nada;
                    else
                        accionActual = Acciones.Saltar;
                }
                break;

            case PlayerController.Estado.Protegido:
                if (distancia < distanciaAccion) //Cerca
                {
                    //Mayor probabilidad de ataque bajo, menor de otros ataques y poco de random
                    if (probabilidad < 50)
                        accionActual = Acciones.AtacarAbajo;
                    else if (probabilidad >= 50 && probabilidad < 60)
                        accionActual = Acciones.AtacarCentro;
                    else if (probabilidad >= 60 && probabilidad < 70)
                        accionActual = Acciones.AtacarArriba;
                    else
                        accionActual = Acciones.Protegerse;
                }
                else //Lejos
                {
                    //Nada y un poquito de error
                    if (probabilidad < 50)
                        accionActual = Acciones.Nada;
                    else if (probabilidad >= 50 && probabilidad < 60)
                        accionActual = Acciones.AtacarCentro;
                    else if (probabilidad >= 60 && probabilidad < 70)
                        accionActual = Acciones.AtacarArriba;
                    else if (probabilidad >= 70 && probabilidad < 80)
                        accionActual = Acciones.Agacharse;
                    else
                        accionActual = Acciones.Saltar;
                }
                break;

            case PlayerController.Estado.Agachado:
                if (distancia < distanciaAccion) //Cerca
                {
                    //Mayor probabilidad de ataque bajo, menor de otros ataques y tambien de salto
                    if (probabilidad < 50)
                        accionActual = Acciones.AtacarAbajo;
                    else if (probabilidad >= 50 && probabilidad < 60)
                        accionActual = Acciones.AtacarCentro;
                    else if (probabilidad >= 60 && probabilidad < 70)
                        accionActual = Acciones.AtacarArriba;
                    else if (probabilidad >= 70 && probabilidad < 80)
                        accionActual = Acciones.Saltar;
                    else
                        accionActual = Acciones.Protegerse;
                }
                else //Lejos
                {
                    //Probabilidad random
                    if (probabilidad < 50)
                        accionActual = Acciones.Nada;
                    else if (probabilidad >= 50 && probabilidad < 65)
                        accionActual = Acciones.Saltar;
                    else if (probabilidad >= 65 && probabilidad < 80)
                        accionActual = Acciones.Protegerse;
                    else
                        accionActual = Acciones.Agacharse;
                }
                break;

            case PlayerController.Estado.Saltando:
                if (distancia < distanciaAccion) //Cerca
                {
                    //Probabilidad dependiente de la vida restante de cada jugador
                    if (vidaIA > vidaJugador)
                    {
                        if (probabilidad < 60)
                            accionActual = Acciones.AtacarArriba;
                        else if (probabilidad >= 60 && probabilidad < 70)
                            accionActual = Acciones.Saltar;
                        else if (probabilidad >= 70 && probabilidad < 90)
                            accionActual = Acciones.Protegerse;
                        else
                            accionActual = Acciones.Nada;
                    }
                    else
                    {
                        if (probabilidad < 70)
                            accionActual = Acciones.Protegerse;
                        else if (probabilidad >= 70 && probabilidad < 90)
                            accionActual = Acciones.AtacarArriba;
                        else
                            accionActual = Acciones.Saltar;
                    }
                }
                else //Lejos
                {
                    //Probabilidad random con mayor proteccion o saltar
                    if (probabilidad < 40)
                        accionActual = Acciones.Protegerse;
                    else if (probabilidad >= 40 && probabilidad < 60)
                        accionActual = Acciones.AtacarArriba;
                    else if (probabilidad >= 60 && probabilidad < 70)
                        accionActual = Acciones.AtacarCentro;
                    else if (probabilidad >= 70 && probabilidad < 80)
                        accionActual = Acciones.Agacharse;
                    else
                        accionActual = Acciones.Nada;
                }
                break;

            default:
                break;
        }
    }

    //Decide cual es la mejor opción de movimiento
    void SiguienteMovimiento()
    {
        int help = Random.Range(0, 100); //Calculo de probabilidades para los casos
        Vector3 miPosicion = transform.position;
        Vector3 posicionAdversario = GameManager.Instance.GetPosJugador(); //Posicion del jugador (target)

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

            case Acciones.AtacarArriba:
                if (!atacando) AtaqueArriba();
                Debug.Log("Ataco arriba");
                break;

            case Acciones.AtacarCentro:
                if (!atacando) AtaqueCentro();
                Debug.Log("Ataco centro");
                break;

            case Acciones.AtacarAbajo:
                if (!atacando) AtaqueAbajo();
                Debug.Log("Ataco abajo");
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
                //El personaje parara su movimiento por si solo
                break;

            default:
                break;
        }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    void Acercarse() //Se acerca al jugador (target)
    {
        Vector3 dir = GameManager.Instance.GetPosJugador() - transform.position;
        dir.Normalize();

        rb.velocity = new Vector3(velocidad * dir.x, rb.velocity.y, rb.velocity.z);
    }

    void Alejarse() //Se aleja del jugador (target)
    {
        Vector3 dir = transform.position - GameManager.Instance.GetPosJugador();
        dir.Normalize();

        rb.velocity = new Vector3(velocidad * dir.x, rb.velocity.y, rb.velocity.z);
    }

    void Agacharse() //Cambiamos a la hitbox de personaje agachado
    {
        GameObject personaje = GameObject.Find("character_knight");
        personaje.transform.localScale = new Vector3(1.0f, 0.65f, 1.0f); //Achatamos el modelo del personaje

        agachado = true;
        emisor.PlayOneShot(sonidos[4]);
        hitboxPersonaje[0].enabled = false;
        hitboxPersonaje[1].enabled = true;
    }

    void Levantarse() //Cambiamos a la hitbox de personaje de pie
    {
        GameObject personaje = GameObject.Find("character_knight");
        personaje.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //Estiramos el modelo del personaje

        agachado = false;
        hitboxPersonaje[0].enabled = true;
        hitboxPersonaje[1].enabled = false;
    }

    void Saltar()
    {
        Nada();
        Levantarse();
        emisor.PlayOneShot(sonidos[0]);
        rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
    }

    void AtaqueArriba()
    {
        Nada();
        emisor.PlayOneShot(sonidos[5]);
        hitboxAtaque[0].SetActive(true); //Activar la hitbox del ataque
        atacando = true; //Activar timer de duracion del ataque
    }
    void AtaqueCentro()
    {
        Nada();
        emisor.PlayOneShot(sonidos[5]);
        hitboxAtaque[1].SetActive(true); //Activar la hitbox del ataque
        atacando = true; //Activar timer de duracion del ataque
    }
    void AtaqueAbajo()
    {
        Nada();
        emisor.PlayOneShot(sonidos[5]);
        hitboxAtaque[2].SetActive(true); //Activar la hitbox del ataque
        atacando = true; //Activar timer de duracion del ataque
    }

    void Protegerse() //Activa el escudo y se protege
    {
        protegido = true;
        emisor.PlayOneShot(sonidos[3]);
        escudo.SetActive(true);
    }

    void Nada() //No esta protegido y ?? TODO
    {
        Levantarse();
        protegido = false;
        escudo.SetActive(false);
    }
    public Estado GetEstado()
    {
        return estadoActual;
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
