using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;

    Rigidbody rb;
    bool enElSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Salto
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            rb.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        //Movimiento en horizontal del personaje
        rb.velocity = new Vector3(velocidad * Input.GetAxis("Horizontal"), rb.velocity.y, rb.velocity.z);
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
