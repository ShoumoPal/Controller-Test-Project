using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDestroyTest : MonoBehaviour
{
    Mesh mesh;

    [SerializeField] GameObject splinters;

    private void Awake()
    {
        mesh = Instantiate(GetComponent<MeshFilter>().mesh);
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void DestroyMesh(int triangleIndex, int amount)
    {
        List<int> triangles = new();
        triangles.AddRange(mesh.triangles);

        Destroy(GetComponent<MeshCollider>());

        int startIndex = triangleIndex * 3;

        if(startIndex + amount - 1 < triangles.Count)
        {
            triangles.RemoveRange(startIndex, amount);
            mesh.triangles = triangles.ToArray();
        }
        gameObject.AddComponent<MeshCollider>();
    }

    public void CutCircularHole(Vector3 center, float radius)
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        List<int> newTriangles = new();

        Transform meshTransform = transform;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = meshTransform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = meshTransform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = meshTransform.TransformPoint(vertices[triangles[i + 2]]);

            bool v0Inside = Vector3.Distance(v0, center) < radius;
            bool v1Inside = Vector3.Distance(v1, center) < radius;
            bool v2Inside = Vector3.Distance(v2, center) < radius;

            // Keep the triangle only if NONE of its vertices are inside the hole
            if (!(v0Inside || v1Inside || v2Inside))
            {
                newTriangles.Add(triangles[i]);
                newTriangles.Add(triangles[i + 1]);
                newTriangles.Add(triangles[i + 2]);
            }
        }

        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = null;
        meshFilter.mesh = mesh;

        // Refresh MeshCollider if present
        if (TryGetComponent(out MeshCollider col))
            col.sharedMesh = mesh;

        Instantiate(splinters, center, Quaternion.identity);
    }
}
