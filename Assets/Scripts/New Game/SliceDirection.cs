using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceDirection : MonoBehaviour
{
    GameObject slicable;

    public float[] rotations = { 165, 150, 135, 120, 105, 90, 75, 60, 45, 30, 15, 0, -15, -30, -45, -60, -75, -90, -105, -120, -135, -150, -165, -180};
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
