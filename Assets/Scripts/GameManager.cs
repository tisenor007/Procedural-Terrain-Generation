using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int newX = 75;
    public int newZ = 75;
    public float newWaterLevel = 5;
    public float newAmp = 10f;
    public float newFreq = 0.06f;
    public bool nextSmoothGen = false;
    public static GameManager gameManager;

    public Text xSizeTxt;
    public Text zSizeTxt;
    public Text newAmpTxt;
    public Text newFreqTxt;
    public Text newWaterLevelTxt;
    public Text nextSmoothGenTxt;
    private GameObject terrainGeneratorObj;
    private TerrainGenerator terrainGenerator;


    void Awake()
    {
        //singleton pattern
        if (gameManager == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        //RegenerateMap();
    }
    void Update()
    {
        if (terrainGeneratorObj == null) { terrainGeneratorObj = GameObject.Find("TerrainGenerator"); }
        if (terrainGeneratorObj != null) { terrainGenerator = terrainGeneratorObj.GetComponent<TerrainGenerator>(); }
        if (terrainGenerator != null)
        {
            if (terrainGenerator.mapSizeX == newX) { xSizeTxt.text = "(Current) X Size: " + newX; }
            else if (terrainGenerator.mapSizeX != newX) { xSizeTxt.text = "Next Gen X Size: " + newX; }
            if (terrainGenerator.mapSizeZ == newZ) { zSizeTxt.text = "(Current) Z Size: " + newZ; }
            else if (terrainGenerator.mapSizeZ != newZ) { zSizeTxt.text = "Next Gen Z Size: " + newZ; }
            if (terrainGenerator.amp == newAmp) { newAmpTxt.text = "(Current) Hill Height: " + newAmp; }
            else if (terrainGenerator.amp != newAmp) { newAmpTxt.text = "Next Gen Hill Height: " + newAmp; }
            if (terrainGenerator.freq == newFreq) { newFreqTxt.text = "(Current) Hill Rarity: " + newFreq; }
            else if (terrainGenerator.freq != newFreq) { newFreqTxt.text = "Next Gen Hill Rarity: " + newFreq; }
            if (terrainGenerator.waterLevel == newWaterLevel) { newWaterLevelTxt.text = "(Current) Water Level: " + newWaterLevel; }
            else if (terrainGenerator.waterLevel != newWaterLevel) { newWaterLevelTxt.text = "Next Gen Water Level: " + newWaterLevel; }
            if (terrainGenerator.smoothGen == nextSmoothGen)
            {
                if (nextSmoothGen == true) { nextSmoothGenTxt.text = "(Current) Generation: SMOOTH"; }
                if (nextSmoothGen == false) { nextSmoothGenTxt.text = "(Current) Generation: CHUNK"; }
            }
            else if (terrainGenerator.smoothGen != nextSmoothGen)
            {
                if (nextSmoothGen == true) { nextSmoothGenTxt.text = "Next Gen Generation: SMOOTH"; }
                if (nextSmoothGen == false) { nextSmoothGenTxt.text = "Next Gen Generation: CHUNK"; }
            }
        }
    }
    public void RegenerateMap()
    {
        SceneManager.LoadScene("World", LoadSceneMode.Single);
    }

    public void SwitchGeneration()
    {
        if (nextSmoothGen == true) { nextSmoothGen = false; }
        else if (nextSmoothGen == false) { nextSmoothGen = true; }
    }

    public void AdjustXSize(float value)
    {
        newX = (int)value;
    }

    public void AdjustZSize(float value)
    {
        newZ = (int)value;
    }

    public void AdjustAmplitude(float value)
    {
        newAmp = (int)value;
    }

    public void AdjustFrequency(float value)
    {
        newFreq = value;
    }

    public void AdjustWaterLevel(float value)
    {
        newWaterLevel = value;
    }
}
