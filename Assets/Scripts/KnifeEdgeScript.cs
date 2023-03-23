using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeEdgeScript : MonoBehaviour
{
   
    KnifeController knifeController;

    
    void Start()
    {
        knifeController = GetComponentInParent<KnifeController>();
    }

    private void OnTriggerEnter(Collider other) {
        knifeController.CollisionController(other,"KnifeEdge");
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ground"))
        {
            GetComponent<MeshCollider>().enabled = true;
        }
    }

    
}
