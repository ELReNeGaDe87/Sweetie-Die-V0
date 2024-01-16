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
    private UnityEngine.Quaternion originalRotation;
    [SerializeField]
    private GameObject pickUpObjectText;
    [SerializeField]
    private GameObject switchObjectText;

    void Start()
    {
        switchCamera = FindObjectOfType<CambiarCamara>();
    }

    void Update()
    {
        RaycastHit hit;
        if (heldObject != null)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
            {
                if (hit.transform.gameObject.GetComponent<Rigidbody>() != null && hit.transform.gameObject.CompareTag("Gift"))
                {
                    if (!switchObjectText.activeSelf)
                    {
                        switchObjectText.SetActive(true);
                    }
                    if (Input.GetMouseButtonDown(0))
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
                }
                else
                {
                    if (switchObjectText.activeSelf)
                    {
                        switchObjectText.SetActive(false);
                    }
                }
            }
            else
            {
                if (switchObjectText.activeSelf)
                {
                    switchObjectText.SetActive(false);
                }
            }

            if (pickUpObjectText.activeSelf)
            {
                pickUpObjectText.SetActive(false);
            }
            return;
        }
        else
        {
            if (switchObjectText.activeSelf)
            {
                switchObjectText.SetActive(false);
            }
        };
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
        {
            if (hit.transform.gameObject != this.gameObject && hit.transform.gameObject.CompareTag("Gift"))
            {
                if (!pickUpObjectText.activeSelf)
                {
                    pickUpObjectText.SetActive(true);
                }

                if (Input.GetMouseButtonDown(0) && hit.transform.gameObject.GetComponent<Rigidbody>() != null)
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