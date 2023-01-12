using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRefactored : MonoBehaviour
{
    Mesh mesh;
    Rigidbody rb;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        rb = GetComponent<Rigidbody>();
    }

    void DestoryMesh(UnityEngine.Collision collision)
    {
        Vector3[] vertices = mesh.vertices;
        int i = 0;
        Debug.Log(vertices.Length);
        while (i < vertices.Length)
        {
            vertices[i] += Vector3.up * 2;
            vertices[i].x = vertices[i].x + 5;
            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();

        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Collidable")
        {
            GetComponent<BoxCollider>().enabled = true;
            DestoryMesh(collision);
        }
    }
}
