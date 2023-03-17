using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    [SerializeField] float knifeSpinAngle;
    [SerializeField] float jumpAmountY;
    [SerializeField] float jumpAmountZ;
    Rigidbody RigidBody;
    BoxCollider BoxCollider;
    Vector3 rotationX;
    Vector3 knifeDirection =  Vector3.forward;
    Vector3 jumpVector;
    float knifeAngle;
    bool isMove = true;
    bool isTouchingGround;
    bool isTapped;
    void Start()
    {
        RigidBody = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
        RigidBody.maxAngularVelocity = 10f;
        rotationX = new Vector3(knifeSpinAngle,0,0);
        jumpVector = new Vector3(0,jumpAmountY,jumpAmountZ);
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            isTapped = true;
            RigidBody.isKinematic = false;
            KnifeTurn();            
            Invoke("ResetTap",0.4f);
        }

        TurnSpeedControl();
        RigidBody.inertiaTensorRotation = Quaternion.identity;
    }

    void KnifeTurn(){
        RigidBody.velocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;

        if (isTouchingGround)
        {
            RigidBody.AddForce(new Vector3(0, jumpAmountY,jumpAmountZ/3), ForceMode.Impulse);
            BoxCollider.enabled = false;
            Invoke("EnableBoxCollider",0.4f);
            RigidBody.AddTorque(rotationX*1.5f, ForceMode.Acceleration);            
        }
        else
        {
            RigidBody.AddForce(jumpVector,ForceMode.Impulse);
            RigidBody.AddTorque(rotationX*1.5f,ForceMode.Acceleration);
        }        
    }
    void BaseTurn(){
        RigidBody.AddTorque(rotationX,ForceMode.Acceleration);
        RigidBody.angularVelocity = Vector3.zero;
        RigidBody.velocity = Vector3.zero;        
    }

    void TurnSpeedControl(){
        knifeAngle = Vector3.Angle(knifeDirection,transform.forward);
        if (isMove && knifeAngle < 30 && !isTapped)
        {
            RigidBody.maxAngularVelocity = 2f;
        }
        /*
        else if (isMove && knifeAngle > 90)
        {
            RigidBody.maxAngularVelocity = 10f;
            BaseTurn();            
        }*/
        else if(isTapped)
        {
            RigidBody.maxAngularVelocity = 10f;
        }
        else
        {
            RigidBody.maxAngularVelocity = 10f;
        }
    }

    //? Bunlar DoTween Delayed Call ile yapılabilir
    void ResetTap(){
        isTapped = false;
    }

    void EnableBoxCollider(){
        BoxCollider.enabled = true;
        isTouchingGround = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Ground")
        {
            isTouchingGround = true;
            RigidBody.isKinematic = true;
        }
    }
    
}
