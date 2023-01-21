using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSplit : MonoBehaviour
{
    public Vector3 hiltEntryPoint;
    public Vector3 tipEntryPoint;
    public Vector3 tipExitPoint;

    public Vector3 side1;
    public Vector3 side2;

    public Vector3 pointA;
    public Vector3 pointB;

    private float timer = 1;

    Vector3 normal;
    List<Triangle> newTriangles1;
    List<Triangle> newTriangles2;

    struct Triangle
    {
        public Vector3 v1;
        public Vector3 v2;
        public Vector3 v3;

        public Vector3 getNormal()
        {
            return Vector3.Cross(v1 - v2, v1 - v3).normalized;
        }

        // Conver direction to point in the direction of the tri
        public void matchDirection(Vector3 dir)
        {
            if (Vector3.Dot(getNormal(), dir) > 0)
            {
                return;
            }
            else
            {
                Vector3 vec = v1;
                v1 = v3;
                v3 = vec;
            }
        }
    }

    public void SliceObject(Collider other)
    {
        Collider coll = GetComponent<Collider>();

        Vector3 vec1 = coll.bounds.center;
        vec1 += transform.up * coll.bounds.extents.y;
        Vector3 vec2 = coll.bounds.center;
        vec2 += transform.up * coll.bounds.extents.y;
        vec2 += transform.right * coll.bounds.extents.x;
        Vector3 vec3 = coll.bounds.center;
        vec3 += transform.up * -coll.bounds.extents.y;
        vec3 += transform.right * coll.bounds.extents.x;

/*
        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(tipEntryPoint);
*/
        Plane plane = new Plane(vec1, vec2, vec3);

        /*plane.SetNormalAndPosition(transformedNormal, transformedStartingPoint);

        if (transformedNormal.x < 0 || transformedNormal.y < 0)
        {
            plane = plane.flipped;
        }
*/
        int[] triangles = other.gameObject.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] verticies = other.gameObject.GetComponent<MeshFilter>().mesh.vertices;

        List<Vector3> intersections = new List<Vector3>();
        newTriangles1 = new List<Triangle>();
        newTriangles2 = new List<Triangle>();

        CheckTriangles(other, plane, triangles, verticies, intersections, newTriangles1, newTriangles2);
    }




    void CheckTriangles(Collider collisionObject, Plane plane, int[] tris, Vector3[] verts, List<Vector3> intersections, List<Triangle> newTris1, List<Triangle> newTris2)
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
                    newTriangles1.Add(new Triangle() { v1 = point1, v2 = point2, v3 = point3 });
                }
                else
                {
                    newTriangles2.Add(new Triangle() { v1 = point1, v2 = point2, v3 = point3 });
                }
            }
        }

        MakeObject(plane, intersections, collisionObject);
    }

    void MakeNewTriangles(Plane plane, List<Vector3> points, Vector3 point1, Vector3 point2, Vector3 point3)
    {
        List<Vector3> leftSide = new List<Vector3>();
        List<Vector3> rightSide = new List<Vector3>();

        leftSide.AddRange(points);
        rightSide.AddRange(points);

        if (plane.GetSide(point1))
        {
            leftSide.Add(point1);
        }
        else
        {
            rightSide.Add(point1);
        }

        if (plane.GetSide(point2))
        {
            leftSide.Add(point2);
        }
        else
        {
            rightSide.Add(point2);
        }

        if (plane.GetSide(point3))
        {
            leftSide.Add(point3);
        }
        else
        {
            rightSide.Add(point3);
        }

        if (leftSide.Count == 3)
        {
            Triangle tri = new Triangle()
            { v1 = leftSide[1], v2 = leftSide[0], v3 = leftSide[2] };
            tri.matchDirection(normal);
            newTriangles1.Add(tri);
        }
        else
        {
            if (Vector3.Dot((leftSide[0] - leftSide[1]), leftSide[2] - leftSide[3]) >= 0)
            {
                Triangle tri = new Triangle()
                { v1 = leftSide[0], v2 = leftSide[2], v3 = leftSide[3] };
                tri.matchDirection(normal);
                newTriangles1.Add(tri);
                tri = new Triangle()
                { v1 = leftSide[0], v2 = leftSide[3], v3 = leftSide[1] };
                tri.matchDirection(normal);
                newTriangles1.Add(tri);
            }
            else
            {
                Triangle tri = new Triangle()
                { v1 = leftSide[0], v2 = leftSide[3], v3 = leftSide[2] };
                tri.matchDirection(normal);
                newTriangles1.Add(tri);
                tri = new Triangle()
                { v1 = leftSide[0], v2 = leftSide[2], v3 = leftSide[1] };
                tri.matchDirection(normal);
                newTriangles1.Add(tri);
            }
        }

        if (rightSide.Count == 3)
        {
            Triangle tri = new Triangle()
            { v1 = rightSide[1], v2 = rightSide[0], v3 = rightSide[2] };
        }
        else
        {
            if (Vector3.Dot((rightSide[0] - rightSide[1]), rightSide[2] - rightSide[3]) >= 0)
            {
                Triangle tri = new Triangle()
                { v1 = rightSide[0], v2 = rightSide[2], v3 = rightSide[3] };
                tri.matchDirection(normal);
                newTriangles2.Add(tri);
                tri = new Triangle()
                { v1 = rightSide[0], v2 = rightSide[3], v3 = rightSide[1] };
                tri.matchDirection(normal);
                newTriangles2.Add(tri);
            }
            else
            {
                Triangle tri = new Triangle()
                { v1 = rightSide[0], v2 = rightSide[3], v3 = rightSide[2] };
                tri.matchDirection(normal);
                newTriangles2.Add(tri);
                tri = new Triangle()
                { v1 = rightSide[0], v2 = rightSide[2], v3 = rightSide[1] };
                tri.matchDirection(normal);
                newTriangles2.Add(tri);
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
                Triangle tri = new Triangle()
                { v1 = intersections[i], v2 = centre, v3 = i + 1 == intersections.Count ? intersections[i] : intersections[i + 1] };
                tri.matchDirection(-plane.normal);
                newTriangles1.Add(tri);

                /// potential issue

                Triangle tri2 = new Triangle()
                { v1 = intersections[i], v2 = centre, v3 = i + 1 == intersections.Count ? intersections[i] : intersections[i + 1] };
                tri2.matchDirection(plane.normal);
                newTriangles2.Add(tri2);
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
            AddVerticies(leftMesh, newTriangles, indices, newTriangles1);
            newTriangles.Clear();
            indices.Clear();
            AddVerticies(rightMesh, newTriangles, indices, newTriangles2);

            leftMesh.RecalculateBounds();
            leftMesh.RecalculateNormals();

            rightMesh.RecalculateBounds();
            rightMesh.RecalculateNormals();

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
            Debug.Log("i got here :)");
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
    void AddVerticies(Mesh mesh, List<Vector3> triangles, List<int> indicies, List<Triangle> triangleList)
    {
        int index = 0;
        foreach (Triangle t in triangleList)
        {
            triangles.Add(t.v1);
            triangles.Add(t.v2);
            triangles.Add(t.v3);
            indicies.Add(index++);
            indicies.Add(index++);
            indicies.Add(index++);
        }
        Debug.Log(triangles.Count);
        mesh.vertices = triangles.ToArray();
        mesh.triangles = indicies.ToArray();

        triangles.Clear();
        indicies.Clear();
    }

    void TriangleIntersection(List<Vector3> points, List<Vector3> intersections, Vector3 intersection)
    {
        intersections.Add(intersection);
        points.Add(intersection);
    }


    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
    }



    public static void DrawPlane(Vector3 a, Vector3 b, Vector3 c, float size,
        Color color, float duration = 0f, bool depthTest = true)
    {

        var plane = new Plane(a, b, c);
        var centroid = (a + b + c) / 3;

        DrawPlaneAtPoint(plane, centroid, size, color, duration, depthTest);
    }
    public static void DrawPlaneNearPoint(Plane plane, Vector3 point, float size, Color color, float duration = 0f, bool depthTest = true)
    {
        var closest = plane.ClosestPointOnPlane(point);
        Color side = plane.GetSide(point) ? Color.cyan : Color.red;
        Debug.DrawLine(point, closest, side, duration, depthTest);

        DrawPlaneAtPoint(plane, closest, size, color, duration, depthTest);
    }

    // Non-public method to do the heavy lifting of drawing the grid of a given plane segment.
    static void DrawPlaneAtPoint(Plane plane, Vector3 center, float size, Color color, float duration, bool depthTest)
    {
        var basis = Quaternion.LookRotation(plane.normal);
        var scale = Vector3.one * size / 10f;

        var right = Vector3.Scale(basis * Vector3.right, scale);
        var up = Vector3.Scale(basis * Vector3.up, scale);

        for (int i = -5; i <= 5; i++)
        {
            Debug.DrawLine(center + right * i - up * 5, center + right * i + up * 5, color, duration, depthTest);
            Debug.DrawLine(center + up * i - right * 5, center + up * i + right * 5, color, duration, depthTest);
        }
    }
}
