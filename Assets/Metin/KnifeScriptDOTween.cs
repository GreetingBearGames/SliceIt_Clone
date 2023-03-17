using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnifeScriptDOTween : MonoBehaviour
{
    [SerializeField] [Range(0,2)] float turningTime;
    
    bool isTapped;
    float angle;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTapped = true;
            KnifeTurn();
        }
        
        //KnifeTurn(true);
    }

    void KnifeTurn(){
        angle = transform.rotation.eulerAngles.x;
        Debug.Log(angle);
        
        if (angle < 90 && !isTapped)
        {
            transform.DORotate(new Vector3(90,0,0),turningTime*2,RotateMode.FastBeyond360);
        }
        else if(angle >= 90 && !isTapped)
        {
            transform.DORotate(new Vector3(360,0,0),turningTime,RotateMode.FastBeyond360);
        }
        else if (isTapped)
        {
            transform.DORotate(new Vector3(360,0,0),turningTime,RotateMode.FastBeyond360);
        }
        

    }
}
