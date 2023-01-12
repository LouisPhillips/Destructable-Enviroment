using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactoredSplit : MonoBehaviour
{
    public int cuts = 4;
    public float explosionForce = 200;
    public float explosionRadius = 5f;



    void DestroyMesh(UnityEngine.Collision impactObject)
    {
       
        //6
        var originalMesh = GetComponent<MeshFilter>().mesh;
        var parts = new List<PartMesh>();
        var subParts = new List<PartMesh>();

        var mainPart = new PartMesh()
        {
            UV = originalMesh.uv,
            Vertices = originalMesh.vertices,
            Normals = originalMesh.normals,
            Triangles = new int[originalMesh.subMeshCount][],
            Bounds = originalMesh.bounds
        };
        for (int i = 0; i < originalMesh.subMeshCount; i++)
            mainPart.Triangles[i] = originalMesh.GetTriangles(i);

        parts.Add(mainPart);
        for (var c = 0; c < cuts; c++)
        {
            for (var i = 0; i < parts.Count; i++)
            {

                var bounds = parts[i].Bounds;
                bounds.Expand(0.5f);
                var plane = new Plane(Random.onUnitSphere, new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z)));

                /*DrawPlane(new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z))
                    , new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z)),
                    new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z)), 1, Color.red, 100f);*/
                DrawPlaneNearPoint(plane, impactObject.contacts[i].point + new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z)), 1, Color.red, 100);
                
            }
        }
        transform.parent = null;
        Destroy(gameObject);
    }
    public static void DrawPlane(Vector3 a, Vector3 b, Vector3 c, float size,
        Color color, float duration = 0f, bool depthTest = true)
    {

        var plane = new Plane(a, b, c);
        var centroid = (a + b + c);

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

    private PartMesh GenerateMesh(PartMesh originalMesh, Plane plane, bool isOnLeft)
    {
        //5
        var partMesh = new PartMesh() { };
        return partMesh;
    }

    private void AddEdge(int subMesh, PartMesh partMesh, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        //4
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Collidable")
        {
            GetComponent<BoxCollider>().enabled = true;
            DestroyMesh(collision);
        }
    }

    public class PartMesh
    {
        private List<Vector3> _Verticies = new List<Vector3>();
        private List<Vector3> _Normals = new List<Vector3>();
        private List<List<int>> _Triangles = new List<List<int>>();
        private List<Vector2> _UVs = new List<Vector2>();
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public int[][] Triangles;
        public Vector2[] UV;
        public GameObject newGameObject;
        public Bounds Bounds = new Bounds();

        public void AddTriangles(int submesh, Vector3 vert1, Vector3 vert2, Vector3 vert3,
            Vector3 normal1, Vector3 normal2, Vector3 normal3, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            //1
            if (_Triangles.Count - 1 < submesh)
                _Triangles.Add(new List<int>());

            _Triangles[submesh].Add(_Verticies.Count);
            _Verticies.Add(vert1);
            _Triangles[submesh].Add(_Verticies.Count);
            _Verticies.Add(vert2);
            _Triangles[submesh].Add(_Verticies.Count);
            _Verticies.Add(vert3);
            _Normals.Add(normal1);
            _Normals.Add(normal2);
            _Normals.Add(normal3);
            _UVs.Add(uv1);
            _UVs.Add(uv2);
            _UVs.Add(uv3);

            Bounds.min = Vector3.Min(Bounds.min, vert1);
            Bounds.min = Vector3.Min(Bounds.min, vert2);
            Bounds.min = Vector3.Min(Bounds.min, vert3);
            Bounds.max = Vector3.Min(Bounds.max, vert1);
            Bounds.max = Vector3.Min(Bounds.max, vert2);
            Bounds.max = Vector3.Min(Bounds.max, vert3);
        }

        public void FillArrays()
        {
            //2
            Vertices = _Verticies.ToArray();
            Normals = _Normals.ToArray();
            UV = _UVs.ToArray();
            Triangles = new int[_Triangles.Count][];
            for (var i = 0; i < _Triangles.Count; i++)
                Triangles[i] = _Triangles[i].ToArray();
        }

        public void MakeGameObject(RefactoredSplit original)
        {
            //3
            newGameObject = new GameObject(original.name);
            newGameObject.transform.position = original.transform.position;
            newGameObject.transform.rotation = original.transform.rotation;
            newGameObject.transform.localScale = original.transform.localScale;

            var mesh = new Mesh();
            mesh.name = original.GetComponent<MeshFilter>().mesh.name;

            mesh.vertices = Vertices;
            mesh.normals = Normals;
            mesh.uv = UV;
            for (var i = 0; i < Triangles.Length; i++)
                mesh.SetTriangles(Triangles[i], i, true);
            Bounds = mesh.bounds;

            newGameObject.AddComponent<MeshRenderer>();
            newGameObject.GetComponent<MeshRenderer>().material = original.GetComponent<MeshRenderer>().material;

            newGameObject.AddComponent<MeshFilter>();
            newGameObject.GetComponent<MeshFilter>().mesh = mesh;

            newGameObject.AddComponent<MeshCollider>();
            newGameObject.GetComponent<MeshCollider>().convex = true;

            newGameObject.AddComponent<Rigidbody>();

            newGameObject.AddComponent<Despawn>();
        }
    }
}
