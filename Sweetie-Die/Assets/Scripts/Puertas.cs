using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puertas : MonoBehaviour
{
    private float currentRotation;
    private RaycastHit hit;
    public AudioClip audioClip;
    // public Text messageText;
    public GameObject player;
    public string lockedDoorTag = "CloseDoor";
    private RecogerObjeto recogerObjeto;

    [SerializeField]
    private GameObject OpenDoorText;

    void Start()
    {
        recogerObjeto = FindObjectOfType<RecogerObjeto>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && hit.transform.CompareTag("OpenDoor") && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenDoor(hit.transform));
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && hit.transform.CompareTag("MonsterDoor") && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenDoorM(hit.transform));
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && hit.transform.CompareTag(lockedDoorTag) && Input.GetKeyDown(KeyCode.E))
        {
            GameObject nearestLockedDoor = FindNearestWithTag(player.transform.position, lockedDoorTag);
            if (nearestLockedDoor != null)
            {
                AudioSource audioSource = nearestLockedDoor.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(audioClip);
                }
            }
            //messageText.text = "Est� cerrada";
            //messageText.enabled = true;
            //StartCoroutine(DisableMessageAfterTime(2));
        }
    }

    IEnumerator OpenDoor(Transform door) 
    {
        currentRotation = door.localEulerAngles.y >= 0 && door.localEulerAngles.y < 180 ? -90f : 90f;

        yield return Rotate(door, currentRotation);

        yield return new WaitForSeconds(1);

        yield return Rotate(door, -currentRotation);
        
    }

    IEnumerator OpenDoorM(Transform door)
    {
        currentRotation = door.localEulerAngles.y >= 0 && door.localEulerAngles.y < 180 ? -90f : 90f;

        yield return Rotate(door, currentRotation);

        yield return new WaitForSeconds(1);

        yield return Rotate(door, -currentRotation);
        //if (recogerObjeto.HoldingObject())
        //{
        //    currentRotation = door.localEulerAngles.y >= 0 && door.localEulerAngles.y < 180 ? -90f : 90f;

        //    yield return Rotate(door, currentRotation);

        //    yield return new WaitForSeconds(1);

        //    yield return Rotate(door, -currentRotation);
        //}
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

    /*IEnumerator DisableMessageAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        messageText.enabled = false;
    }*/

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