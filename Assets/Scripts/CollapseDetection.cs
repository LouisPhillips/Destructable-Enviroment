using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseDetection : MonoBehaviour
{
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 100f);
        RaycastHit belowCheck;
        if(Physics.Raycast(transform.position, Vector3.down, out belowCheck, 100f))
        {
            if(belowCheck.transform.gameObject.tag != "Box" && belowCheck.transform.gameObject.layer != 3)
            {
                Debug.Log("not below");
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }
}
