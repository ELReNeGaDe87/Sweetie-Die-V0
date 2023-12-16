using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;

    private void Update()
    {
        transform.position = cameraTransform.position;
        transform.rotation = cameraTransform.rotation;
    }
}