using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inspector : MonoBehaviour
{
    public GameObject Texto;
    public GameObject ObjetoInspect;
    public GameObject InspectCamera;
    public GameObject MainCamera;
    public GameObject objEnEscena;
    public GameObject player;
    public bool activa;
   private bool controlesActivos = true;
    private bool enModoInspeccion = false;
    private PlayerController playerController; // Referencia al script PlayerController

    void Start()
    {
        playerController = player.GetComponent<PlayerController>(); // Obtener la referencia al script PlayerController
    }

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && activa == true)
        {
            if (!enModoInspeccion)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Entrar en modo inspección
                Texto.SetActive(true);
                ObjetoInspect.SetActive(true);
                InspectCamera.SetActive(true);
                MainCamera.SetActive(false);

                controlesActivos = false;
                playerController.enabled = false;
                enModoInspeccion = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                // Salir del modo inspección
                Texto.SetActive(false);
                ObjetoInspect.SetActive(false);
                InspectCamera.SetActive(false);
                MainCamera.SetActive(true);

              controlesActivos = true;
                playerController.enabled = true;
                enModoInspeccion = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activa = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            activa = false;
            Texto.SetActive(false);
            ObjetoInspect.SetActive(false);
            InspectCamera.SetActive(false);
            objEnEscena.SetActive(true);
            controlesActivos = true;
            playerController.enabled = true;

            // Asegúrate de que al salir del área de activación, la cámara principal se reactive
            MainCamera.SetActive(true);
        }
    }
}
