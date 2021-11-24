using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int mapSizeX = 10;
    public int mapSizeZ = 10;
    //public Material matColour;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles = new int[6];
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        //GetComponent<MeshRenderer>().material = matColour;

        CreateShape();
        UpdateMesh();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        //GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
    }

    public void CreateShape()
    {
        vertices = new Vector3[(mapSizeX * mapSizeZ)];
        //vertices = new Vector3[]
        //{
        //    new Vector3 (0,0,0),
        //    new Vector3 (0,0,1),
        //    new Vector3 (1,0,0),
        //    new Vector3 (1,0,1)
        //};

        //triangles = new int[] {
        //    0,1,2,1,3,2
        //};
        
        for (int i = 0, z = 0; z < mapSizeZ; z++)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                vertices[i] = new Vector3(z, 0, x);
                //OnDrawGizmos(i);
                i++;
               

                triangles = new int[] {
                    0,1,2,1,3,2
                };
            }
        }

    }

    //private void OnDrawGizmos(int i)
    //{
    //    Gizmos.DrawSphere(vertices[i], .1f);
    //}
}
