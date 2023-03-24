using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceDirection : MonoBehaviour
{
    GameObject slicable;

    public float[] rotations = { 90,  0,  -90,  -180};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slicable = GameObject.FindGameObjectWithTag("Slicable");

        transform.position = new Vector3(slicable.transform.position.x - 2, slicable.transform.position.y, slicable.transform.position.z);
    }
}
