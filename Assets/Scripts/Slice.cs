using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;


public class Slice : MonoBehaviour
{
    [SerializeField] Material[] _sliceHalfMaterials;
    [SerializeField] GameObject _sliceParticle;
    [SerializeField] SliceCombiner _sliceCombiner;
    [SerializeField] SliceUISpawner _sliceUISpawner;
    [SerializeField] SliceScore _sliceScore;
    private bool isCollided = false;


    void Start()
    {

    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Cuttable" || other.gameObject.tag == "CuttableTahta")
        {
            int randomColorIndex = Random.Range(1, _sliceHalfMaterials.Length);

            SlicedHull slicedObject = Cut(other.gameObject, _sliceHalfMaterials[randomColorIndex]);
            GameObject slicedUp = slicedObject.CreateUpperHull(other.gameObject, _sliceHalfMaterials[randomColorIndex]);
            GameObject slicedDown = slicedObject.CreateLowerHull(other.gameObject, _sliceHalfMaterials[randomColorIndex]);
            AddComponent(slicedUp);
            AddComponent(slicedDown);

            GameObject sliceEffect = Instantiate(_sliceParticle, other.transform.position + new Vector3(0, 0, -1), Quaternion.Euler(-90, 0, 0));

            Destroy(other.gameObject);
            Destroy(slicedUp, 5);
            Destroy(slicedDown, 5);

            Handheld.Vibrate();
            _sliceCombiner.CounterIncrease();
            _sliceUISpawner.SpawnUIText(other.transform.position);
            _sliceScore.IncreaseScore();
        }


        if (other.gameObject.tag == "CuttableTahta" && !isCollided)
        {
            var parent = other.transform.parent;

            for (int i = 0; i < parent.childCount; i++)
            {
                isCollided = true;
                parent.GetChild(i).gameObject.AddComponent(typeof(Rigidbody));
                parent.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
                parent.GetChild(i).GetComponent<Rigidbody>().mass = 0.2f;
                if (i == parent.childCount - 1)
                {
                    isCollided = false;
                }
            }



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
