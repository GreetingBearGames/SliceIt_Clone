using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public static Rotate instance;

    private Rigidbody rb;
    public GameObject ground;
    private bool isCollideWithGround;
    private bool isCollideWithKnifeHolder = false;



    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

       
    }


    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {

            if (isCollideWithGround)
            {
                rb.isKinematic = false;
                isCollideWithGround = false;
                rb.useGravity = true;
                Invoke("DeactiveGroundTag", .3f);
            }

            Jump();
            Spin();
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollideWithKnifeHolder)
        {
            JumpBack();
            isCollideWithKnifeHolder = true;
            Invoke("KnifeHolderCollidePermission", .5f);

            
        }

        Spin();

    }


    private void Jump()
    {
        Vector3 jumpForce = new Vector3(0, 10, 2f);

        rb.velocity = Vector3.zero;
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    private void JumpBack()
    {
        Vector3 jumpForce = new Vector3(0, 10, -2f);

        rb.velocity = Vector3.zero;
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }

    private void Spin()
    {

        rb.AddTorque(Vector3.right * 90000000, ForceMode.Acceleration);
    }

    private void DeactiveGroundTag()
    {
        ground.gameObject.tag = "GroundActive";
    }

    private void KnifeHolderCollidePermission()
    {
        isCollideWithKnifeHolder = false;
    }

    public void CollideWithGround()
    {
        ground.gameObject.tag = "GroundDeactive";
        rb.isKinematic = true;
        rb.useGravity = false;
        //rb.angularVelocity = Vector3.zero;
        isCollideWithGround = true;
    }

}
