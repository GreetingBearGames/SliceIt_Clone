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

    
}
