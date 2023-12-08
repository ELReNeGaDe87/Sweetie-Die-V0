using System.Collections;
using UnityEngine;

public class Esconderse : MonoBehaviour
{
    private bool canHide = true;
    private bool isHiding = false;
    private CambiarCamara switchCamera;
    private PlayerController playerController;
    private Transform currentDoor;
    private float currentRotation;
    private BoxCollider roomTrigger;

    void Start()
    {
        // Encuentra los componentes en el inicio
        switchCamera = FindObjectOfType<CambiarCamara>();
        playerController = FindObjectOfType<PlayerController>();
        roomTrigger = GameObject.FindGameObjectWithTag("RoomTrigger").GetComponent<BoxCollider>();
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
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f) && hit.transform.CompareTag("HidingDoor"))
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

        currentRotation = door.localEulerAngles.y >= 0 && door.localEulerAngles.y < 180 ? -90f : 90f;

        yield return Rotate(door, currentRotation);

        yield return new WaitForSeconds(1);

        yield return Rotate(door, -currentRotation);

        if (roomTrigger.bounds.Contains(transform.position))
        {
            playerController.ToggleControls();
            switchCamera.SwitchCamera();
            isHiding = true;
            currentDoor = door;
        }
        else
        {
            yield return new WaitForSeconds(20f);
            canHide = true;
        }
    }

    IEnumerator Unhide()
    {
        isHiding = false;
        switchCamera.SwitchCamera();
        playerController.ToggleControls();

        // Usa la puerta y la rotación guardadas
        yield return Rotate(currentDoor, currentRotation);

        while (roomTrigger.bounds.Contains(transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return Rotate(currentDoor, -currentRotation);

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