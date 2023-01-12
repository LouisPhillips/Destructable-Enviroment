using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMesh : MonoBehaviour
{
    private List<Vector2> uvs = new List<Vector2>();
    private List<Vector3> verticies = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<List<int>> triangles = new List<List<int>>();

    public Vector3[] newVertices;
    public Vector3[] newNormals;
    public int[][] newTriangles;
    public Vector2[] newUVs;

    public GameObject newMesh;

    public Bounds Bounds = new Bounds();

    //Gets original mesh data
    public void GetVerticies(Vector3 verticy1, Vector3 verticy2, Vector3 verticy3)
    {
        verticies.Add(verticy1);
        verticies.Add(verticy2);
        verticies.Add(verticy3);
    }

    public void GetNormals(Vector3 normal1, Vector3 normal2, Vector3 normal3)
    {
        normals.Add(normal1);
        normals.Add(normal2);
        normals.Add(normal3);
    }

    public void GetUVS(Vector2 uv1, Vector2 uv2, Vector2 uv3)
    {
        uvs.Add(uv1);
        uvs.Add(uv2);
        uvs.Add(uv3);
    }

    public void CreateTriangles(int submesh)
    {
        triangles[submesh].Add(verticies.Count);
        triangles[submesh].Add(verticies.Count);
        triangles[submesh].Add(verticies.Count);
    }

    public void CreateMesh(PartMesh original)
    {
        newMesh = new GameObject(original.name);
        newMesh.transform.position = original.transform.position;
        newMesh.transform.rotation = original.transform.rotation;
        newMesh.transform.localScale = original.transform.localScale;
        // fills newly created mesh with new point data
        var mesh = new Mesh();
        mesh.vertices = newVertices;
        mesh.normals = newNormals;
        mesh.uv = newUVs;

        mesh.name = original.GetComponent<MeshFilter>().mesh.name;

        for (var i = 0; i < newTriangles.Length; i++)
        {
            mesh.SetTriangles(newTriangles[i], i, true);
        }
        Bounds = mesh.bounds;

        // add components to newly split mesh
        newMesh.AddComponent<MeshRenderer>();
        newMesh.GetComponent<MeshRenderer>().materials = original.GetComponent<MeshRenderer>().materials;

        newMesh.AddComponent<MeshFilter>();
        newMesh.GetComponent<MeshFilter>().mesh = mesh;

        newMesh.AddComponent<MeshCollider>();
        newMesh.GetComponent<MeshCollider>().convex = true;

        newMesh.AddComponent<Rigidbody>();

        newMesh.AddComponent<Despawn>();
    }
}
