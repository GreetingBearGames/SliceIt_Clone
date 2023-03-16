using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;


public class Slice : MonoBehaviour
{
    private GameManager gm;
    public Material[] mat;
    private Rotate rotate;
    public GameObject sliceEffectPrefab;

    void Start()
    {
        rotate = Rotate.instance;
        gm = GameObject.FindObjectOfType<GameManager>();
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Cuttable")
        {
            int randomColorIndex = Random.Range(1, mat.Length);

            SlicedHull slicedObject = Cut(other.gameObject, mat[randomColorIndex]);
            GameObject slicedUp = slicedObject.CreateUpperHull(other.gameObject, mat[randomColorIndex]);
            GameObject slicedDown = slicedObject.CreateLowerHull(other.gameObject, mat[randomColorIndex]);

            AddComponent(slicedUp);
            AddComponent(slicedDown);

            GameObject sliceEffect = Instantiate(sliceEffectPrefab, other.transform.position + new Vector3(0, 0, -1), Quaternion.Euler(-90, 0, 0));
            Destroy(sliceEffect, 1f);

            Destroy(other.gameObject);

            //gm.GameScore(25);
        }

        if (other.gameObject.tag == "GroundActive")
        {

            rotate.CollideWithGround();
        }

        if (other.gameObject.tag == "Obstacle")
        {
            gm.RestartGame();
        }

        if (other.gameObject.tag == "FinishLine")
        {
            gm.NextLevel();
        }


    }


    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "GroundActive")
            rotate.CollideWithGround();
    }
    public SlicedHull Cut(GameObject obj, Material mat = null)
    {
        return obj.Slice(transform.position, obj.transform.right, mat);
    }

    private void AddComponent(GameObject obj)
    {

        obj.AddComponent<MeshCollider>().convex = true;
        obj.AddComponent<Rigidbody>();
        obj.GetComponent<Rigidbody>().mass = 0.05f;
        obj.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
        obj.GetComponent<Rigidbody>().AddExplosionForce(10, obj.transform.position, 10);
    }


}
