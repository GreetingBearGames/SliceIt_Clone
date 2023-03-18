using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var angle = UnityEditor.TransformUtils.GetInspectorRotation(this.transform);
        Debug.Log(angle.x);
        Debug.Log(UnityEditor.TransformUtils.GetInspectorRotation(this.transform));
    }
}
