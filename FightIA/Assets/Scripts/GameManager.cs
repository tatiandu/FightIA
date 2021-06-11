﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject jugador;
    public GameObject enemigo;
    public Timer tiempo;
    public BarraVida vidaJugador;
    public BarraVida vidaEnemigo;

    bool personajesGirados;
    float tiempoPartida;
    PlayerController comportamientoJugador;
    EnemyAI comportamientoEnemigo;

    //Para saber si la partida está en juego
    bool enJuego;

    //INSTANCIA
    public static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        enJuego = true;
        personajesGirados = false;
        comportamientoJugador = jugador.GetComponent<PlayerController>();
        comportamientoEnemigo = enemigo.GetComponent<EnemyAI>();
    }

    void Update()
    {
        if (enJuego)
        {
            CompruebaVictoria();    //Comprobamos si alguno de los contrincantes o ambos se han quedado sin vida

            //Cambia la orientacion de los personajes si se cruzan
            if (!personajesGirados && jugador.transform.position.x > enemigo.transform.position.x)
            {
                Vector3 y = new Vector3(0, 1, 0);
                jugador.transform.Rotate(y, 180);
                enemigo.transform.Rotate(y, -180);
                personajesGirados = true;
            }
            else if (personajesGirados && jugador.transform.position.x < enemigo.transform.position.x)
            {
                Vector3 y = new Vector3(0, 1, 0);
                jugador.transform.Rotate(y, -180);
                enemigo.transform.Rotate(y, 180);
                personajesGirados = false;
            }
        }
    }

    void CompruebaVictoria()
    {
        if(vidaJugador.estaAgotada() && !vidaEnemigo.estaAgotada() || vidaJugador.GetSalud() < vidaEnemigo.GetSalud() && tiempo.tiempoAgotado()) //El jugador de imput ha perdido
        {
            Debug.Log("Ha ganado la IA");
            enJuego = false;
            tiempo.paraTiempo();
        }
        else if (vidaEnemigo.estaAgotada() && !vidaJugador.estaAgotada() || vidaJugador.GetSalud() > vidaEnemigo.GetSalud() && tiempo.tiempoAgotado()) //El jugador de imput ha ganado
        {
            Debug.Log("Ha ganado el jugador humano");
            enJuego = false;
            tiempo.paraTiempo();
        }
        else if (vidaJugador.estaAgotada() && vidaEnemigo.estaAgotada() || vidaJugador.GetSalud() == vidaEnemigo.GetSalud() && tiempo.tiempoAgotado())    //Empate entre los jugadores
        {
            Debug.Log("El hombre y la máquina han empatado");
            enJuego = false;
            tiempo.paraTiempo();
        }
    }

    public bool sigueEnJuego()
    {
        return enJuego;
    }

    //Getter de la posicion del jugador
    public Vector3 GetPosJugador()
    {
        return jugador.transform.position;
    }

    //Getter del estado actual del jugador
    public PlayerController.Estado GetEstadoJugador() 
    {
        return comportamientoJugador.GetEstado();
    }

    //Getter de la vida restante del jugador
    public float GetVidaJugador()
    {
        return vidaJugador.GetSalud();
    }

    //Getter de la vida restante del enemigo
    public float GetVidaEnemigo()
    {
        return vidaEnemigo.GetSalud();
    }
    //Para decrementar la vida del jugador por Input
    public void decrementaVidaJugador(float daño)
    {
        vidaJugador.PerderVida(daño);
    }
    //Para decrementar la vida del enemigo por IA
    public void decrementaVidaEnemigo(float daño)
    {
        vidaEnemigo.PerderVida(daño);
    }
}
