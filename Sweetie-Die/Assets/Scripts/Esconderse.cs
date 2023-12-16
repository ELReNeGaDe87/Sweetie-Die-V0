using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esconderse : MonoBehaviour
{
    private bool canHide = true;
    private bool isHiding = false;
    private CambiarCamara switchCamera;
    private PlayerController playerController;
    private Transform currentDoor;
    private List<BoxCollider> roomTriggers;
    private BoxCollider currentRoomTrigger;
    private float originalRotation;
    private float currentRotation;
    private float hideTime;

    void Start()
    {
        switchCamera = FindObjectOfType<CambiarCamara>();
        playerController = FindObjectOfType<PlayerController>();
        GameObject[] roomTriggerObjects = GameObject.FindGameObjectsWithTag("RoomTrigger");
        roomTriggers = new List<BoxCollider>();
        foreach (GameObject roomTriggerObject in roomTriggerObjects)
        {
            roomTriggers.Add(roomTriggerObject.GetComponent<BoxCollider>());
        }
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }
        if (canHide && Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 1f) && hit.transform.tag.Contains("Hiding"))
            {
                StartCoroutine(Hide(hit.transform));
            }
        }
        else if (((!canHide && isHiding) && Input.GetKeyDown(KeyCode.E)) || ((!canHide && isHiding) && Time.time - hideTime >= 30f))
        {
            StartCoroutine(Unhide());
        }
    }

    IEnumerator Hide(Transform door)
    {
        canHide = false;
        hideTime = Time.time;
        originalRotation = door.localEulerAngles.y;
        if (door.CompareTag("HidingL"))
        {
            currentRotation = originalRotation - 90f;
        }
        else if (door.CompareTag("HidingR"))
        {
            currentRotation = originalRotation + 90f;
        }
        yield return Rotate(door, currentRotation);
        yield return new WaitForSeconds(1);
        yield return Rotate(door, originalRotation);
        foreach (BoxCollider roomTrigger in roomTriggers)
        {
            if (roomTrigger.bounds.Contains(transform.position))
            {
                currentRoomTrigger = roomTrigger;
                playerController.ToggleControls();
                switchCamera.SwitchCamera();
                isHiding = true;
                currentDoor = door;
            }
        }
        if (isHiding == false)
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
        yield return Rotate(currentDoor, currentRotation);
        while (currentRoomTrigger.bounds.Contains(transform.position))
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return Rotate(currentDoor, originalRotation);
        yield return new WaitForSeconds(60f);
        canHide = true;
    }

    IEnumerator Rotate(Transform target, float targetRotation)
    {
        float currentRotation = target.localEulerAngles.y;
        float deltaRotation = Mathf.DeltaAngle(currentRotation, targetRotation);
        float rotationSpeed = deltaRotation / 1f;
        float timer = 0;
        while (timer < 1f)
        {
            float rotation = currentRotation + rotationSpeed * timer;
            target.localEulerAngles = new Vector3(target.localEulerAngles.x, rotation, target.localEulerAngles.z);
            timer += Time.deltaTime;
            yield return null;
        }
        target.localEulerAngles = new Vector3(target.localEulerAngles.x, targetRotation, target.localEulerAngles.z);
    }
}