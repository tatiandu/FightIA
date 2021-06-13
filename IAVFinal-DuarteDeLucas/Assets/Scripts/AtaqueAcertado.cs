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
                AIcontroller.GestionaDanio();
            }

            PlayerController controller = other.gameObject.GetComponent<PlayerController>();

            if (controller != null) //Es el jugador de input
            {
                controller.GestionaDanio();
            }
        }
    }
}
