using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    public Transform lookat;
    public Vector3 offset;
    void Start()
    {
        
    }

    void Update()
    {
        if (lookat == null)
            return;
        transform.position = lookat.position + offset;
        transform.LookAt(lookat);
    }
}
