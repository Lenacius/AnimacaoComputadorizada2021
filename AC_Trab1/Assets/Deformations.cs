using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformations : MonoBehaviour
{
    public GameObject baseCube;
    Mesh mesh;
    Vector3[] vertices;
    Vector3[] normals;

    float t = 0;
    float eT = 0;
    public float aSpeed;
    public float bSpeed;
    float bBSpeed;
    bool anim = false;


    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        normals = mesh.normals;
        bBSpeed = bSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim = true;
        }

        if (anim)
        {
            eT += Time.deltaTime;
            t += Time.deltaTime * aSpeed;
            bSpeed += bSpeed * Time.deltaTime;
            ExpandContract();
            if(eT > 1)
                Reset();
        }
    }

    void ExpandContract()
    {
        for (var i = 0; i < vertices.Length; i++)
        {
            vertices[i] += normals[i] * (Mathf.Sin(Time.deltaTime) * Mathf.Tan(Mathf.Sin(t * bSpeed)));
            mesh.vertices = vertices;
        }

    }

    void Reset()
    {
        vertices = baseCube.GetComponent<MeshFilter>().mesh.vertices;
        mesh.vertices = vertices;
        eT = 0;
        t = 0;
        anim = false;
        bSpeed = bBSpeed;
    }
}
