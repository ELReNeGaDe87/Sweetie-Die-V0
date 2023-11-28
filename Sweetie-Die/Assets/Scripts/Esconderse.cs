using System.Collections;
using UnityEngine;

public class Esconderse : MonoBehaviour
{
    private bool canHide = true;
    private Vector3 originalPosition;
    private bool isHiding = false;
    private CambiarCamara switchCamera;
    private PlayerController playerController;
    private Transform currentDoor;
    private float currentRotation;

    void Start()
    {
        // Encuentra los componentes en el inicio
        switchCamera = FindObjectOfType<CambiarCamara>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (canHide && Input.GetKeyDown(KeyCode.E))
        {
            // Lanza un raycast para detectar la puerta
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f) && hit.transform.CompareTag("Door"))
            {
                StartCoroutine(Hide(hit.transform));
            }
        }
        else if ((!canHide && isHiding) && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Unhide());
        }
    }

    IEnumerator Hide(Transform door)
    {
        canHide = false;

        // Rotar solo la puerta

        // Línea que asigna el valor a currentRotation
        currentRotation = door.localEulerAngles.y >= 0 && door.localEulerAngles.y < 180 ? -90f : 90f;

        yield return Rotate(door, currentRotation);

        yield return new WaitForSeconds(1);

        // Rotar la puerta de nuevo a su posición original
        yield return Rotate(door, -currentRotation);

        playerController.ToggleControls();
        switchCamera.SwitchCamera();
        isHiding = true;

        // Guarda la puerta actual para usarla en Unhide
        currentDoor = door;
    }

    IEnumerator Unhide()
    {
        switchCamera.SwitchCamera();
        playerController.ToggleControls();

        // Usa la puerta y la rotación guardadas
        yield return Rotate(currentDoor, currentRotation);

        yield return new WaitForSeconds(1);

        yield return Rotate(currentDoor, -currentRotation);

        isHiding = false;

        yield return new WaitForSeconds(60f);

        canHide = true;
    }

    IEnumerator Rotate(Transform target, float rotation)
    {
        float timer = 0;
        while (timer < 1f)
        {
            target.Rotate(0, rotation * Time.deltaTime, 0);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}