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
    [SerializeField]
    private bool controlesActivos = true;
    private bool enModoInspeccion = false;
    private PlayerController playerController; // Referencia al script PlayerController
    [SerializeField]
    private GameObject helperText;
    [SerializeField]
    private GameObject aimDot;
   
    void Start()
    {
        playerController = player.GetComponent<PlayerController>(); // Obtener la referencia al script PlayerController
        
    }

    void Update()
    {
           // Verifica si el juego está pausado antes de procesar la entrada de teclado
        if (!PauseMenu.GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.E) && activa == true)
            {               
                if (!enModoInspeccion)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Texto.SetActive(true);
                    ObjetoInspect.SetActive(true);
                    InspectCamera.SetActive(true);
                    MainCamera.SetActive(false);

                    controlesActivos = false;
                    playerController.enabled = false;
                    enModoInspeccion = true;

                    helperText.SetActive(false);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Texto.SetActive(false);
                    ObjetoInspect.SetActive(false);
                    InspectCamera.SetActive(false);
                    MainCamera.SetActive(true);
                    controlesActivos = true;
                    playerController.enabled = true;
                    enModoInspeccion = false;
                    helperText.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activa = true;
            showHelperText(true);
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
            showHelperText(false);

            // Asegúrate de que al salir del área de activación, la cámara principal se reactive
            MainCamera.SetActive(true);
            ChangeDoorTags();
        }
    }

    private void showHelperText(bool value)
    {
        helperText.SetActive(value);
        aimDot.SetActive(!value);
    }
    private void ChangeDoorTags()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("OpenDoorPlayerRoom");

        foreach (GameObject door in doors)
        {
            door.tag = "OpenDoorL";
        }
    }
}