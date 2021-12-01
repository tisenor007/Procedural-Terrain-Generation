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
    public bool smoothGen = false;
    //public Material matColour;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private int[] triangles = new int[6];
    private List<GameObject> water = new List<GameObject>();
    private Vector3 middlePosition;
    private float timeGenSpeed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        //timeGenSpeed = 10;
        CreateTerrain();
        UpdateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(amp / (freq * 10));
        GetComponent<MeshCollider>().sharedMesh = mesh;
        if (smoothGen == false)
        {
            if (player.transform.position.x >= middlePosition.x + mapSizeX / 4 && player.transform.position.x > middlePosition.x || player.transform.position.x <= middlePosition.x - mapSizeX / 4 && player.transform.position.x < middlePosition.x || player.transform.position.z >= middlePosition.z + mapSizeZ / 4 && player.transform.position.z > middlePosition.z || player.transform.position.z <= middlePosition.z - mapSizeZ / 4 && player.transform.position.z < middlePosition.z)
            {
                UpdateTerrain();
            }
        }
        else if (smoothGen == true)
        {
            UpdateTerrain();
        }

    }
    public float ReturnPerlinNoise(int x, int z)
    {
        //time-gen math is for chunk gen and so the terrain doesn't generate too fast or make player appear faster than should
        return Mathf.PerlinNoise((player.transform.position.x / (timeGenSpeed / (freq*10))) + x * freq, (player.transform.position.z / (timeGenSpeed / (freq*10))) + z * freq) * amp;
    }
    public Vector3 CenterTerrainWithPlayer(int x, float y, int z)
    {
        return new Vector3(player.transform.position.x - (mapSizeX / 2) + x, y, player.transform.position.z - (mapSizeZ / 2) + z);
    }
    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        //GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
    }

    public void CreateTerrain()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[(mapSizeX + 1) * (mapSizeZ + 1)];
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float y = ReturnPerlinNoise(x, z);
                vertices[i] = CenterTerrainWithPlayer(x, y, z);
                water.Add(Instantiate(waterPrefab, CenterTerrainWithPlayer(x,waterLevel,z), Quaternion.Euler(90,0,0)));
                i++;
            }
        }
        foreach (GameObject waterSquare in water){waterSquare.transform.parent = this.transform;}

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

        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float height = vertices[i].y;
                uvs[i] = new Vector2((float)x/mapSizeX, (float)z/mapSizeZ);
                i++;
            }
        }
    }
    public void UpdateTerrain()
    {
        GetComponent<MeshCollider>().sharedMesh = mesh;
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float y = ReturnPerlinNoise(x, z);
                vertices[i] = CenterTerrainWithPlayer(x, y, z);
                water[i].transform.position = CenterTerrainWithPlayer(x, waterLevel, z);
                i++;
            }
        }
        middlePosition = player.transform.position;
        UpdateMesh();
    }


}
