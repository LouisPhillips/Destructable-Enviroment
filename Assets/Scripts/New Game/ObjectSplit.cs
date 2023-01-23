using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSplit : MonoBehaviour
{
    private MeshData meshData;

    Vector3 normal;
    List<MeshData.Triangle> newTriangles1;
    List<MeshData.Triangle> newTriangles2;

    public void SliceObject(Collider other)
    {
        Plane plane = CreatePlane();

        meshData = new MeshData();

        meshData.triangles = other.gameObject.GetComponent<MeshFilter>().mesh.triangles;
        meshData.verticies = other.gameObject.GetComponent<MeshFilter>().mesh.vertices;

        List<Vector3> intersections = new List<Vector3>();
        newTriangles1 = new List<MeshData.Triangle>();
        newTriangles2 = new List<MeshData.Triangle>();

        CheckTriangles(other, plane, meshData.triangles, meshData.verticies, intersections, newTriangles1, newTriangles2);
    }

    private Plane CreatePlane()
    {
        Collider collider = GetComponent<Collider>();
        Vector3 centreBounds = collider.bounds.center;
        Vector3 extentsBounds = collider.bounds.extents;

        Vector3 vec1 = centreBounds + transform.up * extentsBounds.y;
        Vector3 vec2 = centreBounds + transform.up * extentsBounds.y + transform.right * centreBounds.x;
        Vector3 vec3 = centreBounds + transform.up * -extentsBounds.y + transform.right * extentsBounds.x;

        return new Plane(vec1, vec2, vec3);
    }

    private bool CheckIntersection(List<Vector3> points, Plane plane, Vector3 point1, Vector3 point2)
    {
        Vector3 direction = point2 - point1;
        float length;
        Vector3 intersection;


        if (plane.Raycast(new Ray(point1, direction), out length) && length <= direction.magnitude)
        {
            return true;
        }
        return false;
    }
    void CheckTriangles(Collider collisionObject, Plane plane, int[] tris, Vector3[] verts, List<Vector3> intersections, List<MeshData.Triangle> newTris1, List<MeshData.Triangle> newTris2)
    {

        for (int i = 0; i < tris.Length; i += 3)
        {
            List<Vector3> points = new List<Vector3>();

            int vert1 = tris[i];
            int vert2 = tris[i + 1];
            int vert3 = tris[i + 2];
            Vector3 point1 = collisionObject.transform.TransformPoint(verts[vert1]);
            Vector3 point2 = collisionObject.transform.TransformPoint(verts[vert2]);
            Vector3 point3 = collisionObject.transform.TransformPoint(verts[vert3]);

            Vector3 direction = point2 - point1;
            float length;

            Vector3 intersection;
            Debug.Log(plane.distance);
            if (plane.Raycast(new Ray(point1, direction), out length) && length <= direction.magnitude)
            {
                intersection = point1 + length * direction.normalized;
                TriangleIntersection(points, intersections, intersection);
            }
            direction = point3 - point2;

            if (plane.Raycast(new Ray(point2, direction), out length) && length <= direction.magnitude)
            {
                intersection = point2 + length * direction.normalized;
                TriangleIntersection(points, intersections, intersection);
            }
            direction = point3 - point1;

            if (plane.Raycast(new Ray(point1, direction), out length) && length <= direction.magnitude)
            {
                intersection = point1 + length * direction.normalized;
                TriangleIntersection(points, intersections, intersection);
            }
            
            if (points.Count > 0)
            {
                // Make new triangles based off entry points
                MakeNewTriangles(plane, points, point1, point2, point3);
            }
            else
            {
                if (plane.GetSide(point1))
                {
                    newTriangles1.Add(new MeshData.Triangle() { v1 = point1, v2 = point2, v3 = point3 });
                }
                else
                {
                    newTriangles2.Add(new MeshData.Triangle() { v1 = point1, v2 = point2, v3 = point3 });
                }
            }
        }

        MakeObject(plane, intersections, collisionObject);
    }

    void MakeNewTriangles(Plane plane, List<Vector3> points, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        List<Vector3> leftSidePoints = new List<Vector3>();
        List<Vector3> rightSidePoints = new List<Vector3>();

        leftSidePoints.AddRange(points);
        rightSidePoints.AddRange(points);

        if (plane.GetSide(point1))
        {
            leftSidePoints.Add(point1);
        }
        else
        {
            rightSidePoints.Add(point1);
        }

        if (plane.GetSide(point2))
        {
            leftSidePoints.Add(point2);
        }
        else
        {
            rightSidePoints.Add(point2);
        }

        if (plane.GetSide(point3))
        {
            leftSidePoints.Add(point3);
        }
        else
        {
            rightSidePoints.Add(point3);
        }

        if (leftSidePoints.Count == 3)
        {
            meshData.MeshTriangle(normal, leftSidePoints[1], leftSidePoints[0], leftSidePoints[2], newTriangles1);
        }
        else
        {
            if (Vector3.Dot((leftSidePoints[0] - leftSidePoints[1]), leftSidePoints[2] - leftSidePoints[3]) >= 0)
            {
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[2], leftSidePoints[3], newTriangles1);
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[3], leftSidePoints[1], newTriangles1);
            }
            else
            {
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[3], leftSidePoints[2], newTriangles1);
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[2], leftSidePoints[1], newTriangles1);
            }
        }
  
        if (rightSidePoints.Count == 3)
        {
            /*MeshData.Triangle tri = new MeshData.Triangle()
            { v1 = rightSide[1], v2 = rightSide[0], v3 = rightSide[2] };*/
            meshData.MeshTriangle(normal, rightSidePoints[1], rightSidePoints[0], rightSidePoints[2], newTriangles2);

        }
        else
        {
            if (Vector3.Dot((rightSidePoints[0] - rightSidePoints[1]), rightSidePoints[2] - rightSidePoints[3]) >= 0)
            {
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[2], rightSidePoints[3], newTriangles2);
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[3], rightSidePoints[1], newTriangles2);
            }
            else
            {
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[3], rightSidePoints[2], newTriangles2);
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[2], rightSidePoints[1], newTriangles2);
            }
        }

    }

    void MakeObject(Plane plane, List<Vector3> intersections, Collider original)
    {
        if (intersections.Count > 1)
        {
            Vector3 centre = Vector3.zero;
            foreach (Vector3 vector in intersections)
            {
                centre += vector;
            }
            centre /= intersections.Count;

            for (int i = 0; i < intersections.Count; i++)
            {
                meshData.MeshTriangle(-plane.normal, intersections[i], centre, i + 1 == intersections.Count ? intersections[i] : intersections[i + 1], newTriangles1);
                meshData.MeshTriangle(plane.normal, intersections[i], centre, i + 1 == intersections.Count ? intersections[i] : intersections[i + 1], newTriangles2);
            }
        }

        if (intersections.Count > 0)
        {
            Material material = original.gameObject.GetComponent<MeshRenderer>().material;

            Mesh leftMesh = new Mesh();
            Mesh rightMesh = new Mesh();

            List<Vector3> newTriangles = new List<Vector3>();
            List<int> indices = new List<int>();

            /// potential issue
            Debug.Log(newTriangles1.Count);
            AddVerticies(leftMesh, newTriangles, indices, newTriangles1);
            AddVerticies(rightMesh, newTriangles, indices, newTriangles2);

            GameObject leftGameObject = new GameObject();
            GameObject rightGameObject = new GameObject();

            AddComponents(leftGameObject, material, leftMesh);
            AddComponents(rightGameObject, material, rightMesh);

            GameController.speedChange = GameController.speedChange + 0.25f;
            Instantiate(original);
            ObjectLauncher.newZ = Random.RandomRange(-24, 24);

            GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
  

            arrow.GetComponent<SliceDirection>().transform.rotation = new Quaternion(arrow.transform.rotation.x, 90, arrow.GetComponent<SliceDirection>().rotations[Random.RandomRange(0, arrow.GetComponent<SliceDirection>().rotations.Length)], arrow.transform.rotation.w);
            leftGameObject.GetComponent<Rigidbody>().AddForceAtPosition(leftGameObject.transform.position.normalized * 100f, leftGameObject.transform.position);
            rightGameObject.GetComponent<Rigidbody>().AddForceAtPosition(rightGameObject.transform.position.normalized * 100f, rightGameObject.transform.position);
            Destroy(original.gameObject);
        }
    }
    void AddComponents(GameObject gameObject, Material originalMaterial, Mesh mesh)
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = originalMaterial;

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.sharedMesh = mesh;

        gameObject.AddComponent<Rigidbody>();
    }
    void AddVerticies(Mesh mesh, List<Vector3> triangles, List<int> indicies, List<MeshData.Triangle> triangleList)
    {
        int index = 0;
        foreach (MeshData.Triangle t in triangleList)
        {
            triangles.Add(t.v1);
            triangles.Add(t.v2);
            triangles.Add(t.v3);
            indicies.Add(index++);
            indicies.Add(index++);
            indicies.Add(index++);
        }
        mesh.vertices = triangles.ToArray();
        mesh.triangles = indicies.ToArray();

        triangles.Clear();
        indicies.Clear();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void TriangleIntersection(List<Vector3> points, List<Vector3> intersections, Vector3 intersection)
    {

        intersections.Add(intersection);
        points.Add(intersection);
    }
}
