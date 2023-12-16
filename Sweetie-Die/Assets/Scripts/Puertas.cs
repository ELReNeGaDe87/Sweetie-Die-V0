using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puertas : MonoBehaviour
{
    private float currentRotation;
    private RaycastHit hit;
    public AudioClip audioClip;
    public bool puedesAbrir = true;
    public GameObject player;
    private bool canInteract = true;
    private RecogerObjeto recogerObjeto;
    private float originalRotation;

    [SerializeField]
    private GameObject OpenDoorText;

    void Start()
    {
        recogerObjeto = FindObjectOfType<RecogerObjeto>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puedesAbrir && Physics.Raycast(transform.position, transform.forward, out hit, 1f) && hit.transform.tag.Contains("OpenDoor") && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenDoor(hit.transform));
        }
        else if (puedesAbrir && Physics.Raycast(transform.position, transform.forward, out hit, 1f) && hit.transform.tag.Contains("MonsterDoor") && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenDoorM(hit.transform));
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && hit.transform.CompareTag("CloseDoor") && Input.GetKeyDown(KeyCode.E))
        {
            GameObject nearestLockedDoor = FindNearestWithTag(player.transform.position, "CloseDoor");
            if (nearestLockedDoor != null)
            {
                AudioSource audioSource = nearestLockedDoor.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(audioClip);
                }
            }
        }
    }

    IEnumerator OpenDoor(Transform door) 
    {
        canInteract = false;
        originalRotation = door.localEulerAngles.y;
        if (door.CompareTag("OpenDoorL"))
        {
            currentRotation = originalRotation - 90f;
        }
        else if (door.CompareTag("OpenDoorR"))
        {
            currentRotation = originalRotation + 90f;
        }
        yield return Rotate(door, currentRotation);
        yield return new WaitForSeconds(1);
        yield return Rotate(door, originalRotation);
        canInteract = true;
    }

    IEnumerator OpenDoorM(Transform door)
    {
        canInteract = false;
        originalRotation = door.localEulerAngles.y;
        if (door.CompareTag("MonsterDoorL"))
        {
            currentRotation = originalRotation - 90f;
        }
        else if (door.CompareTag("MonsterDoorR"))
        {
            currentRotation = originalRotation + 90f;
        }
        yield return Rotate(door, currentRotation);
        yield return new WaitForSeconds(1);
        yield return Rotate(door, originalRotation);
        canInteract = true;
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

    GameObject FindNearestWithTag(Vector3 position, string tag)
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
        if (taggedObjects.Length == 0) return null;
        GameObject nearestObject = taggedObjects[0];
        float minDistance = Vector3.Distance(position, nearestObject.transform.position);
        foreach (GameObject obj in taggedObjects)
        {
            float distance = Vector3.Distance(position, obj.transform.position);
            if (distance < minDistance)
            {
                nearestObject = obj;
                minDistance = distance;
            }
        }
        return nearestObject;
    }

    private void DisplayOpenDoorText(bool value)
    {
        OpenDoorText.SetActive(value);
    }
}