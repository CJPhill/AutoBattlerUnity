using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }
    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
    }
}



[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent (typeof(MeshCollider))]

public class HexRenderer : MonoBehaviour
{
    private Mesh m_mesh;
    private MeshFilter m_filter;
    public MeshRenderer m_renderer;
    public MeshCollider m_collider;

    private List<Face> m_faces;

    public Material idleMaterial;
    public Material selectedMaterial;

    public float innerSize;
    public float outerSize;
    public float height;
    public bool isFlatTopped;
    [SerializeField]
    private bool isSelected = false;
    private int hexGridLayer;

    private void Awake()
    {
        m_filter = GetComponent<MeshFilter>();
        m_renderer = GetComponent<MeshRenderer>();
        m_collider = GetComponent<MeshCollider>();

        m_mesh = new Mesh();
        m_mesh.name = "Hex";

        m_filter.mesh = m_mesh;
        m_renderer.material = idleMaterial;

        m_collider.sharedMesh = m_mesh;
        m_collider.convex = true;

        hexGridLayer = LayerMask.NameToLayer("HexGrid");
        gameObject.layer = hexGridLayer;




    }

    private void Update()
    {
        DrawMesh();
        if (isSelected) { m_renderer.material = selectedMaterial; }
        else { m_renderer.material = idleMaterial;}
    }


    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }
    
    private void DrawFaces()
    {
        m_faces = new List<Face>();

        //Top faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
        }
        //Bottom Faces
        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
        }
        //outer faces
        for (int point = 0;point < 6; point++)
        {
            m_faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
        }
        //Inner faces
        for (int point = 0;point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point, false));
        }
    }

    private void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < m_faces.Count; i++)
        {
            //Add the vertices
            vertices.AddRange(m_faces[i].vertices);
            uvs.AddRange(m_faces[i].uvs);

            //Offset the triangles
            int offset = (4 * i);
            foreach (int triangle in m_faces[i].triangles)
            {
                tris.Add(triangle + offset);
            }
        }

        m_mesh.vertices = vertices.ToArray();
        m_mesh.triangles = tris.ToArray();
        m_mesh.uv = uvs.ToArray();
        m_mesh.RecalculateNormals();
    }  

    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        if (reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs);
    }

    protected Vector3 GetPoint(float size, float height, int index)
    {
        float angle_deg = isFlatTopped ? 60 * index: 60*index-30;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector3((size * Mathf.Cos(angle_rad)), height, size * Mathf.Sin(angle_rad));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Unit"))
        {
            isSelected = true;
            Debug.Log("woah");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Unit"))
        {
            isSelected = false;
        }
    }
}
