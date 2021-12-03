using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public enum Biome
    {
        GrassLands,
        Mountains,
        Beach,
        Snow,
        Length
    }
    public GameObject player;
    public GameObject waterPrefab;
    [HideInInspector]
    public int mapSizeX;
    [HideInInspector]
    public int mapSizeZ;
    [HideInInspector]
    public float amp;
    [HideInInspector]
    public float freq;
    [HideInInspector]
    public float waterLevel;
    [HideInInspector]
    public bool smoothGen = false;
    private float mountainHeight = 10;
    private float mountainSnowHeight = 6;
    private GameObject water;
    private Mesh mesh;
    private Vector3[] vertices;
    private Vector2[] uvs;
    private Biome[] biomes;
    private int[][] triangles;
    private Vector3 middlePosition;
    private float timeGenSpeed = 10;
    private GameObject gameManagerObj;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManagerObj == null) { gameManagerObj = GameObject.Find("GameManager"); }
        if (gameManagerObj != null) { gameManager = gameManagerObj.GetComponent<GameManager>(); }
        if (gameManager != null)
        {
            mapSizeX = gameManager.newX;
            mapSizeZ = gameManager.newZ;
            amp = gameManager.newAmp;
            freq = gameManager.newFreq;
            waterLevel = gameManager.newWaterLevel;
            smoothGen = gameManager.nextSmoothGen;
        }
        CreateTerrain();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(amp / (freq * 10));
        if (smoothGen == false)
        {
            if (player.transform.position.x >= middlePosition.x + mapSizeX / 4 && player.transform.position.x > middlePosition.x || player.transform.position.x <= middlePosition.x - mapSizeX / 4 && player.transform.position.x < middlePosition.x || player.transform.position.z >= middlePosition.z + mapSizeZ / 4 && player.transform.position.z > middlePosition.z || player.transform.position.z <= middlePosition.z - mapSizeZ / 4 && player.transform.position.z < middlePosition.z)
            {
                CreateTerrain();
            }
        }
        else if (smoothGen == true){CreateTerrain();}
    }
    public float ReturnPerlinNoise(int x, int z)
    {
        //time-gen math is for chunk gen and so the terrain doesn't generate too fast or make player appear faster than should
        return Mathf.PerlinNoise((player.transform.position.x / (timeGenSpeed / (freq * 10))) + x * freq, (player.transform.position.z / (timeGenSpeed / (freq * 10))) + z * freq) * amp;
    }
    public Vector3 CenterTerrainWithPlayer(int x, float y, int z)
    {
        return new Vector3(player.transform.position.x - (mapSizeX / 2) + x, y, player.transform.position.z - (mapSizeZ / 2) + z);
    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.subMeshCount = 4;
        mesh.vertices = vertices;
        //mesh.triangles = triangles;
        mesh.SetTriangles(triangles[0], 0);
        mesh.SetTriangles(triangles[1], 1);
        mesh.SetTriangles(triangles[2], 2);
        mesh.SetTriangles(triangles[3], 3);
        mesh.uv = uvs;

        mesh.RecalculateNormals();

        //GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
    }

    public void CreateTerrain()
    {
        Destroy(water);
        water = Instantiate(waterPrefab, CenterTerrainWithPlayer(mapSizeX / 2, waterLevel, mapSizeZ / 2), Quaternion.Euler(90, 0, 0));
        triangles = new int[(int)Biome.Length][];
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        vertices = new Vector3[(mapSizeX + 1) * (mapSizeZ + 1)];
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float y = ReturnPerlinNoise(x, z);
                vertices[i] = CenterTerrainWithPlayer(x, y, z);
                water.transform.localScale = new Vector3(mapSizeX, mapSizeZ, 1);
                i++;
            }
        }
        water.transform.parent = transform;

        //determines biomes based off height
        biomes = new Biome[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y >= mountainHeight && vertices[i].y < mountainHeight + mountainSnowHeight) { biomes[i] = Biome.Mountains; }
            else if (vertices[i].y < mountainHeight && vertices[i].y > waterLevel) { biomes[i] = Biome.GrassLands; }
            else if (vertices[i].y < waterLevel) { biomes[i] = Biome.Beach; }
            else if (vertices[i].y > mountainHeight + mountainSnowHeight) { biomes[i] = Biome.Snow; }
        }

        //sets triangles for each biome
        triangles[0] = new int[mapSizeX * mapSizeZ * 6];
        triangles[1] = new int[mapSizeX * mapSizeZ * 6];
        triangles[2] = new int[mapSizeX * mapSizeZ * 6];
        triangles[3] = new int[mapSizeX * mapSizeZ * 6];
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < mapSizeZ; z++)
        {
            for (int x = 0; x < mapSizeX; x++)
            {
                if (biomes[vert] == Biome.Mountains)
                {
                    triangles[0][tris + 0] = vert + 0;
                    triangles[0][tris + 1] = vert + mapSizeX + 1;
                    triangles[0][tris + 2] = vert + 1;
                    triangles[0][tris + 3] = vert + 1;
                    triangles[0][tris + 4] = vert + mapSizeX + 1;
                    triangles[0][tris + 5] = vert + mapSizeX + 2;
                }
                else if (biomes[vert] == Biome.GrassLands)
                {
                    triangles[1][tris + 0] = vert + 0;
                    triangles[1][tris + 1] = vert + mapSizeX + 1;
                    triangles[1][tris + 2] = vert + 1;
                    triangles[1][tris + 3] = vert + 1;
                    triangles[1][tris + 4] = vert + mapSizeX + 1;
                    triangles[1][tris + 5] = vert + mapSizeX + 2;
                }
                else if (biomes[vert] == Biome.Snow)
                {
                    triangles[2][tris + 0] = vert + 0;
                    triangles[2][tris + 1] = vert + mapSizeX + 1;
                    triangles[2][tris + 2] = vert + 1;
                    triangles[2][tris + 3] = vert + 1;
                    triangles[2][tris + 4] = vert + mapSizeX + 1;
                    triangles[2][tris + 5] = vert + mapSizeX + 2;
                }
                else if (biomes[vert] == Biome.Beach)
                {
                    triangles[3][tris + 0] = vert + 0;
                    triangles[3][tris + 1] = vert + mapSizeX + 1;
                    triangles[3][tris + 2] = vert + 1;
                    triangles[3][tris + 3] = vert + 1;
                    triangles[3][tris + 4] = vert + mapSizeX + 1;
                    triangles[3][tris + 5] = vert + mapSizeX + 2;
                }

                vert++;
                tris += 6;
            }
            vert++;
        }
        //allows texture to be displayed...
        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= mapSizeZ; z++)
        {
            for (int x = 0; x <= mapSizeX; x++)
            {
                float height = vertices[i].y;
                uvs[i] = new Vector2((float)x / mapSizeX, (float)z / mapSizeZ);
                i++;
            }
        }
        middlePosition = player.transform.position;
        UpdateMesh();
        //updates collider....
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}
