using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int mapSizeX = 10;
    public int mapSizeZ = 10;
    public float amp = 10;
    public float freq = 0.08f;
    public GameObject player;
    //public Material matColour;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles = new int[6];
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
        CreateShape();
        UpdateMesh();
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
        vertices = new Vector3[(mapSizeX + 1) * (mapSizeZ + 1)];
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float y = Mathf.PerlinNoise(player.transform.position.x + x * freq, player.transform.position.z + z * freq) * amp;
                vertices[i] = new Vector3(player.transform.position.x - (mapSizeZ / 2) + x, y, player.transform.position.z - (mapSizeZ/2) + z);
                //OnDrawGizmos(i);
                i++;
            }
        }

        triangles = new int[mapSizeX * mapSizeZ * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < mapSizeZ; z++)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + mapSizeX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + mapSizeX + 1;
                triangles[tris + 5] = vert + mapSizeX + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    //private void OnDrawGizmos(int i)
    //{
    //    Gizmos.DrawSphere(vertices[i], .1f);
    //}
}
