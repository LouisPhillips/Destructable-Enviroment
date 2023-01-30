using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSplit : MonoBehaviour
{
    private MeshData meshData;

    List<MeshData.Triangle> leftTriangles;
    List<MeshData.Triangle> rightTriangles;

    public void SliceObject(Collider other)
    {
        Plane plane = CreatePlane();

        meshData = new MeshData();

        meshData.triangles = other.gameObject.GetComponent<MeshFilter>().mesh.triangles;
        meshData.verticies = other.gameObject.GetComponent<MeshFilter>().mesh.vertices;
        meshData.uvs = other.gameObject.GetComponent<MeshFilter>().mesh.uv;

        List<Vector3> intersections = new List<Vector3>();
        leftTriangles = new List<MeshData.Triangle>();
        rightTriangles = new List<MeshData.Triangle>();

        GetVertices(other, plane, meshData.uvs, meshData.triangles, meshData.verticies, intersections, leftTriangles, rightTriangles);
    }

    private Plane CreatePlane()
    {
        Collider collider = GetComponent<Collider>();
        Vector3 centreBounds = collider.bounds.center;
        Vector3 extentsBounds = collider.bounds.extents;
        Vector3[] vectors = new Vector3[] { centreBounds + transform.up * extentsBounds.y, centreBounds + transform.up * extentsBounds.y + transform.right * centreBounds.x, centreBounds + transform.up * -extentsBounds.y + transform.right * extentsBounds.x };
        return new Plane(vectors[0], vectors[1], vectors[2]);
    }

    void GetVertices(Collider collisionObject, Plane plane, Vector2[] uvs, int[] tris, Vector3[] verts, List<Vector3> intersections, List<MeshData.Triangle> newTris1, List<MeshData.Triangle> newTris2)
    {

        for (int i = 0; i < tris.Length; i += 3)
        {
            List<Vector3> points = new List<Vector3>();
            int[] verticies = new int[] { tris[i], tris[i + 1], tris[i + 2] };
            Vector3[] collisionPoints = new Vector3[] { collisionObject.transform.TransformPoint(verts[verticies[0]]),
            collisionObject.transform.TransformPoint(verts[verticies[1]]),
            collisionObject.transform.TransformPoint(verts[verticies[2]])};

            Vector3 normal = Vector3.Cross(collisionPoints[0] - collisionPoints[1], collisionPoints[0] - collisionPoints[2]);

            Vector3 direction = collisionPoints[1] - collisionPoints[0];
            float length;

            Vector3 intersection;
            if (plane.Raycast(new Ray(collisionPoints[0], direction), out length) && length <= direction.magnitude)
            {
                intersection = collisionPoints[0] + length * direction.normalized;
                TriangleIntersection(points, intersections, intersection);
            }
            direction = collisionPoints[2] - collisionPoints[1];

            if (plane.Raycast(new Ray(collisionPoints[1], direction), out length) && length <= direction.magnitude)
            {
                intersection = collisionPoints[1] + length * direction.normalized;
                TriangleIntersection(points, intersections, intersection);
            }
            direction = collisionPoints[2] - collisionPoints[0];

            if (plane.Raycast(new Ray(collisionPoints[0], direction), out length) && length <= direction.magnitude)
            {
                intersection = collisionPoints[0] + length * direction.normalized;
                TriangleIntersection(points, intersections, intersection);
            }

            if (points.Count > 0)
            {
                // Make new triangles based off entry points
                MakeNewTriangles(plane, points, collisionPoints, normal);
            }
            else
            {
                if (plane.GetSide(collisionPoints[0]))
                {
                    leftTriangles.Add(new MeshData.Triangle() { v1 = collisionPoints[0], v2 = collisionPoints[1], v3 = collisionPoints[2] });
                }
                else
                {
                    rightTriangles.Add(new MeshData.Triangle() { v1 = collisionPoints[0], v2 = collisionPoints[1], v3 = collisionPoints[2] });
                }
            }
        }

        MakeObject(plane, intersections, collisionObject);
    }

    void MakeNewTriangles(Plane plane, List<Vector3> points, Vector3[] collisionPoints, Vector3 normal)
    {
        List<Vector3> leftSidePoints = new List<Vector3>();
        List<Vector3> rightSidePoints = new List<Vector3>();

        leftSidePoints.AddRange(points);
        rightSidePoints.AddRange(points);

        if (plane.GetSide(collisionPoints[0]))
        {
            leftSidePoints.Add(collisionPoints[0]);
        }
        else
        {
            rightSidePoints.Add(collisionPoints[0]);
        }

        if (plane.GetSide(collisionPoints[1]))
        {
            leftSidePoints.Add(collisionPoints[1]);
        }
        else
        {
            rightSidePoints.Add(collisionPoints[1]);
        }

        if (plane.GetSide(collisionPoints[2]))
        {
            leftSidePoints.Add(collisionPoints[2]);
        }
        else
        {
            rightSidePoints.Add(collisionPoints[2]);
        }

        if (leftSidePoints.Count == 3)
        {
            meshData.MeshTriangle(normal, leftSidePoints[1], leftSidePoints[0], leftSidePoints[2], leftTriangles);
        }
        else
        {
            if (Vector3.Dot((leftSidePoints[0] - leftSidePoints[1]), leftSidePoints[2] - leftSidePoints[3]) >= 0)
            {
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[2], leftSidePoints[3], leftTriangles);
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[3], leftSidePoints[1], leftTriangles);
            }
            else
            {
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[3], leftSidePoints[2], leftTriangles);
                meshData.MeshTriangle(normal, leftSidePoints[0], leftSidePoints[2], leftSidePoints[1], leftTriangles);
            }
        }

        if (rightSidePoints.Count == 3)
        {
            meshData.MeshTriangle(normal, rightSidePoints[1], rightSidePoints[0], rightSidePoints[2], rightTriangles);
        }
        else
        {
            if (Vector3.Dot((rightSidePoints[0] - rightSidePoints[1]), rightSidePoints[2] - rightSidePoints[3]) >= 0)
            {
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[2], rightSidePoints[3], rightTriangles);
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[3], rightSidePoints[1], rightTriangles);
            }
            else
            {
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[3], rightSidePoints[2], rightTriangles);
                meshData.MeshTriangle(normal, rightSidePoints[0], rightSidePoints[2], rightSidePoints[1], rightTriangles);
            }
        }

    }

    void MakeObject(Plane plane, List<Vector3> intersections, Collider original)
    {
        List<MeshData.Triangle> leftInside = new List<MeshData.Triangle>();
        List<MeshData.Triangle> rightInside = new List<MeshData.Triangle>();

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
                // makes inside triangles
                meshData.MeshTriangle(-plane.normal, intersections[i], centre, i + 1 == intersections.Count ? intersections[i] : intersections[i + 1], leftInside);
                meshData.MeshTriangle(plane.normal, intersections[i], centre, i + 1 == intersections.Count ? intersections[i] : intersections[i + 1], rightInside);

                // Add these triangles to seperate list
                // Assign those meshes a different material

                // solutuions: inner material component which has its own seperate matieral
            }
        }

        if (intersections.Count > 0)
        {
            Material material = original.gameObject.GetComponent<MeshRenderer>().material;
            Material insideMat = GetComponent<InsideMaterial>().insideMaterial;

            Mesh leftMesh = new Mesh();
            Mesh rightMesh = new Mesh();

            Mesh rightInsideMesh = new Mesh();
            Mesh leftInsideMesh = new Mesh();

            
            List<Vector3> newTriangles = new List<Vector3>();
            List<int> indices = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            AddVerticies(leftMesh, newTriangles, indices, leftTriangles, uvs);
            AddVerticies(rightMesh, newTriangles, indices, rightTriangles, uvs);

            AddVerticies(rightInsideMesh, newTriangles, indices, rightInside, uvs);
            AddVerticies(leftInsideMesh, newTriangles, indices, leftInside, uvs);

            GameObject leftGameObject = new GameObject();
            GameObject rightGameObject = new GameObject();

            GameObject leftGameObjectIn = new GameObject();
            GameObject rightGameObjectIn = new GameObject();

            AddComponents(leftGameObject, material, insideMat, leftMesh, leftInsideMesh);
            AddComponents(rightGameObject, material, insideMat, rightMesh, rightInsideMesh);

            AddComponents(leftGameObjectIn, insideMat, insideMat, leftInsideMesh, leftInsideMesh);
            AddComponents(rightGameObjectIn, insideMat, insideMat, rightInsideMesh, rightInsideMesh);

            GameController.speedChange = GameController.speedChange + 0.25f;
            Instantiate(original);
            ObjectLauncher.newZ = Random.RandomRange(-24, 24);

            GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
            leftGameObject.name = "leftGameObject";
            rightGameObject.name = "rightGameObject";

            arrow.GetComponent<SliceDirection>().transform.rotation = new Quaternion(arrow.transform.rotation.x, 90, arrow.GetComponent<SliceDirection>().rotations[Random.RandomRange(0, arrow.GetComponent<SliceDirection>().rotations.Length)], arrow.transform.rotation.w);
            leftGameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.position.normalized * 100f, transform.position);
            rightGameObject.GetComponent<Rigidbody>().AddForceAtPosition(transform.position.normalized * 100f, transform.position);
            leftGameObjectIn.GetComponent<Rigidbody>().AddForceAtPosition(transform.position.normalized * 100f, transform.position);
            rightGameObjectIn.GetComponent<Rigidbody>().AddForceAtPosition(transform.position.normalized * 100f, transform.position);

            leftGameObjectIn.AddComponent<PositionInside>();
            rightGameObjectIn.AddComponent<PositionInside>();

            leftGameObjectIn.transform.parent = leftGameObject.transform;
            rightGameObjectIn.transform.parent = rightGameObject.transform;

            Destroy(original.gameObject);
        }
    }

    void AddComponents(GameObject gameObject, Material originalMaterial, Material insideMaterial, Mesh mesh, Mesh insideMesh)
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        meshFilter.mesh = mesh;
        //meshFilter.mesh = insideMesh;
        //GetComponent<InsideMaterial>().insideMaterial;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        /*meshRenderer.materials[0] = originalMaterial;
        meshRenderer.materials[1] = insideMaterial;*/
        meshRenderer.material = originalMaterial;
        //meshRenderer.material = insideMaterial;

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.convex = true;
        meshCollider.sharedMesh = mesh;

        gameObject.AddComponent<Despawn>();

        gameObject.AddComponent<Rigidbody>();
    }

    void AddVerticies(Mesh mesh, List<Vector3> triangles, List<int> indicies, List<MeshData.Triangle> triangleList, List<Vector2> uvs)
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
            /*uvs.Add(new Vector2(1 + Mathf.Sin(t.v1.x * (2 * Mathf.PI / triangleList.Count)), 1 + Mathf.Cos(t.v1.z * (2 * Mathf.PI / triangleList.Count))) * 0.5f);
            uvs.Add(new Vector2(1 + Mathf.Sin(t.v2.x * (2 * Mathf.PI / triangleList.Count)), 1 + Mathf.Cos(t.v2.z * (2 * Mathf.PI / triangleList.Count))) * 0.5f);
            uvs.Add(new Vector2(1 + Mathf.Sin(t.v3.x * (2 * Mathf.PI / triangleList.Count)), 1 + Mathf.Cos(t.v3.z * (2 * Mathf.PI / triangleList.Count))) * 0.5f);*/

            uvs.Add(new Vector2(t.v1.x, t.v1.z));
            uvs.Add(new Vector2(t.v2.x, t.v2.z));
            uvs.Add(new Vector2(t.v3.x, t.v3.z));
        }
        mesh.vertices = triangles.ToArray();
        mesh.triangles = indicies.ToArray();
        mesh.uv = uvs.ToArray();

        triangles.Clear();
        indicies.Clear();
        uvs.Clear();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

    }


    void TriangleIntersection(List<Vector3> points, List<Vector3> intersections, Vector3 intersection)
    {
        intersections.Add(intersection);
        points.Add(intersection);
    }
}
