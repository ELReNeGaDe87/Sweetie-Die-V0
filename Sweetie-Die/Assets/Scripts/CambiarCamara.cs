using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiarCamara : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;
    private bool movementActive = false;
    private float initialRotationY;
    public float sensitivity = 2.0f; // Sensibilidad del movimiento de la cámara
    public GameObject player; // Referencia al jugador
    public bool justExited = false;

    void Start()
    {
        // Deshabilita la cámara secundaria al inicio
        secondaryCamera.enabled = false;
        // Establece la cámara principal como la cámara activa
        mainCamera.enabled = true;
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (movementActive)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float rotationY = transform.localEulerAngles.y + mouseX;

            // Limita la rotación a un rango de -50 a 50 grados respecto a la rotación inicial
            if (rotationY - initialRotationY > 180) rotationY -= 360; // Convierte el ángulo a -180 a 180
            rotationY = Mathf.Clamp(rotationY, initialRotationY - 42f, initialRotationY + 42f);

            transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        }
        if (justExited == true)
        {
            StartCoroutine(ResetJustExited());
        }
    }

    IEnumerator ResetJustExited()
    {
        yield return new WaitForSeconds(3f);
        justExited = false;
    }

    public void SwitchCamera()
    {
        // Si la cámara principal está activa, cambia a la cámara secundaria
        if (mainCamera.enabled)
        {
            // Encuentra el spawnpoint más cercano al jugador
            GameObject closestSpawnpoint = FindClosestSpawnpoint();
            // Mueve la cámara secundaria al spawnpoint más cercano
            secondaryCamera.transform.position = closestSpawnpoint.transform.position;
            secondaryCamera.transform.rotation = closestSpawnpoint.transform.rotation;

            // Guarda la rotación inicial en el eje Y del spawnpoint
            initialRotationY = secondaryCamera.transform.localEulerAngles.y;

            mainCamera.enabled = false;
            secondaryCamera.enabled = true;
            movementActive = true;
        }
        // Si la cámara secundaria está activa, cambia a la cámara principal
        else
        {
            movementActive = false;
            secondaryCamera.enabled = false;
            mainCamera.enabled = true;
            justExited = true;
        }
    }

    GameObject FindClosestSpawnpoint()
    {
        GameObject[] spawnpoints;
        spawnpoints = GameObject.FindGameObjectsWithTag("SpawnCamera");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = player.transform.position;
        foreach (GameObject spawnpoint in spawnpoints)
        {
            Vector3 diff = spawnpoint.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = spawnpoint;
                distance = curDistance;
            }
        }
        return closest;
    }
    public bool isCameraActive()
    {
        return movementActive;
    }

    public bool JustExited()
    {
        return justExited;
    }
}