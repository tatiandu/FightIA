using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Funciones publicas que son llamadas al interactuar con elementos de la interfaz de usuario
public class Boton : MonoBehaviour
{
    // Metodo para recargar la escena que este abierta en este momento
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Metodo para cerrar el juego
    public static void Exit()
    {
        Application.Quit();
    }
}