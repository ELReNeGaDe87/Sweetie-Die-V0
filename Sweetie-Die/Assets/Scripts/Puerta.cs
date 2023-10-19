using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public float speed;
    public float angle;
    public Vector3 direction;
    public bool check;
    public bool opencheck;

    // Start is called before the first frame update
    void Start()
    {
        angle = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Mathf.Round(transform.eulerAngles.y) != angle)
        {
            transform.Rotate(direction * speed);
        }
        if (Input.GetButtonDown("Fire1") && check == true && opencheck==false)
        {
            angle = 80;
            direction = Vector3.up*Time.deltaTime;
            opencheck=true;
        }else if  (Input.GetButtonDown("Fire1") && check == true && opencheck==true){
            angle=0;
            direction = Vector3.down*Time.deltaTime;
            opencheck=false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            check = true;
        }
    }
        void OnTriggerExit(Collider other){
       if (other.gameObject.tag == "Player")
        {
            check = false;
        }
        }
}
