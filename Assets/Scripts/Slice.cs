using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;


public class Slice : MonoBehaviour
{
    public Material[] mat;
    public GameObject sliceEffectPrefab;

    void Start()
    {

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
            Handheld.Vibrate();
            Destroy(sliceEffect, 1f);

            Destroy(other.gameObject);
            Destroy(slicedUp, 5);
            Destroy(slicedDown, 5);
        }
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
