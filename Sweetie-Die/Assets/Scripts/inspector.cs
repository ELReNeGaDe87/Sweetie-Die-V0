using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    private UIManager uIManager;

    private bool lookedAtBook;
   
    void Start()
    {
        playerController = player.GetComponent<PlayerController>(); // Obtener la referencia al script PlayerController
        uIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
           // Verifica si el juego está pausado antes de procesar la entrada de teclado
        if (!PauseMenu.GameIsPaused)
        {
            if (enModoInspeccion)
            {
                uIManager.HideEAndAimDot();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UnInspectBook();
                }
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, 2f) && hit.transform.gameObject.CompareTag("PlayerBook"))
            {
                uIManager.ShowE(true);
                lookedAtBook = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (!enModoInspeccion)
                    {
                        InspectBook();
                    }
                    else
                    {
                        UnInspectBook();
                    }
                }
                return;
            }
                // If the hit object is not a "PlayerBook," set the text to inactive.
            if (lookedAtBook)
            {
                uIManager.ShowE(false);
                lookedAtBook = false;
            }
        }
    }

    private void InspectBook()
    {
        UnityEngine.Debug.Log("Inspect Book");

        uIManager.HideAllTexts();
        uIManager.HideAimDot();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Texto.SetActive(true);
        ObjetoInspect.SetActive(true);
        InspectCamera.SetActive(true);
        MainCamera.SetActive(false);

        controlesActivos = false;
        playerController.enabled = false;
        enModoInspeccion = true;
    }

    private void UnInspectBook()
    {
        UnityEngine.Debug.Log("UnInspect Book");

        uIManager.ShowAimDot();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Texto.SetActive(false);
        ObjetoInspect.SetActive(false);
        InspectCamera.SetActive(false);
        MainCamera.SetActive(true);
        controlesActivos = true;
        playerController.enabled = true;
        enModoInspeccion = false;

        ChangeDoorTags();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        activa = true;
    //        showHelperText(true);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //        Cursor.visible = false;
    //        activa = false;
    //        Texto.SetActive(false);
    //        ObjetoInspect.SetActive(false);
    //        InspectCamera.SetActive(false);
    //        objEnEscena.SetActive(true);
    //        controlesActivos = true;
    //        playerController.enabled = true;
    //        showHelperText(false);

    //        // Asegúrate de que al salir del área de activación, la cámara principal se reactive
    //        MainCamera.SetActive(true);
    //        ChangeDoorTags();
    //    }
    //}

    private void ChangeDoorTags()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("OpenDoorPlayerRoom");

        foreach (GameObject door in doors)
        {
            door.tag = "OpenDoorL";
        }
    }
}