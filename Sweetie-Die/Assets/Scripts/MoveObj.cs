using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObj : MonoBehaviour
{
    public float speedH;
    public float speedV;
    float moveH;
    float moveV;



    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused){
        moveH += speedH * Input.GetAxis("Mouse X");
        moveV -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(moveV, moveH, 0f);
        }
    }
}
