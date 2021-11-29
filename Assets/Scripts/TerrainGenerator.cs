using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int mapSizeX = 10;
    public int mapSizeZ = 10;
    public float amp = 10;
    public float freq = 0.08f;
    public float waterLevel;
    public GameObject player;
    public GameObject waterPrefab;
    //public Material matColour;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles = new int[6];
    private List<GameObject> water = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
        UpdateShape();
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
        vertices = new Vector3[(mapSizeX + 1) * (mapSizeZ + 1)];
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float y = Mathf.PerlinNoise((player.transform.position.x / 10) + x * freq, (player.transform.position.z / 10) + z * freq) * amp;
                vertices[i] = new Vector3(player.transform.position.x - (mapSizeX / 2) + x, y, player.transform.position.z - (mapSizeZ/2) + z);
                water.Add(Instantiate(waterPrefab, new Vector3(player.transform.position.x - (mapSizeX / 2) + x, waterLevel, player.transform.position.z - (mapSizeZ / 2) + z), Quaternion.Euler(90,0,0)));
                i++;
            }
        }
        foreach (GameObject waterSquare in water)
        {
            waterSquare.transform.parent = this.transform;
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
    public void UpdateShape()
    {
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float y = Mathf.PerlinNoise((player.transform.position.x / 10) + x * freq, (player.transform.position.z / 10) + z * freq) * amp;
                vertices[i] = new Vector3(player.transform.position.x - (mapSizeX / 2) + x, y, player.transform.position.z - (mapSizeZ / 2) + z);
                water[i].transform.position = new Vector3(player.transform.position.x - (mapSizeX / 2) + x, waterLevel, player.transform.position.z - (mapSizeZ / 2) + z);
                i++;
            }
        }
        
    }

}
