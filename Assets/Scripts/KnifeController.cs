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
    [SerializeField] float bounceY;
    [SerializeField] float bounceZ;

    [Header ("Stuck Movement")]
    [SerializeField] float jumpYStuck;
    [SerializeField] float jumpZStuck;
    
    [Space]
    [Header ("Controls")]

    [SerializeField] string lastTouchedObject;
    [SerializeField] bool isTouchingGround;
    [SerializeField] public bool isCutting;
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

    GameManager GameManager;
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
        GameManager = FindObjectOfType<GameManager>();
        Physics.gravity = new Vector3(0,-30f,0);   
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
        {   
            if (isFailed)
            {
                return;
            }
            SoundManager.instance.Play("KnifeTurn");
            isTapped = true;
            isCutting = false;
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

    void BounceBack(){
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(new Vector3(0,0,bounceZ),ForceMode.Impulse);

    }

    void BounceUp(){
        rigidBody.velocity = Vector3.zero;
        rigidBody.AddForce(new Vector3(0,bounceY,bounceZ),ForceMode.Impulse);
        Jump();
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
   
    void EnableCollider(){ 
        if (!isCutting)
        {
            knifeEdgeCollider.enabled = true;
        }
        
        knifeBackCollider.enabled = true;
        isTouchingGround = false;
    }

    public void CollisionController(Collider other,string sender){
        string tag = other.tag;
        lastTouchedObject = tag;
        if (sender == "KnifeEdge")
        {
            if(tag == "Ground"){
                SoundManager.instance.Play("KnifeHitFloor");
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
                isTouchingGround = true;
                isCutting = false;
            }
            else if (tag == "Cuttable"){
                SoundManager.instance.Play("Slice");
                SlicingControl();
            }
            else if (tag == "FailGround"){
                Fail();
            }
            else if(tag == "Empty" || tag == "Multiplier"){
                rigidBody.isKinematic = true;
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            }            
        }
        else if (sender == "KnifeBack")
        {
            rigidBody.isKinematic = false;
            SoundManager.instance.Play("KnifeHandleHit");
            if(tag == "FailGround"){
                Fail();
            }
            else if(tag == "Ground"){
                BounceUp();
                BaseTurn();
            }
            else if(!isCutting && tag=="Cuttable")
            {
                BounceBack();
                BaseTurn();
            }
            else if(tag == "Empty" || tag == "Multiplier"){
                BounceBack();
                BaseTurn();
            }
        }
    }
    private void Fail(){
        isFailed = true;
        rigidBody.constraints = RigidbodyConstraints.None;
        knifeBackCollider.isTrigger = false;
        knifeEdgeCollider.isTrigger = false;
        rigidBody.AddForce(new Vector3(70,70,0));
        rigidBody.isKinematic = false;
        GameManager.Instance.LoseLevel();
    }

    void SlicingControl(){
        SliceMovement();
        isCutting = true;
        knifeBackCollider.enabled = false;
        Invoke("EnableColliders",0.5f);
        DOVirtual.DelayedCall(0.1f,(() => isCutting = false)); 
    }

    void SliceMovement(){
        float angularVelocityX = rigidBody.angularVelocity.x;
        float velocityZ = rigidBody.velocity.z;

        velocityZ -=3;
        if (angularVelocityX == 0)
        {
            velocityZ = 0;
        }
        rigidBody.velocity = new Vector3(0,rigidBody.velocity.y,velocityZ);

        if (knifeAngle>30 || knifeAngle<0)
            return;

        angularVelocityX -= 3;
        if(knifeAngle >5 && knifeAngle< 15 && !isTapped || angularVelocityX < 0)
        {
            angularVelocityX = 0;
            rigidBody.angularVelocity = new Vector3(angularVelocityX,0,0);
        }
    }

}
