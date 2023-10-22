using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puerta : MonoBehaviour
{
    public float speed;
    public float angle;
    public Vector3 direction;
    public bool check;
    public bool opencheck;
    public TextMeshProUGUI interactText;
    // Start is called before the first frame update
    void Start()
    {
        angle = transform.eulerAngles.y;
        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Mathf.Round(transform.eulerAngles.y) != angle)
        {
            transform.Rotate(direction * speed);
        }
        if (Input.GetButton("Abrir puerta") && check == true && opencheck==false)
        {
            angle = 80;
            direction = Vector3.up*Time.deltaTime;
            opencheck=true;
        }else if  (Input.GetButton("Abrir puerta") && check == true && opencheck==true){
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
            interactText.gameObject.SetActive(true);
        }
    }
        void OnTriggerExit(Collider other){
       if (other.gameObject.tag == "Player")
        {
            check = false;
            interactText.gameObject.SetActive(false);
        }
        }
}
