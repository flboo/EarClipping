using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolygonTool;

public class testMesh : MonoBehaviour
{

    public Vector3[] vertexs;
    public int[] triangles;


    void Start()
    {
        GameObject go = new GameObject();

        MeshFilter mf = go.AddComponent<MeshFilter>();

        MeshRenderer renderer = go.AddComponent<MeshRenderer>();

        renderer.material.shader = Shader.Find("Sprites/Default");

        Mesh m = new Mesh();

        m.vertices = vertexs;

        m.triangles = triangles;

        mf.mesh = m;
    }


}
