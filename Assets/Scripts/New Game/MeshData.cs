using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData : MonoBehaviour
{
    public struct Triangle
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;

        public Vector3 getNormal()
        {
            return Vector3.Cross(v1 - v2, v1 - v3).normalized;
        }

        public void matchDirection(Vector3 dir)
        {
            if (Vector3.Dot(getNormal(), dir) > 0)
            {
                return;
            }
            else
            {
                Vector3 vector = v1;
                v1 = v3;
                v3 = vector;
            }
        }
    }

    public void MeshTriangle(Vector3 normal, Vector3 v1, Vector3 v2, Vector3 v3, List<Triangle> triangles)
    {
        Triangle tri = new Triangle() { v1 = v1, v2 = v2, v3 = v3 };
        tri.matchDirection(normal);
        triangles.Add(tri);
    }

    public int[] triangles;
    public Vector3[] verticies;
    public Vector2[] uvs;

  



}
