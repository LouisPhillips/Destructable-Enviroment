using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // The mesh to split
    public Mesh mesh;

    // The point at which to split the mesh
    public Vector3 splitPoint;

    // The normal of the plane on which to split the mesh
    public Vector3 splitNormal;

    // The resulting meshes after splitting
    public Mesh mesh1;
    public Mesh mesh2;

    void Start()
    {
        // Compute the plane on which to split the mesh
        Plane splitPlane = new Plane(splitNormal, splitPoint);

        // Split the mesh into two halves
        Test.SplitMesh(mesh, splitPlane, out mesh1, out mesh2);
    }

    // Split a mesh by a plane
    public static void SplitMesh(Mesh mesh, Plane splitPlane, out Mesh mesh1, out Mesh mesh2)
    {
        mesh1 = new Mesh();
        mesh2 = new Mesh();

        // Create lists to store the vertices and triangles of each mesh
        List<Vector3> verts1 = new List<Vector3>();
        List<Vector3> verts2 = new List<Vector3>();
        List<int> tris1 = new List<int>();
        List<int> tris2 = new List<int>();

        // Loop through each triangle in the mesh
        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            // Get the triangle vertices
            Vector3 vert1 = mesh.vertices[mesh.triangles[i]];
            Vector3 vert2 = mesh.vertices[mesh.triangles[i + 1]];
            Vector3 vert3 = mesh.vertices[mesh.triangles[i + 2]];

            // Compute the side of the split plane for each vertex
            splitPlane.GetSide(vert1);
            splitPlane.GetSide(vert2);
            splitPlane.GetSide(vert3);

            float side1 = vert1.x;
            float side2 = vert2.x;
            float side3 = vert3.x;

            // If all vertices are on the same side of the plane, add the triangle
            // to the appropriate mesh
            if (side1 > 0 && side2 > 0 && side3 > 0)
            {
                verts1.Add(vert1);
                verts1.Add(vert2);
                verts1.Add(vert3);
                tris1.Add(verts1.Count - 3);
                tris1.Add(verts1.Count - 2);
                tris1.Add(verts1.Count - 1);
            }
            else if (side1 < 0 && side2 < 0 && side3 < 0)
            {
                verts2.Add(vert1);
                verts2.Add(vert2);
                verts2.Add(vert3);
                tris2.Add(verts2.Count - 3);
                tris2.Add(verts2.Count - 2);
                tris2.Add(verts2.Count - 1);
            }
            // If the triangle spans the plane, split it into two triangles
            // and add them to the appropriate mesh
            else
            {
                // Compute the points where the triangle intersects the split plane
                Vector3[] splitPoints;
                if (SplitTriangle(vert1, vert2, vert3, splitPlane, out splitPoints))
                {
                    // Add the two new triangles to the appropriate mesh
                    if (side1 > 0 && side2 > 0)
                    {
                        verts1.Add(vert1);
                        verts1.Add(vert2);
                        verts1.Add(splitPoints[0]);
                        tris1.Add(verts1.Count - 3);
                        tris1.Add(verts1.Count - 2);
                        tris1.Add(verts1.Count - 1);

                        verts1.Add(splitPoints[0]);
                        verts1.Add(splitPoints[1]);
                        verts1.Add(vert3);
                        tris1.Add(verts1.Count - 3);
                        tris1.Add(verts1.Count - 2);
                        tris1.Add(verts1.Count - 1);
                    }
                    else if (side1 > 0 && side3 > 0)
                    {
                        verts1.Add(vert1);
                        verts1.Add(vert3);
                        verts1.Add(splitPoints[0]);
                        tris1.Add(verts1.Count - 3);
                        tris1.Add(verts1.Count - 2);
                        tris1.Add(verts1.Count - 1);

                        verts1.Add(splitPoints[0]);
                        verts1.Add(splitPoints[1]);
                        verts1.Add(vert2);
                        tris1.Add(verts1.Count - 3);
                        tris1.Add(verts1.Count - 2);
                        tris1.Add(verts1.Count - 1);
                    }
                    else if (side2 > 0 && side3 > 0)
                    {
                        verts1.Add(vert2);
                        verts1.Add(vert3);
                        verts1.Add(splitPoints[0]);
                        tris1.Add(verts1.Count - 3);
                        tris1.Add(verts1.Count - 2);
                        tris1.Add(verts1.Count - 1);

                        verts1.Add(splitPoints[0]);
                        verts1.Add(splitPoints[1]);
                        verts1.Add(vert1);
                        tris1.Add(verts1.Count - 3);
                        tris1.Add(verts1.Count - 2);
                        tris1.Add(verts1.Count - 1);
                    }
                    else if (side1 < 0 && side2 < 0)
                    {
                        // Add the two new triangles to the appropriate mesh
                        verts2.Add(vert1);
                        verts2.Add(vert3);
                        verts2.Add(splitPoints[0]);
                        tris2.Add(verts2.Count - 3);
                        tris2.Add(verts2.Count - 2);
                        tris2.Add(verts2.Count - 1);

                        verts2.Add(splitPoints[0]);
                        verts2.Add(splitPoints[1]);
                        verts2.Add(vert2);
                        tris2.Add(verts2.Count - 3);
                        tris2.Add(verts2.Count - 2);
                        tris2.Add(verts2.Count - 1);
                    }
                    else if (side2 < 0 && side3 < 0)
                    {
                        verts2.Add(vert2);
                        verts2.Add(vert3);
                        verts2.Add(splitPoints[0]);
                        tris2.Add(verts2.Count - 3);
                        tris2.Add(verts2.Count - 2);
                        tris2.Add(verts2.Count - 1);

                        verts2.Add(splitPoints[0]);
                        verts2.Add(splitPoints[1]);
                        verts2.Add(vert1);
                        tris2.Add(verts2.Count - 3);
                        tris2.Add(verts2.Count - 2);
                        tris2.Add(verts2.Count - 1);
                    }
                }
            }
        }

        // Set the vertices and triangles of each mesh
        mesh1.vertices = verts1.ToArray();
        mesh1.triangles = tris1.ToArray();
        mesh2.vertices = verts2.ToArray();
        mesh2.triangles = tris2.ToArray();
    }

    // Split a triangle by a plane
    public static bool SplitTriangle(Vector3 vert1, Vector3 vert2, Vector3 vert3, Plane splitPlane, out Vector3[] splitPoints)
    {
        // Compute the side of the split plane for each vertex
        splitPlane.GetSide(vert1);
        splitPlane.GetSide(vert2);
        splitPlane.GetSide(vert3);

        float side1 = vert1.x;
        float side2 = vert2.x;
        float side3 = vert3.x;

        // If the triangle is on one side of the plane, return no split points
        if (side1 > 0 && side2 > 0 && side3 > 0)
        {
            splitPoints = null;
            return false;
        }
        else if (side1 < 0 && side2 < 0 && side3 < 0)
        {
            splitPoints = null;
            return false;
        }

        // Compute the points where the triangle intersects the split plane
        splitPoints = new Vector3[2];
        int splitPointCount = 0;
        if (side1 > 0 && side2 <= 0 || side1 <= 0 && side2 > 0)
        {
            splitPoints[splitPointCount++] = Vector3.Lerp(vert1, vert2, side1 / (side1 - side2));
        }
        if (side1 > 0 && side2 <= 0 || side1 <= 0 && side2 > 0)
        {
            splitPoints[splitPointCount++] = Vector3.Lerp(vert1, vert2, side1 / (side1 - side2));
        }
        if (side1 > 0 && side3 <= 0 || side1 <= 0 && side3 > 0)
        {
            splitPoints[splitPointCount++] = Vector3.Lerp(vert1, vert3, side1 / (side1 - side3));
        }
        if (side2 > 0 && side3 <= 0 || side2 <= 0 && side3 > 0)
        {
            splitPoints[splitPointCount++] = Vector3.Lerp(vert2, vert3, side2 / (side2 - side3));
        }

        // Return whether the triangle intersects the plane
        return splitPointCount == 2;
    }
}
