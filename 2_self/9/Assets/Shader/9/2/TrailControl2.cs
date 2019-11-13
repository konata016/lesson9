using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailControl2 : MonoBehaviour
{
    const int FRAME_MAX = 100;
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
            CreateMesh(6);
        }
    }

    void CreateMesh(int s)
    {
        mesh.Clear();

        int n = point0.Count;
        Vector3[] vertexArray = new Vector3[2 * s * (n - 1) + 2];
        Vector2[] uvArray = new Vector2[2 * s * (n - 1) + 2];
        int[] indexArray = new int[6 * s * (n - 1)];

        int idv = 0, idI = 0;
        float dUv = 1 / ((float)s * (float)(n - 1));
        int id0 = 0, id1 = 0, id2 = 1, id3 = 2;
        if (n - 1 < id3) id3 = n - 1;

        for (int i = 0; i < n - 1; i++)
        {
            for (int f = 0; f < s; f++)
            {
                float t = (float)f / s;
                Vector3 p0 = CatmullRom(point0[id0], point0[id1], point0[id2], point0[id3], t);
                Vector3 p1 = CatmullRom(point1[id0], point1[id1], point1[id2], point1[id3], t);


                vertexArray[idv + 0] = transform.InverseTransformPoint(p0);
                vertexArray[idv + 1] = transform.InverseTransformPoint(p1);

                uvArray[idv + 0].x =
                uvArray[idv + 1].x = 1 - dUv * (float)(i * s + f);
                uvArray[idv + 0].y = 0;
                uvArray[idv + 1].y = 1;

                //if (i == n - 1) break;
                indexArray[idI + 0] = idv;
                indexArray[idI + 1] = idv + 1;
                indexArray[idI + 2] = idv + 2;
                indexArray[idI + 3] = idv + 2;
                indexArray[idI + 4] = idv + 1;
                indexArray[idI + 5] = idv + 3;

                idv += 2;
                idI += 6;
            }

            if (i != 0) id0++;
            if (n - 1 < ++id1) id1 = n - 1;
            if (n - 1 < ++id2) id2 = n - 1;
            if (n - 1 < ++id3) id3 = n - 1;
        }

        vertexArray[idv + 0] = transform.InverseTransformPoint(point0[n-1]);
        vertexArray[idv + 1] = transform.InverseTransformPoint(point1[n-1]);

        uvArray[idv + 0].x = uvArray[idv + 1].x = 0;
        uvArray[idv + 0].y = 0;
        uvArray[idv + 1].y = 1;

        mesh.vertices = vertexArray;
        mesh.uv = uvArray;
        mesh.triangles = indexArray;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    Vector3 CatmullRom(Vector3 p0,Vector3 p1,Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t * t2;
        float c0 = -0.5f * t3 + t2 - 0.5f * t;
        float c1 = 1.5f * t3 - 2.5f * t2 + 1;
        float c2 = -1.5f * t3 + 2.0f * t2 + 0.5f * t;
        float c3 = 0.5f * t3 - 0.5f * t2;

        return p0 * c0 + p1 * c1 + p2 * c2 + p3 * c3;

    }
}
