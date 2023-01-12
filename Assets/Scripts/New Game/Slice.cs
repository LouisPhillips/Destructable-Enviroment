using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice : MonoBehaviour
{
    private GameObject baseObj;
    private GameObject tipObj;
    // Start is called before the first frame update
    void Awake()
    {
        baseObj = GameObject.Find("Bottom");
        tipObj = GameObject.Find("Tip");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(UnityEngine.Collision collider)
    {
        collider.gameObject.GetComponent<ObjectSplit>().slice(collider);
        if (collider.gameObject.tag == ("Slicable"))
        {
            collider.gameObject.GetComponent<ObjectSplit>().hiltEntryPoint = baseObj.transform.position;
            collider.gameObject.GetComponent<ObjectSplit>().tipEntryPoint = tipObj.transform.position;
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collider)
    {
        if (collider.gameObject.tag == ("Slicable"))
        {
            collider.gameObject.GetComponent<ObjectSplit>().tipExitPoint = tipObj.transform.position;
            //collider.gameObject.GetComponent<ObjectSplit>().Cut(collider.transform);

            collider.gameObject.GetComponent<ObjectSplit>().slice(collider);
        }

    }
}
