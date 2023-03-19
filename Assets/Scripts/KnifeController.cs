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
    [SerializeField] bool isFailed;
    [SerializeField] bool isMove = true;
    [SerializeField] bool isTapped;

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

    void Start()
    {
        RigidBody = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
        RigidBody.maxAngularVelocity = 10f;
        spinTorque = new Vector3(knifeSpinTorque,0,0);
    }

    private void Update() {
        if (isTouchingGround)
        {
            //RigidBody.velocity = Vector3.zero;
            //RigidBody.maxAngularVelocity = 0f;
            RigidBody.isKinematic = true;
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {   
            Debug.Log("Tıklama");
            isTapped = true;
            RigidBody.isKinematic = false;
            knifeEdgeCollider.isTrigger = true;
            TurnSpeedControl();
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
        knifeBackCollider.isTrigger = true;

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
        
        if(isFailed){
            RigidBody.maxAngularVelocity = 0f;
            RigidBody.velocity = Vector3.zero;
        }
    }
   
    void EnableCollider(){ //* Delayed Call ile yapılabilir
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
                knifeEdgeCollider.isTrigger = false;
            }
            else if (tag == "Cuttable"){
                isCutting = true;
                RigidBody.AddForce(Vector3.down,ForceMode.Impulse);
                DOVirtual.DelayedCall(1,(() => isCutting = false));      
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
                RigidBody.MovePosition(new Vector3(transform.position.x,transform.position.y,transform.position.z+jumpZBounce));
            }
            else if(!isCutting)
            {
                RigidBody.MovePosition(new Vector3(transform.position.x,transform.position.y,transform.position.z+jumpZBounce));
            }
        }
    }

    void Bounce(Collider other){
        Vector3 bounceDirection = (transform.position - other.transform.position).normalized;
        float tempY=jumpYBounce,tempZ=jumpZBounce;
        Debug.Log(bounceDirection);
        tempY = bounceDirection.y<0 ? tempY*-1 : tempY;
        tempZ = bounceDirection.z<0 ? tempZ*-1 : tempZ;

        if (other.CompareTag("Ground"))
        {
           RigidBody.AddTorque(new Vector3(0,tempY,tempZ).normalized,ForceMode.Impulse);
        }
        else if(other.CompareTag("Cuttable"))
        {
            tempY = 0;
            RigidBody.MovePosition(new Vector3(transform.position.x,transform.position.y+tempY,transform.position.z+tempZ));
        }
    }
    private void Fail(){
    }

}
