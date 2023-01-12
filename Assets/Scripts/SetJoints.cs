using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetJoints : MonoBehaviour
{
    public List<GameObject> brick;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject child in transform)
        {
            if (brick.Count < transform.childCount)
            {
                brick.Add(child);
            }
            for (int i = 0; i < brick.Count; i++)
            {
                child.GetComponent<FixedJoint>().connectedBody = brick[i + 1].GetComponent<Rigidbody>();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {



    }
}
