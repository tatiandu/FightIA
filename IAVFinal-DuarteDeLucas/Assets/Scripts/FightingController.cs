using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float knockback;
    public float tiempoAtaque;
    public GameObject escudo;
    public GameObject[] hitboxAtaque; //0: Arriba, 1: Centro, 2: Abajo
    public Collider[] hitboxPersonaje; //0: De pie, 1: Agachado

    protected Rigidbody rb;
    protected bool enElSuelo; //Si esta en contacto con el suelo
    protected bool atacando; //Si esta atacando
    protected bool agachado; //Si esta agachado
    protected bool protegido; //Si se esta protegiendo

    public Estado estadoActual;
    public enum Estado { Saltando, Agachado, Protegido, Atacando, Nada }

    protected AudioSource emisorAudio;
    //Sonidos de salto, ataque, bloqueo, etc.
    public AudioClip[] sonidos;  //0: Salto 1: Bloqueo 2: Daño recibido 3: Protegerse 4: Agacharse

    protected void ActualizaEstado()
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
    virtual public void GestionaDanio() { }
    virtual protected void Salto() { }
    virtual protected void Agachar() { }
    virtual protected void Proteger() { }
    virtual protected void Ataque() { }

    public Estado GetEstado()
    {
        return estadoActual;
    }
}