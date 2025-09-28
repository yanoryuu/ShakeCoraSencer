using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateY : MonoBehaviour
{

    public Vector3 RotateCenter = Vector3.zero;
    public float RotateSpeed = 0.5f;

    
    void Update()
    {
        transform.RotateAround(RotateCenter, Vector3.up, Time.deltaTime * RotateSpeed);    
    }
}
