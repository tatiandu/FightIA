using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject jugador;
    public GameObject enemigo;
    public Timer tiempo;
    public BarraVida vidaJugador;
    public BarraVida vidaEnemigo;

    float tiempoPartida;
    PlayerController comportamientoJugador;
    EnemyAI comportamientoEnemigo;

    void Start()
    {
        comportamientoJugador = jugador.GetComponent<PlayerController>();
        comportamientoEnemigo = enemigo.GetComponent<EnemyAI>();
    }

    void Update()
    {
        
    }

    //Getter del estado actual del jugador
    public PlayerController.Estado GetEstadoJugador() 
    {
        return comportamientoJugador.GetEstado();
    }
}
