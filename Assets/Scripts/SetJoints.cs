using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetJoints : MonoBehaviour
{
    public List<Transform> brick;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform child in transform)
        {
            if (brick.Count < transform.childCount)
            {
                brick.Add(child);
            }
        }

        
    }
}
