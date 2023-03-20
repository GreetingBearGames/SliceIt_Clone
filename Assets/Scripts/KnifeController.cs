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

    [SerializeField] string touchingObject;
    [SerializeField] bool isTouchingGround;
    [SerializeField] bool isCutting;
    [SerializeField] bool isFailed;
    [SerializeField] bool isMove = true;
    [SerializeField] bool isTapped;
    [SerializeField] bool isBounce;

    [Space]
    [Header ("Colliders")]
    [SerializeField] Collider knifeBackCollider;
    [SerializeField] Collider knifeEdgeCollider;

    [Space]
    //[Header ("Scripts")]


    Rigidbody rigidBody;
    BoxCollider boxCollider;
    Vector3 spinTorque;
    float knifeAngle;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        rigidBody.maxAngularVelocity = 10f;
        spinTorque = new Vector3(knifeSpinTorque,0,0);        
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {   
            if (isFailed)
            {
                return;
            }
            isTapped = true;
            rigidBody.isKinematic = false;
            knifeEdgeCollider.isTrigger = true;
            TurnSpeedControl();
            Jump();
            Spin();            
            DOVirtual.DelayedCall(0.4f,(()=>isTapped = false));
        }

        if (isTouchingGround)
        {
            rigidBody.isKinematic = true;
        }
        
        if (!isFailed)
        {
            rigidBody.inertiaTensorRotation = Quaternion.identity;
        }
        
    }
    private void FixedUpdate()
    {
        TurnSpeedControl();
        rigidBody.inertiaTensorRotation = Quaternion.identity;
    }

    void Jump(){
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;

        if (isTouchingGround)
        {
            knifeEdgeCollider.enabled = false;
            Invoke("EnableCollider",0.4f);
            rigidBody.AddForce(new Vector3(0,jumpYStuck,jumpZStuck),ForceMode.Impulse);
            isTouchingGround = false;
        }
        else
        {
            rigidBody.AddForce(new Vector3(0,jumpY,jumpZ),ForceMode.Impulse);
        } 
    }

    void Spin(){
        rigidBody.AddTorque(spinTorque,ForceMode.Acceleration);
    }

    void BaseTurn(){
        rigidBody.AddTorque(spinTorque,ForceMode.Acceleration);
        rigidBody.maxAngularVelocity = maxAVFast;
        rigidBody.angularVelocity = Vector3.zero;     
    }

    void TurnSpeedControl(){
        var angle = UnityEditor.TransformUtils.GetInspectorRotation(this.transform);
        knifeAngle = angle.x;
        
        if (isBounce)
        {
            return;
        }

        if(knifeAngle >-10 && knifeAngle< 0 && !isTapped)
        {
            rigidBody.maxAngularVelocity = maxAVSlow;
        }/* 
        else if(knifeAngle>240 && knifeAngle <260){
            rigidBody.AddTorque(spinTorque*1.5f,ForceMode.Acceleration);
            rigidBody.maxAngularVelocity = maxAVFast*1.5f;
        }
        else if((knifeAngle>260 && knifeAngle <270))
        {
            rigidBody.maxAngularVelocity = maxAVFast;
        }*/
        else if(knifeAngle>110 && knifeAngle <120){
            BaseTurn();
        }

        if(isTapped){
            rigidBody.maxAngularVelocity = maxAVFast;
        }
        
        if(isFailed){
            rigidBody.maxAngularVelocity = 0f;
            rigidBody.velocity = Vector3.zero;
        }
    }
   
    void EnableCollider(){ //* Delayed Call ile yapılabilir
        knifeEdgeCollider.enabled = true;
        knifeBackCollider.enabled = true;
        isTouchingGround = false;
    }

    public void CollisionController(Collider other,string sender){
        string tag = other.tag;
        touchingObject = tag;
        if (sender == "KnifeEdge")
        {
            if(tag == "Ground"){
                isTouchingGround = true;
                isCutting = false;
            }
            else if (tag == "Cuttable"){
                isCutting = true;
                DOVirtual.DelayedCall(0.1f,(() => isCutting = false)); 
            }
            else if (tag == "Obstacle"){
                Fail();
            }            
        }
        else if (sender == "KnifeBack")
        {
            if(tag == "Obstacle"){
                Fail();
            }
            else if(tag == "Ground"){
                BaseTurn();
            }
            else if(!isCutting && tag=="Cuttable")
            {
                Bounce();
            }
        }
    }
    void Bounce(){
        isBounce = true;
        rigidBody.MovePosition(new Vector3(transform.position.x,transform.position.y,transform.position.z+jumpZBounce));
        rigidBody.AddTorque(spinTorque,ForceMode.Acceleration);
        rigidBody.maxAngularVelocity = maxAVFast;
        DOVirtual.DelayedCall(0.3f,(()=>isBounce=false));
    }
    private void Fail(){
        isFailed = true;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.AddForce(new Vector3(0,7,0));
        rigidBody.isKinematic = false;
        knifeBackCollider.isTrigger = false;
        knifeEdgeCollider.isTrigger = false;
    }

}
