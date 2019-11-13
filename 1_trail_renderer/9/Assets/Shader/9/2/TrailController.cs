using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TrailController : MonoBehaviour
{
    const int FRAME_MAX = 10;
    List<Vector3> point0 = new List<Vector3>();
    List<Vector3> point1 = new List<Vector3>();
    Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Time.timeScale = 0;

        if (FRAME_MAX <= point0.Count)
        {
            point0.RemoveAt(0);
            point1.RemoveAt(0);
        }

        point0.Add(transform.position);
        point1.Add(transform.TransformPoint(new Vector3(0, 1, 0)));

        if (2 <= point0.Count)
        {
            CreateMesh();
        }
    }

    void CreateMesh()
    {
        mesh.Clear();

        int n = point0.Count;
        Vector3[] vertexArray = new Vector3[2 * n];
        Vector2[] uvArray = new Vector2[2 * n];
        int[] indexArray = new int[6 * (n - 1)];

        int idv = 0, idI = 0;
        float dUv = 1 / ((float)n - 1);
        for (int i = 0; i < n; i++)
        {
            vertexArray[idv + 0] = transform.InverseTransformPoint(point0[i]);
            vertexArray[idv + 1] = transform.InverseTransformPoint(point1[i]);
            
            uvArray[idv + 0].x =
            uvArray[idv + 1].x = 1 - dUv * (float)i;
            uvArray[idv + 0].x = 0;
            uvArray[idv + 1].x = 1;

            if (i == n - 1) break;
            indexArray[idI + 0] = idv;
            indexArray[idI + 1] = idv + 1;
            indexArray[idI + 2] = idv + 2;
            indexArray[idI + 3] = idv + 2;
            indexArray[idI + 4] = idv + 1;
            indexArray[idI + 5] = idv + 3;

            idv += 2;
            idI += 6;
        }

        mesh.vertices = vertexArray;
        mesh.uv = uvArray;
        mesh.triangles = indexArray;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
