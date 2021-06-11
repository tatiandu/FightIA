using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    Slider barra;
    float valorFinal = 1;

    public float velocidad = 0.5f;
    public ParticleSystem particulas;
    public Image imagenBarra;

    private void Awake()
    {
        barra = gameObject.GetComponent<Slider>();
    }

    void Start()
    {

    }

    void Update()
    {
        if (barra.value > valorFinal)
        {
            barra.value -= velocidad * Time.deltaTime;
            if (!particulas.isPlaying)
            {
                particulas.Play();
            }
        }
        else
        {
            particulas.Stop();
        }

        if(barra.value <= 0)
        {
            imagenBarra.enabled = false;
            particulas.Stop();
        }
    }

    public void PerderVida(float valor)
    {
        valorFinal = barra.value - valor;
    }

    public float GetSalud()
    {
        return barra.value;
    }

    public bool estaAgotada()
    {
        return barra.value <= 0;
    }
}
