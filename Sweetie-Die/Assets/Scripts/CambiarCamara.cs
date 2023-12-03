using UnityEngine;

public class CambiarCamara : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondaryCamera;
    private bool movementActive = false;
    private float initialRotationY;
    public float sensitivity = 2.0f; // Sensibilidad del movimiento de la c�mara
    public GameObject player; // Referencia al jugador

    void Start()
    {
        // Deshabilita la c�mara secundaria al inicio
        secondaryCamera.enabled = false;
        // Establece la c�mara principal como la c�mara activa
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

            // Limita la rotaci�n a un rango de -50 a 50 grados respecto a la rotaci�n inicial
            if (rotationY - initialRotationY > 180) rotationY -= 360; // Convierte el �ngulo a -180 a 180
            rotationY = Mathf.Clamp(rotationY, initialRotationY - 50f, initialRotationY + 50f);

            transform.localRotation = Quaternion.Euler(0, rotationY, 0);
        }
    }

    public void SwitchCamera()
    {
        // Si la c�mara principal est� activa, cambia a la c�mara secundaria
        if (mainCamera.enabled)
        {
            // Encuentra el spawnpoint m�s cercano al jugador
            GameObject closestSpawnpoint = FindClosestSpawnpoint();
            // Mueve la c�mara secundaria al spawnpoint m�s cercano
            secondaryCamera.transform.position = closestSpawnpoint.transform.position;
            secondaryCamera.transform.rotation = closestSpawnpoint.transform.rotation;

            // Guarda la rotaci�n inicial en el eje Y del spawnpoint
            initialRotationY = secondaryCamera.transform.localEulerAngles.y;

            mainCamera.enabled = false;
            secondaryCamera.enabled = true;
            movementActive = true;
        }
        // Si la c�mara secundaria est� activa, cambia a la c�mara principal
        else
        {
            movementActive = false;
            secondaryCamera.enabled = false;
            mainCamera.enabled = true;
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
}