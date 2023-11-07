using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splash : MonoBehaviour
{
    public float tiempoDeEspera = 3.0f;  

    private bool sePresionoUnaTecla = false;

    void Update()
    {
        if (!sePresionoUnaTecla)
        {
            
            if (Time.timeSinceLevelLoad >= tiempoDeEspera)
            {
                CambiarEscena();
            }

            
            if (Input.anyKeyDown)
            {
                sePresionoUnaTecla = true;
                CambiarEscena();
            }
        }
    }

    void CambiarEscena()
    {
        // Cambia a la escena principal
        SceneManager.LoadScene("MainMenu");
    }
}