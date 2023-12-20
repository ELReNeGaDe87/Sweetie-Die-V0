using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public float pickupDistance = 1.0f;
    private CambiarCamara switchCamera;
    public Vector3 heldObjectPosition = new Vector3(0.6f, -0.4f, 1f);
    public GameObject heldObject = null;
    private Quaternion originalRotation;
    [SerializeField]
    private GameObject pickUpObjectText;

    void Start()
    {
        switchCamera = FindObjectOfType<CambiarCamara>();
    }

    void Update()
    {
        if (heldObject != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.transform.SetParent(null);
                heldObject.transform.rotation = originalRotation;
                heldObject = null;
            }
            if (pickUpObjectText.activeSelf)
            {
                pickUpObjectText.SetActive(false);
            }
            return;
        };

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
        {
            if (hit.transform.gameObject != this.gameObject && hit.transform.gameObject.CompareTag("Gift"))
            {
                if (!pickUpObjectText.activeSelf)
                {
                    pickUpObjectText.SetActive(true);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    heldObject = hit.transform.gameObject;
                    originalRotation = heldObject.transform.rotation;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.transform.SetParent(transform);
                    heldObject.transform.localPosition = heldObjectPosition;
                }
            }
            else
            {
                // If the hit object is not a "Gift," set the text to inactive.
                if (pickUpObjectText.activeSelf)
                {
                    pickUpObjectText.SetActive(false);
                }
            }
        }
        else
        {
            // If no object is hit, set the text to inactive.
            if (pickUpObjectText.activeSelf)
            {
                pickUpObjectText.SetActive(false);
            }
        }

    }
    public bool HoldingObject()
    {
        return heldObject != null;
    }
}