﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float tiempoAtaque;
    public GameObject[] hitboxAtaque; //0: Arriba, 1: Centro, 2: Abajo
    public Collider[] hitboxJugador; //0: De pie, 1: Agachado

    Rigidbody rb;
    bool enElSuelo; //Si esta en contacto con el suelo
    bool atacando; //Si esta atacando
    bool protegido; //Si se esta protegiendo
    float temporizador; //Para el ataque

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        atacando = false;
        protegido = false;
        temporizador = 0;
    }

    void Update()
    {
        Salto(); //Input de salto
        Agachar(); //Input de agacharse
        Proteger(); //Input de protegerse
        Ataque(); //Input de ataque

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
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && enElSuelo)
        {
            hitboxJugador[0].enabled = false;
            hitboxJugador[1].enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            hitboxJugador[0].enabled = true;
            hitboxJugador[1].enabled = false;
        }

        //Animacion correspondiente TODO
    }

    void Proteger()
    {
        if (Input.GetKeyDown(KeyCode.I) && !atacando && enElSuelo)
        {
            protegido = true;

        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            protegido = false;

        }

        //Animacion correspondiente TODO
    }

    void Ataque()
    {
        if (Input.GetKeyDown(KeyCode.J) && !atacando) //Arriba
        {
            hitboxAtaque[0].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque

            //Animacion correspondiente TODO
        }
        else if (Input.GetKeyDown(KeyCode.K) && !atacando) //Centro
        {
            hitboxAtaque[1].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque

            //Animacion correspondiente TODO
        }
        else if (Input.GetKeyDown(KeyCode.L) && !atacando) //Abajo
        {
            hitboxAtaque[2].SetActive(true); //Activar la hitbox del ataque
            atacando = true; //Activar timer de duracion del ataque

            //Animacion correspondiente TODO
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
