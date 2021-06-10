using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float tiempoRestante = 120;

    Text textoTimer;
    bool activado;

    private void Start()
    {
        textoTimer = GetComponent<Text>();
        activado = true; //Comenzar el contador automaticamente
    }

    void Update()
    {
        if (activado)
        {
            if (tiempoRestante > 0) //Mientras quede tiempo restamos
            {
                tiempoRestante -= Time.deltaTime;
                MostrarTiempo(tiempoRestante);
            }
            else //Cuando llegue a 0 paramos el contador
            {
                Debug.Log("Se acabo el tiempo ups!");
                tiempoRestante = 0;
                activado = false;
            }
        }
    }

    void MostrarTiempo(float tiempoAMostrar) //Actualiza el texto del tiempo restante
    {
        tiempoAMostrar += 1;

        float minutos = Mathf.FloorToInt(tiempoAMostrar / 60);
        float segundos = Mathf.FloorToInt(tiempoAMostrar % 60);

        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public float GetTiempo()
    {
        return tiempoRestante;
    }
}