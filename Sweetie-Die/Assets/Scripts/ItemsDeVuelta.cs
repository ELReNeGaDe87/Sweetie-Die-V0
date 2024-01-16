using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDeVuelta : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public GameObject object3;
    public GameObject object4;
    public GameObject object5;

    private struct ObjectState
    {
        public GameObject obj;
        public Vector3 position;
        public Quaternion rotation;
    }

    private List<ObjectState> initialStates = new List<ObjectState>();

    void Start()
    {
        initialStates.Add(new ObjectState { obj = object1, position = object1.transform.position, rotation = object1.transform.rotation });
        initialStates.Add(new ObjectState { obj = object2, position = object2.transform.position, rotation = object2.transform.rotation });
        initialStates.Add(new ObjectState { obj = object3, position = object3.transform.position, rotation = object3.transform.rotation });
        initialStates.Add(new ObjectState { obj = object4, position = object4.transform.position, rotation = object4.transform.rotation });
        initialStates.Add(new ObjectState { obj = object5, position = object5.transform.position, rotation = object5.transform.rotation });
    }

    public void ReturnItems()
    {
        foreach (var state in initialStates)
        {
            state.obj.transform.position = state.position;
            state.obj.transform.rotation = state.rotation;
        }
    }
}