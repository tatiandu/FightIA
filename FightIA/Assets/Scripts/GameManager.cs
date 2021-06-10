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

    bool personajesGirados;
    float tiempoPartida;
    PlayerController comportamientoJugador;
    EnemyAI comportamientoEnemigo;

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
        personajesGirados = false;
        comportamientoJugador = jugador.GetComponent<PlayerController>();
        comportamientoEnemigo = enemigo.GetComponent<EnemyAI>();
    }

    void Update()
    {
        //Cambia la orientacion de los personajes si se cruzan
        if(!personajesGirados && jugador.transform.position.x > enemigo.transform.position.x)
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

    public Vector3 GetPosJugador()
    {
        return jugador.transform.position;
    }

    //Getter del estado actual del jugador
    public PlayerController.Estado GetEstadoJugador() 
    {
        return comportamientoJugador.GetEstado();
    }
}
