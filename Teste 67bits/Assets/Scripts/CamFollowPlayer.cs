using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    [SerializeField]
    Vector3 offSet;
    [SerializeField]
    GameObject target;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition,target.transform.localPosition + offSet,Time.deltaTime * 2.5f);
    }
}
