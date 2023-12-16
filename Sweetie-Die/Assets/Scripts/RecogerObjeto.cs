using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogerObjeto : MonoBehaviour
{
    public float pickupDistance = 1.0f;
    private CambiarCamara switchCamera;
    public Vector3 heldObjectPosition = new Vector3(0.6f, -0.4f, 1f);
    public static GameObject heldObject = null;
    private Quaternion originalRotation;
    [SerializeField]
    private GameObject pickUpObjectText;

    void Start()
    {
        switchCamera = FindObjectOfType<CambiarCamara>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
                {
                    if (hit.transform.gameObject != this.gameObject && hit.transform.gameObject.GetComponent<Rigidbody>() != null)
                    {
                        heldObject = hit.transform.gameObject;
                        originalRotation = heldObject.transform.rotation;
                        heldObject.GetComponent<Rigidbody>().isKinematic = true;
                        heldObject.transform.SetParent(transform);
                        heldObject.transform.localPosition = heldObjectPosition;
                    }
                }
            }
            else
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
                heldObject.transform.SetParent(null);
                heldObject.transform.rotation = originalRotation;
                heldObject = null;
            }
        }
    }
    public static bool HoldingObject()
    {
        return heldObject != null;
    }

    private void DisplayText(bool value)
    {
        pickUpObjectText.SetActive(value);
    }
}