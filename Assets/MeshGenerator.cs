using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{

    Mesh mesh;
    Vector3[] verts;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    NoiseSampler sampler;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        sampler = new PerlinNoiseSampler();

        CreateShape();
        UpdateMesh();
    }

    void CreateShape()
    {
        //Create array to hold all of the vertices
        verts = new Vector3[(xSize + 1) * (zSize + 1)];

        //loop through verts array to add vertex coords
        for (int index = 0, z = 0; z<=zSize; z++)
        {
            for(int x = 0; x <= xSize; x++)
            {
                verts[index] = new Vector3(x, sampler.getYValue(x,z), z);
                index++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;
        for(int z = 0; z<zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
        
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verts;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}

//idk how imports/exports/namespaces work in c# so I'm just adding this to the bottom here
public abstract class NoiseSampler
{
    public abstract float getYValue(int x, int z);
}

public class PerlinNoiseSampler : NoiseSampler
{
    public override float getYValue(int x, int z)
    {
        return Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
    }
}