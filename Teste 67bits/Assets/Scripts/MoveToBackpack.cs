using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBackpack : MonoBehaviour
{
    public bool move = false;
    [SerializeField]
    private Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(move){
            transform.position = Vector3.Lerp(transform.position,target.position, Time.deltaTime * 3);
        }
    }

    public void SetTarget(Transform t){
        target = t;
    }
}
