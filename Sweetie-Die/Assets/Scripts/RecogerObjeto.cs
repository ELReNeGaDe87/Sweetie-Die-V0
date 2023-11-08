using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public float pickupDistance = 1.0f;
    public Vector3 heldObjectPosition = new Vector3(0.6f, -0.4f, 1f);
    private GameObject heldObject = null;
    private Quaternion originalRotation;

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickupDistance))
                {
                    heldObject = hit.transform.gameObject;
                    originalRotation = heldObject.transform.rotation;
                    heldObject.GetComponent<Rigidbody>().isKinematic = true;
                    heldObject.transform.SetParent(transform);
                    heldObject.transform.localPosition = heldObjectPosition;
                }
            }
        }
        else if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.SetParent(null);
            heldObject.transform.rotation = originalRotation;
            heldObject = null;
        }
    }
}