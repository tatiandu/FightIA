using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueAcertado : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Personajes"))
        {
            EnemyAI AIcontroller = other.gameObject.GetComponent<EnemyAI>();

            if (AIcontroller != null) //Es la IA enemiga
            {
                if (AIcontroller.GetEstado() != EnemyAI.Estado.Protegido)   //Si no está protegido es daño maximo
                {
                    GameManager.Instance.decrementaVidaEnemigo(0.15f);
                }
                else { GameManager.Instance.decrementaVidaEnemigo(0.05f); } //Si esta protegido el daño se reduce a un tercio
            }

            PlayerController controller = other.gameObject.GetComponent<PlayerController>();

            if (controller != null) //Es el jugador de input
            {
                if(controller.GetEstado() != PlayerController.Estado.Protegido) {   //Si no está protegido es daño maximo
                    GameManager.Instance.decrementaVidaJugador(0.15f);
                }
                else{ GameManager.Instance.decrementaVidaJugador(0.05f); }  //Si esta protegido el daño se reduce a un tercio
            }
        }
    }
}
