using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceMeshGenerator : MonoBehaviour
{
    public Material material;
    public float pointSpacing;
    public float generationSpeed;
    public float lineWidth = 0.5f;
    public float lineDepth = 5f;
    public bool useLineWidth = true;
    public Vector2 generationOffset;
    public ListenerComponent mic;
    List<Vector2> topVertices;

    float distanceToNewPoint;
    float horizontalLength;
    MeshRenderer meshRenderer;
    PolygonCollider2D collider;
    MeshFilter meshFilter;
    Mesh mesh;
    bool isGenerating = false;

    // Start is called before the first frame update
    void Start()
    {

        collider = GetComponent<PolygonCollider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();

        topVertices = new List<Vector2>();
    }

    public void StartGenerating()
    {
        topVertices.Add(new Vector2(generationOffset.x, mic.pitchHeight));
        topVertices.Add(new Vector2(generationOffset.x, mic.pitchHeight));
        isGenerating = true;
    }

    public void StopGenerating()
    {
        isGenerating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGenerating)
        {
            horizontalLength += generationSpeed * Time.deltaTime;
            distanceToNewPoint -= generationSpeed * Time.deltaTime;

            topVertices[topVertices.Count - 1] = new Vector2(generationOffset.x + horizontalLength, mic.pitchHeight);

            if (distanceToNewPoint < 0f)
            {
                distanceToNewPoint += pointSpacing;
                topVertices.Add(new Vector2(generationOffset.x + horizontalLength, mic.pitchHeight));
            }

            UpdateMesh();

        }
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[topVertices.Count * 2];
        Vector3[] normals = new Vector3[topVertices.Count * 2];
        Vector2[] uvs = new Vector2[topVertices.Count * 2];
        for (int i = 0; i < topVertices.Count; i++)
        {
            vertices[i] = new Vector3(topVertices[i].x, topVertices[i].y, 0f);
            normals[i] = new Vector3(0f, 0f, -1f);
            uvs[i] = new Vector2(i % 2f, 0f);
        }
        if(useLineWidth)
        {
            for (int i = topVertices.Count; i < (topVertices.Count * 2); i++)
            {
                vertices[i] = new Vector3(topVertices[i - topVertices.Count].x, topVertices[i - topVertices.Count].y, 0f);
                vertices[i].y -= lineWidth;
                normals[i] = new Vector3(0f, 0f, 1f);
                uvs[i] = new Vector2(i % 2f, 1f);
            }
        }
        else
        {
            for (int i = topVertices.Count; i < (topVertices.Count * 2); i++)
            {
                vertices[i] = new Vector3(topVertices[i - topVertices.Count].x, topVertices[i - topVertices.Count].y, 0f);
                vertices[i].y = -lineDepth;
                normals[i] = new Vector3(0f, 0f, 1f);
                uvs[i] = new Vector2(i % 2f, 1f);
            }
        }


        int[] tris = new int[(topVertices.Count - 1) * 6];
        for (int i = 0; i < (topVertices.Count - 1) * 6; i += 6)
        {
            tris[i] = i / 6;
            tris[i + 1] = i / 6 + topVertices.Count + 1;
            tris[i + 2] = i / 6 + topVertices.Count;

            tris[i + 3] = i / 6;
            tris[i + 4] = i / 6 + 1;
            tris[i + 5] = i / 6 + topVertices.Count + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.normals = normals;
        mesh.uv = uvs;
        meshFilter.mesh = mesh;

        Vector2[] collisionPath = new Vector2[vertices.Length];
        for(int i = 0; i < vertices.Length; i++)
        {
            int n = i;
            if (i > topVertices.Count - 1)
            {
                n = vertices.Length - (i - topVertices.Count + 1);
            }
            collisionPath[i] = new Vector2(vertices[n].x, vertices[n].y);
        }
        collider.SetPath(0, collisionPath);
    }

    public void ResetShape()
    {
        mesh.Clear();
        topVertices.Clear();
        horizontalLength = 0f;
        distanceToNewPoint = pointSpacing;
    }
}
