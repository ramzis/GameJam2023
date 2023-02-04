using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayQuad : MonoBehaviour
{
    Mesh mesh;
    public MeshFilter meshFilter;
    Vector3[] vertices;

    Vector3[] originalV;
    void Start()
    {
        mesh = meshFilter.mesh;
        vertices = mesh.vertices;
        originalV = mesh.vertices;
        //for (var i = 0; i < vertices.Length; i++)
        //{
        //    Debug.Log(vertices[i]);
        //   // vertices[i] += Vector3.up * Time.deltaTime;
        //}
    }


    void Update()
    {
        for (var i = 2; i < vertices.Length; i++)
        {

            vertices[i] = new Vector3(originalV[i].x +Mathf.Sin(Time.time)*0.05f, originalV[i].y + Mathf.Cos(Time.time) * 0.05f, originalV[i].z);
        }

        //// assign the local vertices array into the vertices array of the Mesh.
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }


    public void moveShake()
    {

    }
}
