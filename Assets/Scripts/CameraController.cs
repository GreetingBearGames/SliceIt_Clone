using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform knife;
    
    [SerializeField] float followSpeed;
    Vector3 offset;

    void Start()
    {
        offset = (transform.position) - knife.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,knife.position+offset,followSpeed);
    }
}
