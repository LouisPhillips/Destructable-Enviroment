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

    private Mesh original;

    private float timer = 1;
    

    public void Cut(Transform intersectedObj)
    {
        side1 = tipExitPoint - tipEntryPoint;
        side2 = tipExitPoint - hiltEntryPoint;

        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        Vector3 transformedNormal = ((Vector3)(intersectedObj.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        Vector3 transformedStartingPoint = intersectedObj.gameObject.transform.InverseTransformPoint(tipEntryPoint);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(transformedNormal, transformedStartingPoint);

        if(transformedNormal.x < 0 || transformedNormal.y < 0)
        {
            plane = plane.flipped;
        }



        DrawPlane(tipEntryPoint, tipExitPoint, hiltEntryPoint, 5, Color.red, 500);


    }

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

    void CreatePlane(Collision other)
    {
        Collider coll = GetComponent<Collider>();

        /*// Create cutting plane
        Vector3 vec1 = coll.bounds.center;
        vec1 += transform.up * coll.bounds.extents.y;
        Vector3 vec2 = coll.bounds.center;
        vec2 += transform.up * coll.bounds.extents.y;
        vec2 += transform.right * coll.bounds.extents.x;
        Vector3 vec3 = coll.bounds.center;
        vec3 += transform.up * -coll.bounds.extents.y;
        vec3 += transform.right * coll.bounds.extents.x;

        Plane pl = new Plane(vec1, vec2, vec3);
        Transform tr = other.transform;
        Mesh m = other.gameObject.GetComponent<MeshFilter>().mesh;
        //int[] triangles = m.triangles;
        Vector3[] verts = m.vertices;

        //List<Vector3> intersections = new List<Vector3>();
        //List<Triangle> newTris1 = new List<Triangle>();
        //List<Triangle> newTris2 = new List<Triangle>();*/

        //
        side1 = tipExitPoint - tipEntryPoint;
        side2 = tipExitPoint - hiltEntryPoint;

        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(tipEntryPoint);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(transformedNormal, transformedStartingPoint);

        if (transformedNormal.x < 0 || transformedNormal.y < 0)
        {
            plane = plane.flipped;
        }

        int[] triangles = other.gameObject.GetComponent<MeshFilter>().mesh.triangles;
        Vector3[] verticies = other.gameObject.GetComponent<MeshFilter>().mesh.vertices;

        List<Vector3> intersections = new List<Vector3>();
        List<Triangle> newTriangles1 = new List<Triangle>();
        List<Triangle> newTriangles2 = new List<Triangle>();

        CheckTriangles(other, plane, normal, triangles, verticies, intersections, newTriangles1, newTriangles2);
    }


    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    void CheckTriangles(Collision collisionObject, Plane plane, Vector3 normal, int[] tris, Vector3[] verts, List<Vector3> intersections, List<Triangle> newTris1, List<Triangle> newTris2)
    {
        for(int i = 0; i< tris.Length; i += 3)
        {
            List<Vector3> points = new List<Vector3>();

            int vert1 = tris[i];
            int vert2 = tris[i + 1];
            int vert3 = tris[i + 2];
            Vector3 point1 = collisionObject.transform.TransformPoint(verts[vert1]);
            Vector3 point2 = collisionObject.transform.TransformPoint(verts[vert2]);
            Vector3 point3 = collisionObject.transform.TransformPoint(verts[vert3]);

            Vector3 direction = point2 - point1;
            float ent;

            Vector3 intersection;

            if (plane.Raycast(new Ray(point1, direction), out ent) && ent <= direction.magnitude)
            {

            }
        }
    }

    public void slice(UnityEngine.Collision other)
    {
        Collider coll = GetComponent<Collider>();

        // Create cutting plane
        Vector3 vec1 = coll.bounds.center;
        vec1 += transform.up * coll.bounds.extents.y;
        Vector3 vec2 = coll.bounds.center;
        vec2 += transform.up * coll.bounds.extents.y;
        vec2 += transform.right * coll.bounds.extents.x;
        Vector3 vec3 = coll.bounds.center;
        vec3 += transform.up * -coll.bounds.extents.y;
        vec3 += transform.right * coll.bounds.extents.x;

        Plane pl = new Plane(vec1, vec2, vec3);
        Transform tr = other.transform;
        Mesh m = other.gameObject.GetComponent<MeshFilter>().mesh;
        int[] triangles = m.triangles;
        Vector3[] verts = m.vertices;

        List<Vector3> intersections = new List<Vector3>();
        List<Triangle> newTris1 = new List<Triangle>();
        List<Triangle> newTris2 = new List<Triangle>();

        /////////////////////////////////////////////////////

        // Loop through tris
        for (int i = 0; i < triangles.Length; i += 3)
        {
            List<Vector3> points = new List<Vector3>();

            int v1 = triangles[i];
            int v2 = triangles[i + 1];
            int v3 = triangles[i + 2];
            Vector3 p1 = tr.TransformPoint(verts[v1]);
            Vector3 p2 = tr.TransformPoint(verts[v2]);
            Vector3 p3 = tr.TransformPoint(verts[v3]);
            Vector3 norm = Vector3.Cross(p1 - p2, p1 - p3);

            Vector3 dir = p2 - p1;
            float ent;

            // Check if tris are intersected
            if (pl.Raycast(new Ray(p1, dir), out ent) && ent <= dir.magnitude)
            {
                Vector3 intersection = p1 + ent * dir.normalized;
                intersections.Add(intersection);
                points.Add(intersection);
            }
            dir = p3 - p2;
            if (pl.Raycast(new Ray(p2, dir), out ent) && ent <= dir.magnitude)
            {
                Vector3 intersection = p2 + ent * dir.normalized;
                intersections.Add(intersection);
                points.Add(intersection);
            }
            dir = p3 - p1;
            if (pl.Raycast(new Ray(p1, dir), out ent) && ent <= dir.magnitude)
            {
                Vector3 intersection = p1 + ent * dir.normalized;
                intersections.Add(intersection);
                points.Add(intersection);
            }

            // Group tris and create new tris
            if (points.Count > 0)
            {
                Debug.Assert(points.Count == 2);
                List<Vector3> points1 = new List<Vector3>();
                List<Vector3> points2 = new List<Vector3>();
                // Intersection verts
                points1.AddRange(points);
                points2.AddRange(points);
                // Check which side the original vert was
                if (pl.GetSide(p1))
                {
                    points1.Add(p1);
                }
                else
                {
                    points2.Add(p1);
                }
                if (pl.GetSide(p2))
                {
                    points1.Add(p2);
                }
                else
                {
                    points2.Add(p2);
                }
                if (pl.GetSide(p3))
                {
                    points1.Add(p3);
                }
                else
                {
                    points2.Add(p3);
                }

                if (points1.Count == 3)
                {
                    Triangle tri = new Triangle() { v1 = points1[1], v2 = points1[0], v3 = points1[2] };
                    tri.matchDirection(norm);
                    newTris1.Add(tri);
                }
                else
                {
                    Debug.Assert(points1.Count == 4);
                    if (Vector3.Dot((points1[0] - points1[1]), points1[2] - points1[3]) >= 0)
                    {
                        Triangle tri = new Triangle() { v1 = points1[0], v2 = points1[2], v3 = points1[3] };
                        tri.matchDirection(norm);
                        newTris1.Add(tri);
                        tri = new Triangle() { v1 = points1[0], v2 = points1[3], v3 = points1[1] };
                        tri.matchDirection(norm);
                        newTris1.Add(tri);
                    }
                    else
                    {
                        Triangle tri = new Triangle() { v1 = points1[0], v2 = points1[3], v3 = points1[2] };
                        tri.matchDirection(norm);
                        newTris1.Add(tri);
                        tri = new Triangle() { v1 = points1[0], v2 = points1[2], v3 = points1[1] };
                        tri.matchDirection(norm);
                        newTris1.Add(tri);
                    }
                }

                if (points2.Count == 3)
                {
                    Triangle tri = new Triangle() { v1 = points2[1], v2 = points2[0], v3 = points2[2] };
                    tri.matchDirection(norm);
                    newTris2.Add(tri);
                }
                else
                {
                    Debug.Assert(points2.Count == 4);
                    if (Vector3.Dot((points2[0] - points2[1]), points2[2] - points2[3]) >= 0)
                    {
                        Triangle tri = new Triangle() { v1 = points2[0], v2 = points2[2], v3 = points2[3] };
                        tri.matchDirection(norm);
                        newTris2.Add(tri);
                        tri = new Triangle() { v1 = points2[0], v2 = points2[3], v3 = points2[1] };
                        tri.matchDirection(norm);
                        newTris2.Add(tri);
                    }
                    else
                    {
                        Triangle tri = new Triangle() { v1 = points2[0], v2 = points2[3], v3 = points2[2] };
                        tri.matchDirection(norm);
                        newTris2.Add(tri);
                        tri = new Triangle() { v1 = points2[0], v2 = points2[2], v3 = points2[1] };
                        tri.matchDirection(norm);
                        newTris2.Add(tri);
                    }
                }
            }
            else
            {
                if (pl.GetSide(p1))
                {
                    newTris1.Add(new Triangle() { v1 = p1, v2 = p2, v3 = p3 });
                }
                else
                {
                    newTris2.Add(new Triangle() { v1 = p1, v2 = p2, v3 = p3 });
                }
            }
        }

        if (intersections.Count > 1)
        {
            // Sets center
            Vector3 center = Vector3.zero;
            foreach (Vector3 vec in intersections)
            {
                center += vec;
            }
            center /= intersections.Count;
            for (int i = 0; i < intersections.Count; i++)
            {
                Triangle tri = new Triangle() { v1 = intersections[i], v2 = center, v3 = i + 1 == intersections.Count ? intersections[i] : intersections[i + 1] };
                tri.matchDirection(-pl.normal);
                newTris1.Add(tri);
            }
            for (int i = 0; i < intersections.Count; i++)
            {
                Triangle tri = new Triangle() { v1 = intersections[i], v2 = center, v3 = i + 1 == intersections.Count ? intersections[i] : intersections[i + 1] };
                tri.matchDirection(pl.normal);
                newTris2.Add(tri);
            }
        }

        if (intersections.Count > 0)
        {
            // Creates new meshes
            Material mat = other.gameObject.GetComponent<MeshRenderer>().material;
            Destroy(other.gameObject);

            Mesh mesh1 = new Mesh();
            Mesh mesh2 = new Mesh();

            List<Vector3> tris = new List<Vector3>();
            List<int> indices = new List<int>();

            int index = 0;
            foreach (Triangle thing in newTris1)
            {
                tris.Add(thing.v1);
                tris.Add(thing.v2);
                tris.Add(thing.v3);
                indices.Add(index++);
                indices.Add(index++);
                indices.Add(index++);
            }
            mesh1.vertices = tris.ToArray();
            mesh1.triangles = indices.ToArray();

            index = 0;
            tris.Clear();
            indices.Clear();
            foreach (Triangle thing in newTris2)
            {
                tris.Add(thing.v1);
                tris.Add(thing.v2);
                tris.Add(thing.v3);
                indices.Add(index++);
                indices.Add(index++);
                indices.Add(index++);
            }
            mesh2.vertices = tris.ToArray();
            mesh2.triangles = indices.ToArray();

            mesh1.RecalculateNormals();
            mesh1.RecalculateBounds();
            mesh2.RecalculateNormals();
            mesh2.RecalculateBounds();

            // Create new objects

            GameObject go1 = new GameObject();
            GameObject go2 = new GameObject();

            MeshFilter mf1 = go1.AddComponent<MeshFilter>();
            mf1.mesh = mesh1;
            MeshRenderer mr1 = go1.AddComponent<MeshRenderer>();
            mr1.material = mat;
            MeshCollider mc1 = go1.AddComponent<MeshCollider>();
            //if (mf1.mesh.vertexCount <= 255) {
            mc1.convex = true;
            go1.AddComponent<Rigidbody>();
            //}
            mc1.sharedMesh = mesh1;
            go1.tag = "Slicable";

            MeshFilter mf2 = go2.AddComponent<MeshFilter>();
            mf2.mesh = mesh2;
            MeshRenderer mr2 = go2.AddComponent<MeshRenderer>();
            mr2.material = mat;
            MeshCollider mc2 = go2.AddComponent<MeshCollider>();
            //if (mf2.mesh.vertexCount <= 255) {
            mc2.convex = true;
            go2.AddComponent<Rigidbody>();
            //}
            mc2.sharedMesh = mesh2;
            go2.tag = "Slicable";
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
