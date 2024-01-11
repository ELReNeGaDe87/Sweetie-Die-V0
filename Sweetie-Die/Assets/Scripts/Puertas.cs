using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Puertas : MonoBehaviour
{
    private float currentRotation;
    private RaycastHit hit;
    public AudioClip puertablock;
    public AudioClip abrirpuerta;
    public AudioClip cerrarpuerta;
    public GameObject player;
    private bool canInteract = true;
    private RecogerObjeto recogerObjeto;
    private float originalRotation;
    private ConversationStarter conversationStarter;
    private PlayerController playerController;
    private ReadBookText readBookText;

    [SerializeField]
    private GameObject openDoorText;
    [SerializeField]
    private GameObject aimDot;
    [SerializeField]
    private Transform talkToMonsterTransform;

    void Start()
    {
        recogerObjeto = FindObjectOfType<RecogerObjeto>();
        conversationStarter = FindObjectOfType<ConversationStarter>();
        playerController = FindObjectOfType<PlayerController>();
        readBookText = FindObjectOfType<ReadBookText>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract && Physics.Raycast(transform.position, transform.forward, out hit, 2f) && hit.transform.tag.Contains("OpenDoorPlayerRoom") && Input.GetKeyDown(KeyCode.E))
        {
            if (!readBookText.IsShowing())
            {
                readBookText.Show();
            }
        }

        if (canInteract && Physics.Raycast(transform.position, transform.forward, out hit, 2f) && (hit.transform.tag.Contains("OpenDoor") && !hit.transform.tag.Contains("OpenDoorPlayerRoom")))
        {
            if (!openDoorText.activeSelf)
            {
                UnityEngine.Debug.Log("OpenDoor");
                showOpenDoorText(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(OpenDoor(hit.transform));
            }
            return;
        }
        if (canInteract && Physics.Raycast(transform.position, transform.forward, out hit, 2f) && hit.transform.tag.Contains("MonsterDoor"))
        {
            if (!openDoorText.activeSelf)
            {
                UnityEngine.Debug.Log("MonsterDoor");
                showOpenDoorText(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(OpenDoorM(hit.transform));
                playerController.Teleport(talkToMonsterTransform);
                conversationStarter.StartConversation();
            }
            return;
        }
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f) && hit.transform.CompareTag("CloseDoor") && Input.GetKeyDown(KeyCode.E))
        {
            GameObject nearestLockedDoor = FindNearestWithTag(player.transform.position, "CloseDoor");
            if (nearestLockedDoor != null)
            {
                AudioSource audioSource = nearestLockedDoor.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(puertablock);
                }
            }
            return;
        }
        if (openDoorText.activeSelf)
        {
            showOpenDoorText(false);
        }
    }

    IEnumerator OpenDoor(Transform door) 
    {
        canInteract = false;
        originalRotation = door.localEulerAngles.y;
        GameObject nearestOpenDoor = null;
        if (door.CompareTag("OpenDoorL"))
        {
            nearestOpenDoor = FindNearestWithTag(player.transform.position, "OpenDoorL");
            currentRotation = originalRotation - 90f;
        }
        else if (door.CompareTag("OpenDoorR"))
        {
            nearestOpenDoor = FindNearestWithTag(player.transform.position, "OpenDoorR");
            currentRotation = originalRotation + 90f;
        }
        if (nearestOpenDoor != null)
        {
            AudioSource audioSource = nearestOpenDoor.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(abrirpuerta);
            }
        }
        yield return Rotate(door, currentRotation);
        yield return new WaitForSeconds(1);
        yield return Rotate(door, originalRotation);
        if (nearestOpenDoor != null)
        {
            AudioSource audioSource = nearestOpenDoor.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(cerrarpuerta);
            }
        }
        canInteract = true;
    }

    IEnumerator OpenDoorM(Transform door)
    {
        canInteract = false;
        GameObject nearestMonsterDoor = null;
        originalRotation = door.localEulerAngles.y;
        if (door.CompareTag("MonsterDoorL"))
        {
            nearestMonsterDoor = FindNearestWithTag(player.transform.position, "MonsterDoorL");
            currentRotation = originalRotation - 90f;
        }
        else if (door.CompareTag("MonsterDoorR"))
        {
            nearestMonsterDoor = FindNearestWithTag(player.transform.position, "MonsterDoorR");
            currentRotation = originalRotation + 90f;
        }
        if (nearestMonsterDoor != null)
        {
            AudioSource audioSource = nearestMonsterDoor.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(abrirpuerta);
            }
        }
        yield return Rotate(door, currentRotation);
        yield return new WaitForSeconds(1);
        yield return Rotate(door, originalRotation);
        if (nearestMonsterDoor != null)
        {
            AudioSource audioSource = nearestMonsterDoor.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.PlayOneShot(cerrarpuerta);
            }
        }
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

    private void showOpenDoorText(bool value)
    {
        openDoorText.SetActive(value);
        aimDot.SetActive(!value);
    }
}