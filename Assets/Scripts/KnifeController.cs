using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class KnifeController : MonoBehaviour
{
    [Header ("Knife Movement Controls")]
    [Space]

    [Header ("Base Spin")]
    [SerializeField] float knifeSpinTorque;
    [SerializeField] float jumpY;
    [SerializeField] float jumpZ;
    [SerializeField] float maxAVSlow;
    [SerializeField] float maxAVFast;

    [Space]
    [Header ("Bounce Movement")]
    [SerializeField] float jumpYBounce;
    [SerializeField] float jumpZBounce;

    [Header ("Stuck Movement")]
    [SerializeField] float jumpYStuck;
    [SerializeField] float jumpZStuck;
    
    [Space]
    [Header ("Controls")]
    [SerializeField] bool isTouchingGround;
    [SerializeField] bool isCutting;

    [Space]
    [Header ("Colliders")]
    [SerializeField] Collider knifeBackCollider;
    [SerializeField] Collider knifeEdgeCollider;

    [Space]
    [Header ("Scripts")]
    [SerializeField] Slice backSliceScript;

    Rigidbody RigidBody;
    BoxCollider BoxCollider;
    Vector3 spinTorque;
    float knifeAngle;
    bool isMove = true;
    
    bool isTapped;

    void Start()
    {
        RigidBody = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
        RigidBody.maxAngularVelocity = 10f;
        spinTorque = new Vector3(knifeSpinTorque,0,0);
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isTapped = true;
            RigidBody.isKinematic = false;
            Jump();
            Spin();            
            DOVirtual.DelayedCall(0.4f,(()=>isTapped = false));
        }

        TurnSpeedControl();
        RigidBody.inertiaTensorRotation = Quaternion.identity;
    }

    void Jump(){
        RigidBody.velocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;

        if (isTouchingGround)
        {
            knifeEdgeCollider.enabled = false;
            Invoke("EnableCollider",0.4f);
            RigidBody.AddForce(new Vector3(0,jumpYStuck,jumpZStuck),ForceMode.Impulse);
            isTouchingGround = false;
        }
        else
        {
            RigidBody.AddForce(new Vector3(0,jumpY,jumpZ),ForceMode.Impulse);
        } 
    }

    void Spin(){
        RigidBody.AddTorque(spinTorque,ForceMode.Acceleration);
    }

    void BaseTurn(){
        RigidBody.AddTorque(spinTorque,ForceMode.Acceleration);
        RigidBody.maxAngularVelocity = maxAVFast;
        RigidBody.angularVelocity = Vector3.zero;     
    }

    void TurnSpeedControl(){
        var angle = UnityEditor.TransformUtils.GetInspectorRotation(this.transform);
        knifeAngle = angle.x;

        if(knifeAngle >-10 && knifeAngle< 0 && !isTapped){
            RigidBody.maxAngularVelocity = maxAVSlow;
        }
        
        if(knifeAngle>110 && knifeAngle <120){
            BaseTurn();
        }

        if(isTapped){
            RigidBody.maxAngularVelocity = maxAVFast;
        }
    }
   
    //* Bunlar DoTween Delayed Call ile yapılabilir

    void EnableCollider(){
        knifeEdgeCollider.enabled = true;
        isTouchingGround = false;
    }

    public void CollisionController(Collider other,string sender){
        string tag = other.tag;
        
        if (sender == "KnifeEdge")
        {
            if(tag == "Ground"){
                isTouchingGround = true;
                isCutting = false;
                RigidBody.isKinematic = true;
                RigidBody.angularVelocity = Vector3.zero;
            }
            else if (tag == "Cuttable"){
                isCutting = true;
                RigidBody.AddForce(Vector3.down,ForceMode.Impulse);
                DOVirtual.DelayedCall(1,(() => isCutting = false));      
            }
            else if (tag == "FailObstacle"){
                //* Fail olacağı durumlar
            }            
        }
        else if (sender == "KnifeBack")
        {
            if(tag == "FailObstacle"){
                //* Fail olacağı durumlar
            }
            else if(!isCutting)
            {
                RigidBody.AddForce(new Vector3(0,jumpYBounce,jumpZBounce),ForceMode.Impulse);
                RigidBody.velocity = Vector3.zero;
            }
        }
    }
}
