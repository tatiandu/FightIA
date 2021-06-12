using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float tiempoRestante;
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
            if (!tiempoAgotado()) //Mientras quede tiempo restamos
            {
                tiempoRestante -= Time.deltaTime;
                MostrarTiempo(tiempoRestante);
            }
            else //Cuando llegue a 0 paramos el contador
            {
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

    public void SetTiempo(float t)
    {
        tiempoRestante = t;
    }

    public float GetTiempo()
    {
        return tiempoRestante;
    }

    public bool tiempoAgotado()
    {
        return tiempoRestante <= 0;
    }

    public void paraTiempo()
    {
        activado=false;
    }
}