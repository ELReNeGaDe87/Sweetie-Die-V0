using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    private float pickupDistance = 1.5f;
    private CambiarCamara switchCamera;
    public UnityEngine.Vector3 heldObjectPosition = new UnityEngine.Vector3(0.6f, -0.4f, 1f);
    public GameObject heldObject = null;
    public ItemsDeVuelta itemsDeVuelta;
    private UnityEngine.Quaternion originalRotation;
    private UnityEngine.Vector3 originalPosition;
    private Transform originalParent;
    [SerializeField]
    private GameObject pickUpObjectText;
    [SerializeField]
    private GameObject switchObjectText;

    void Start()
    {
        switchCamera = FindObjectOfType<CambiarCamara>();
        itemsDeVuelta = FindObjectOfType<ItemsDeVuelta>();
    }

    void Update()
    {
        RaycastHit hit;
        bool isRaycastHit = Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance);
        bool canPickupObject = isRaycastHit && hit.transform != null && hit.transform.gameObject != this.gameObject && hit.transform.gameObject.CompareTag("Gift");

        if (heldObject != null)
        {
            switchObjectText.SetActive(canPickupObject);
            pickUpObjectText.SetActive(false);

            if (canPickupObject && Input.GetMouseButtonDown(0))
            {
                SwitchObjects(hit);
            }
        }
        else
        {
            switchObjectText.SetActive(false);
            pickUpObjectText.SetActive(canPickupObject);

            if (canPickupObject && Input.GetMouseButtonDown(0) && hit.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                PickupObject(hit);
            }
        }
    }

    void PickupObject(RaycastHit hit)
    {
        heldObject = hit.transform.gameObject;
        originalRotation = heldObject.transform.rotation;
        originalPosition = heldObject.transform.position;
        originalParent = heldObject.transform.parent;

        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.transform.SetParent(transform);
        heldObject.transform.localPosition = heldObjectPosition;
    }

    void SwitchObjects(RaycastHit hit)
    {
        UnityEngine.Vector3 previousPosition = heldObject.transform.position;
        UnityEngine.Quaternion previousRotation = heldObject.transform.rotation;

        heldObject.GetComponent<Rigidbody>().isKinematic = false;
        heldObject.transform.SetParent(null);
        heldObject.transform.rotation = originalRotation;

        if (hit.transform.gameObject != heldObject)
        {
            heldObject.transform.position = hit.transform.position;
            heldObject.transform.rotation = hit.transform.rotation;

            heldObject = hit.transform.gameObject;
            originalRotation = heldObject.transform.rotation;
            originalPosition = heldObject.transform.position;
            originalParent = heldObject.transform.parent;

            heldObject.GetComponent<Rigidbody>().isKinematic = true;
            heldObject.transform.SetParent(transform);
            heldObject.transform.localPosition = heldObjectPosition;
        }
        else
        {
            heldObject.transform.position = previousPosition;
            heldObject.transform.rotation = previousRotation;
        }
    }

    public bool HoldingObject()
    {
        return heldObject != null;
    }

    public void ReturnToSender()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.SetParent(originalParent);
            heldObject = null;
            itemsDeVuelta.ReturnItems();
        }
    }
}