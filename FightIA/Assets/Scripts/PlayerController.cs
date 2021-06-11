﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
    float temporizador; //Para el ataque

    public Estado estadoActual;
    public enum Estado { Saltando, Agachado, Protegido, Atacando, Nada }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        atacando = false;
        agachado = false;
        protegido = false;
        temporizador = 0;

        estadoActual = Estado.Nada; //Estado inicial
    }

    void Update()
    {
        Salto(); //Input de salto
        Agachar(); //Input de agacharse
        Proteger(); //Input de protegerse
        Ataque(); //Input de ataque

        ActualizaEstado(); //Actualizamos el estado actual del jugador

        if (atacando) //Si esta ejecutando un ataque
        {
            temporizador += Time.deltaTime;

            if (temporizador >= tiempoAtaque) { //Si ha pasado el tiempo de ataque
                atacando = false;
                temporizador = 0;

                foreach (GameObject aux in hitboxAtaque) { //Desactivar hitbox activas
                    aux.SetActive(false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //Movimiento en horizontal del personaje si no se esta protegiendo
        if (!protegido)
        {
            rb.velocity = new Vector3(velocidad * Input.GetAxis("Horizontal"), rb.velocity.y, rb.velocity.z);

            //Animacion correspondiente TODO
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
        else { //Saltando, Atacando
            if (atacando) estadoActual = Estado.Atacando;
            else estadoActual = Estado.Saltando;
        }
    }

    void Salto()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && enElSuelo && !protegido)
        {
            rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
        }

        //Animacion correspondiente TODO
    }

    void Agachar()
    {
        GameObject personaje = GameObject.Find("character_rogue");

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && enElSuelo) //Se agacha
        {
            agachado = true;
            hitboxPersonaje[0].enabled = false;
            hitboxPersonaje[1].enabled = true;

            personaje.transform.localScale = new Vector3(1.0f, 0.65f, 1.0f); //Achatamos el modelo del personaje
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) //Se pone de pie
        {
            agachado = false;
            hitboxPersonaje[0].enabled = true;
            hitboxPersonaje[1].enabled = false;

            personaje.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); //Estiramos el modelo del personaje
        }

        //Animacion correspondiente TODO
    }

    void Proteger()
    {
        if (Input.GetKeyDown(KeyCode.I) && !atacando && enElSuelo)
        {
            protegido = true;
            escudo.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            protegido = false;
            escudo.SetActive(false);
        }

        //Animacion correspondiente TODO
    }

    void Ataque()
    {
        if (Input.GetKeyDown(KeyCode.J) && !atacando && !protegido) //Arriba
        {
            hitboxAtaque[0].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque

            //Animacion correspondiente TODO
        }
        else if (Input.GetKeyDown(KeyCode.K) && !atacando && !protegido) //Centro
        {
            hitboxAtaque[1].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque

            //Animacion correspondiente TODO
        }
        else if (Input.GetKeyDown(KeyCode.L) && !atacando && !protegido) //Abajo
        {
            hitboxAtaque[2].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque

            //Animacion correspondiente TODO
        }
    }

    public Estado GetEstado()
    {
        return estadoActual;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Plane")
        {
            estadoActual = Estado.Nada; //Actualizamos estado
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
