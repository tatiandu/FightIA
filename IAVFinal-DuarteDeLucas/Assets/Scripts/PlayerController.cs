using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : FightingController
{
    float temporizador; //Para el ataque

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        emisorAudio = GetComponent<AudioSource>();
        atacando = false;
        agachado = false;
        protegido = false;
        temporizador = 0;

        estadoActual = Estado.Nada; //Estado inicial
    }

    void Update()
    {
        if (GameManager.Instance.sigueEnJuego())
        {
            Salto(); //Input de salto
            Agachar(); //Input de agacharse
            Proteger(); //Input de protegerse
            Ataque(); //Input de ataque

            ActualizaEstado(); //Actualizamos el estado actual del jugador

            if (atacando) //Si esta ejecutando un ataque
            {
                temporizador += Time.deltaTime;

                if (temporizador >= tiempoAtaque)
                { //Si ha pasado el tiempo de ataque
                    atacando = false;
                    temporizador = 0;

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
            //Movimiento en horizontal del personaje si no se esta protegiendo
            if (!protegido)
            {
                rb.velocity = new Vector3(velocidad * Input.GetAxis("Horizontal"), rb.velocity.y, rb.velocity.z);
            }
        }
    }

    //Gestionar el daño recibido de un ataque
    public override void GestionaDanio()
    {
        if (estadoActual != Estado.Protegido)   //Si no está protegido es danio maximo
        {
            emisorAudio.PlayOneShot(sonidos[2]);
            GameManager.Instance.decrementaVidaJugador(0.1f);

            //Efecto de repulsion
            Vector3 dir = transform.position - GameManager.Instance.GetPosEnemigo();
            dir.Normalize();
            rb.AddForce(dir * knockback, ForceMode.Impulse);
        }
        else    //Si esta protegido el danio se reduce a un tercio
        {
            emisorAudio.PlayOneShot(sonidos[1]);
            GameManager.Instance.decrementaVidaJugador(0.04f); 
        } 
    }

    protected override void Salto()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && enElSuelo && !protegido)
        {
            emisorAudio.PlayOneShot(sonidos[0]);
            rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
        }
    }

    protected override void Agachar()
    {
        GameObject personaje = GameObject.Find("character_rogue");

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && enElSuelo) //Se agacha
        {
            agachado = true;
            hitboxPersonaje[0].enabled = false;
            hitboxPersonaje[1].enabled = true;

            personaje.transform.localScale = new Vector3(1.0f, 0.65f, 1.0f); //Achatamos el modelo del personaje
            emisorAudio.PlayOneShot(sonidos[4]);
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) //Se pone de pie
        {
            agachado = false;
            hitboxPersonaje[0].enabled = true;
            hitboxPersonaje[1].enabled = false;

            personaje.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //Estiramos el modelo del personaje
        }
    }

    protected override void Proteger()
    {
        if (Input.GetKeyDown(KeyCode.I) && !atacando && enElSuelo)
        {
            protegido = true;
            escudo.SetActive(true);

            emisorAudio.PlayOneShot(sonidos[3]);
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            protegido = false;
            escudo.SetActive(false);
        }
    }

    protected override void Ataque()
    {
        if (Input.GetKeyDown(KeyCode.J) && !atacando && !protegido) //Arriba
        {
            hitboxAtaque[0].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque
            emisorAudio.PlayOneShot(sonidos[5]);
        }
        else if (Input.GetKeyDown(KeyCode.K) && !atacando && !protegido) //Centro
        {
            hitboxAtaque[1].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque
            emisorAudio.PlayOneShot(sonidos[5]);
        }
        else if (Input.GetKeyDown(KeyCode.L) && !atacando && !protegido) //Abajo
        {
            hitboxAtaque[2].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque
            emisorAudio.PlayOneShot(sonidos[5]);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Plane")
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
