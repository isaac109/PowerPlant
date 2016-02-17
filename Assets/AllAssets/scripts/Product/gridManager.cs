using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class gridManager : MonoBehaviour {

    public GameObject testInfoHolder;
    public GameObject hex;
    public GameObject manager;
    public bool done = false;
    static int _height = 30;
    static int _width = 60;
    public int cwidth = 0;
    public GameObject[][] tiles = new GameObject[_width][];

    float percentLand = .5f;
    int minLandSeeds = 5;
    int maxLandSeeds = 10;
    int percentModifier = 5;
    int cityNum = 42;
    int cityCounter = 0;
    public List<GameObject> land;
    int landCounter = 0;
    bool landCreated = false;
    int allOceanNum = 0;
    int maxAllOceanNum = 550;
    int moveCoastCounter = 0;

    int tileBiomeNum = 0;
    int[] biomeNums = new int[8];//mountain, desert, plains, valley, hills,marsh,forest,tundra
    int biomeBleedHeight = 1;
    List<int> rangeOfTundra = new List<int>();
    List<int> rangeOfForest = new List<int>();
    List<int> rangeOfDesert = new List<int>();
    List<int> acceptableX;

    List<GameObject> polarWater;
    float percentFrozenCaps = .5f;
    float environmentHealth = 1f;
    public int maxNumFrozenCaps;
    public int numFrozenCaps;

    public float maxHeight = 0;
    public float maxWidth = 0;
    public float cMaxHeight = 0;
    public float cMaxWidth = 0;

    public List<hexTile2> biomeSizeHolder;
	// Use this for initialization

    public List<continents> landMasses;
    int addNeighborChance = 100;
    public class continents
    {
        public List<hexTile2> lands;
        public continents()
        {
            lands = new List<hexTile2>();
        }
    }

    bool isTest = false;
    bool isRealistic = false;

    public List<biomeTile> biomeTileLists = new List<biomeTile>();
    public class biomeTile
    {
        public List<hexTile2> tiles = new List<hexTile2>();
    }
    List<int> biomesToChangeCount = new List<int>();

	void Start () {
        Random.seed = (int)System.DateTime.Now.Ticks;
        maxAllOceanNum = (int)(((float)(_height * _width) * (1 - percentLand)) - ((float)(_height * _width) * (1 - percentLand)) / 3 + ((float)(_height * _width) * (1 - percentLand)) / 18);
        testInfoHolder = GameObject.Find("testInfoHolder");
        landMasses = new List<continents>();
        foreach (int i in biomeNums)
        {
            biomeNums[i] = 0;
        }
        land = new List<GameObject>();
        for (int i = 0; i < _width; i++)
        {
            tiles[i] = new GameObject[_height];
            for (int j = 0; j < _height; j++)
            {
                GameObject temp;
                if (i % 2 == 0)
                {
                    temp = Instantiate(hex,new Vector3((5*Mathf.Sqrt(3)*i),0,(10*j)), Quaternion.Euler(new Vector3(270,180,0))) as GameObject;     
                }
                else
                {
                    temp = Instantiate(hex, new Vector3((5 * Mathf.Sqrt(3) * i), 0, (5 + (10 * j))), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;   
                }
                temp.name = "hex" + i.ToString() + "00" + j.ToString();
                temp.GetComponent<hexTile2>().gm = this.GetComponent<gridManager>();
                temp.GetComponent<hexTile2>().heightInArray = j;
                temp.GetComponent<hexTile2>().widthInArray = i;
                tiles[i][j] = temp;
                temp.GetComponent<hexTile2>().setTerrain(hexTile2.biomes.OCEAN);
                
            }
        }
        maxHeight = tiles[_width - 1][_height - 1].transform.position.z;
        maxWidth = tiles[_width - 1][_height - 1].transform.position.x;
        cwidth = _height;
        cMaxHeight = maxHeight;
        cMaxWidth = maxWidth;

        

        done = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (!landCreated && tiles[_width - 1][_height - 1].GetComponent<hexTile2>().searched)
        {
            //establishBorderNeighbors();
            landSeeds();
            generateContinents();
            landCreated = true;
            checkOceanSize();
            float percentBiome = .125f;
            tileBiomeNum = (int)((float)landCounter * percentBiome + 10);
            if (isRealistic)
            {
                createSpecialBiomes();
            }
            createBiomes();           
            clearIslandsSetCoasts();            
            setModifiers();
            setCitys();
            createExtraMaps();
            setPolarCaps();
            collectBiomes();
            modifyPolarCap();
            startGame();
            if (isTest)
            {
                if (testInfoHolder.GetComponent<testInfo>().itterationNum < testInfoHolder.GetComponent<testInfo>().maxItteration)
                {
                    newMapTest();
                }
                else
                {
                    testInfoHolder.GetComponent<testInfo>().print();
                }
            }
        }
        
	}
    public void nextTurn()
    {
        environmentHealth -= .1f;
        modifyPolarCap();
        //moveInCoasts();
        alterBiomes();
    }
    void checkOceanSize()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                hexTile2 temp = tiles[i][j].GetComponent<hexTile2>();
                bool allOcean = true;
                if (temp.isOcean)
                {
                    for (int k = 0; k < temp.counter; k++)
                    {
                        if (!temp.neighbors[k].GetComponent<hexTile2>().isOcean)
                        {
                            allOcean = false;
                            break;
                        }
                    }
                    if (allOcean)
                    {
                        //testInfoHolder.GetComponent<testInfo>().numOfOcean++;
                        allOceanNum ++;
                    }
                }
            }
        }
        Debug.Log("Num of ocean with all ocean neighbors: " + allOceanNum);
        if (allOceanNum > maxAllOceanNum)
        {
            SceneManager.LoadScene("map");
        }
        /*
         * for testing purposes
        Debug.Log("iteration: " +testInfoHolder.GetComponent<testInfo>().itterationNum + "," + testInfoHolder.GetComponent<testInfo>().numOfOcean);
        if (testInfoHolder.GetComponent<testInfo>().itterationNum < testInfoHolder.GetComponent<testInfo>().maxItteration)
        {
            newMapTest();
        }
        else
        {
            testInfoHolder.GetComponent<testInfo>().print();
        }*/
    }
    void newMapTest()
    {
        for (int i = 0; i < biomeNums.Length; i++)
        {
            testInfoHolder.GetComponent<testInfo>().biomeNums[i] = biomeNums[i];
        }
        testInfoHolder.GetComponent<testInfo>().itterationNum ++;
        testInfoHolder.GetComponent<testInfo>().newMap();
    }
    void landSeeds()
    {
        int x;
        int y;
        int heightRange = 5;

        x = Random.Range(0, _width);
        y = Random.Range(_height - heightRange, _height);
        seed(x, y);

        x = Random.Range(0, _width);
        y = Random.Range(0, heightRange);
        seed(x, y);

        x = Random.Range(0, _width);
        y = Random.Range(_height / 2 - heightRange / 2, _height / 2 + heightRange / 2);
        seed(x, y);

        int num = Random.Range(minLandSeeds, maxLandSeeds);
        while (landCounter < num)
        {
            y = Random.Range(0, _height);
            x = Random.Range(0, _width);
            seed(x,y);
        }
    }
    void seed(int x, int y)
    {
        if (!land.Contains(tiles[x][y]))
        {
            tiles[x][y].GetComponent<hexTile2>().setTerrain(hexTile2.biomes.MOUNTAIN);
            tiles[x][y].GetComponent<hexTile2>().isLand = true;
            land.Add(tiles[x][y]);
            landCounter++;
            continents newContinent = new continents();
            newContinent.lands.Add(tiles[x][y].GetComponent<hexTile2>());
            landMasses.Add(newContinent);
        }
    }
    void setCitys()
    {
        List<hexTile2> acceptableTiles = new List<hexTile2>();
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                bool hasMod = false;
                for (int k = 0; k < tiles[i][j].GetComponent<hexTile2>().modifierTypes.Length; k++)
                {
                    if (tiles[i][j].GetComponent<hexTile2>().modifierTypes[k] == true)
                    {
                        hasMod = true;
                    }
                }
                if (tiles[i][j].GetComponent<hexTile2>().isLand && !hasMod)
                {
                    acceptableTiles.Add(tiles[i][j].GetComponent<hexTile2>());
                }
            }
        }
        int numMaxPop = (int)((float)cityNum * .1);
        int counterMax = 0;
        int numMedPop = (int)((float)cityNum * .4);
        int counterMed = 0;
        int numMinPop = (int)((float)cityNum * .5);
        int counterMin = 0;
        int[] biomeCities = new int[8];
        int maxBiomeCityNum = (int)((float)cityNum * .125)+1;
        int biome = 0;
        while (cityCounter < cityNum || acceptableTiles.Count == 0)
        {
            int x = Random.Range(0, acceptableTiles.Count - 1);
            bool hasCityNeightbor = false;
            for (int i = 0; i < acceptableTiles[x].counter; i++)
            {
                if (acceptableTiles[x].neighbors[i].GetComponent<hexTile2>().hasCity)
                {
                    hasCityNeightbor = true;
                }
            }
            bool biomeFull = false;
            for (int i = 0; i < acceptableTiles[x].biomeTypes.Length; i++)
            {
                if (acceptableTiles[x].biomeTypes[i] == true)
                {
                    biome = i;
                    if (biomeCities[i] >= maxBiomeCityNum)
                    {
                        biomeFull = true;
                    }
                }
            }
            if (hasCityNeightbor || biomeFull)
            {
                acceptableTiles.RemoveAt(x);
                continue;
            }
            biomeCities[biome]++;
            cityCounter++;
            acceptableTiles[x].setCity(true);
            acceptableTiles[x].gameObject.AddComponent<cityHexManager>();
            if(counterMax <= numMaxPop)
            {
                acceptableTiles[x].GetComponent<cityHexManager>().population = Random.Range(900000,9000000);
            }
            else if (counterMed <= numMedPop)
            {
                acceptableTiles[x].GetComponent<cityHexManager>().population = Random.Range(300000, 900000);
            }
            else if (counterMin <= numMinPop)
            {
                acceptableTiles[x].GetComponent<cityHexManager>().population = Random.Range(100000, 300000);
            }
            acceptableTiles[x].GetComponent<cityHexManager>().cityName = acceptableTiles[x].gameObject.name;
            acceptableTiles.RemoveAt(x);
        }
    }
    void setModifiers()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            { 
                hexTile2 temp = tiles[i][j].GetComponent<hexTile2>();
                if (!temp.isOcean)
                {
                    int mod = Random.Range(0, 8);
                    int r = Random.Range(0, 100);
                    if (r <= percentModifier)
                    {
                        temp.setModifier((hexTile2.modifiers)mod, true);
                    }
                    else if (r <= 3 * percentModifier)
                    {
                        if (mod == 1 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 2 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 3 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 4 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 5 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 6 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 7 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                        if (mod == 8 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod, true);
                        }
                    }
                }
            }
        }
    }
    void createExtraMaps()
    {
       // GameObject temp = Instantiate(tiles[0][0], new Vector3(tiles[0][0].transform.position.x, tiles[0][0].transform.position.y, tiles[0][0].transform.position.z+maxHeight+5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
       // temp = Instantiate(tiles[0][1], new Vector3(tiles[1][0].transform.position.x, tiles[1][0].transform.position.y, tiles[1][0].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
       // temp = Instantiate(tiles[0][0], new Vector3(tiles[0][0].transform.position.x + 5 * Mathf.Sqrt(3)+maxWidth, tiles[0][0].transform.position.y, tiles[0][0].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c1";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i][j + _height-5], new Vector3(tiles[i][j + _height-5].transform.position.x, tiles[i][j + _height-5].transform.position.y, tiles[i][j + _height-5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c2";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c3";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + _width-5][j], new Vector3(tiles[i + _width-5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + _width-5][j].transform.position.y, tiles[i + _width-5][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c4";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c5";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + _width - 5][j], new Vector3(tiles[i + _width - 5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + _width - 5][j].transform.position.y, tiles[i + _width - 5][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c6";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i][j + _height - 5], new Vector3(tiles[i][j + _height - 5].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j + _height - 5].transform.position.y, tiles[i][j + _height - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c7";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + _width - 5][j + _height - 5], new Vector3(tiles[i + _width - 5][j + _height - 5].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + _width - 5][j + _height - 5].transform.position.y, tiles[i + _width - 5][j + _height - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c8";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
            }
        }
    }
    /*void establishBorderNeighbors()
    {
        for(int i=0; i < _width; i++)
        {
            for(int j = 0; j < _height; j++)
            {
                if (j == _height-1)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[i][0];
                    if (i % 2 != 0 && i != _width-1)
                    {
                        tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[i - 1][0];
                        tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[i + 1][0];
                    }
                }
                if (j == 0)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[i][_height-1];
                    if (i % 2 == 0 && i != 0)
                    {
                        tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[i - 1][_height - 1];
                        tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[i + 1][_height - 1];
                    }
                }
                if (i == 0 && (j != 0 && j != _height - 1))
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[_width - 1][j];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[_width - 1][j - 1];
                }
                if (i == _width - 1 && (j != 0 && j != _height - 1))
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[0][j];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[0][j + 1];
                }
                if (i == 0 && j == 0)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[2] = tiles[1][_height - 1];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[_width - 1][_height - 1];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[_width - 1][0];
                }
                if (i == _width - 1 && j == _height - 1)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[2] = tiles[_width - 2][0];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[0][0];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[0][_height - 1];
                }
                if (i == _width - 1 && j == 0)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[0][0];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[0][1];
                }
                if (i == 0 && j == _height - 1)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[_width - 1][_height - 1];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[_width - 1][_height - 2];
                }
                tiles[i][j].GetComponent<hexTile2>().counter = 6;
            }
        }
    }*/
    void setIfCoast(GameObject tile)
    {
        hexTile2 temp = tile.GetComponent<hexTile2>();
        for (int i = 0; i < temp.counter; i++)
        {
            if (!temp.neighbors[i].GetComponent<hexTile2>().isOcean && temp.isOcean)
            {
                temp.isCoast = true;
                break;
            }
        }
    }
    void clearIslandsSetCoasts()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                changeIfIsland(tiles[i][j]);
                setIfCoast(tiles[i][j]);
            }
        }
    }
    void collectBiomes()
    {
        for (int i = 0; i < 8; i++)
        {
            biomeTileLists.Add(new biomeTile());
        }
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (tiles[i][j].GetComponent<hexTile2>().isLand)
                {
                    for (int k = 0; k < tiles[i][j].GetComponent<hexTile2>().biomeTypes.Length; k++)
                    {
                        if (tiles[i][j].GetComponent<hexTile2>().biomeTypes[k] == true)
                        {
                            biomeTileLists[k].tiles.Add(tiles[i][j].GetComponent<hexTile2>());
                        }
                    }
                }
            }
        }
        for (int i = 0; i < biomeTileLists.Count; i++)
        {
            biomesToChangeCount.Add(biomeTileLists[i].tiles.Count / 9);       
        }
    }
    void alterBiomes()
    {
        for (int i = 0; i < biomeTileLists.Count; i++)
        {
            if (biomeTileLists[i].tiles.Count != 0)
            {
                for (int j = 0; j < biomesToChangeCount[i]; j++)
                {
                    if (biomeTileLists[i].tiles.Count != 0)
                    {
                        int r = Random.Range(0, biomeTileLists[i].tiles.Count - 1);
                        biomeTileLists[i].tiles[r].isSunBleached = true;
                        biomeTileLists[i].tiles.RemoveAt(r);
                    }
                }
            }
        }
    }
    void moveInCoasts()
    {
        bool moveCoast = false;
        if (environmentHealth <= .6 && moveCoastCounter == 0)
        {
            moveCoastCounter++;
            moveCoast = true;
        }
        if (environmentHealth <= .2 && moveCoastCounter == 1)
        {
            moveCoastCounter++;
            moveCoast = true;
        }
        if (moveCoast)
        {
            List<hexTile2> coasts = new List<hexTile2>();
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    if (tiles[i][j].GetComponent<hexTile2>().isCoast)
                    {
                        coasts.Add(tiles[i][j].GetComponent<hexTile2>());
                    }
                }
            }
            for (int i = 0; i < coasts.Count; i++)
            {
                for (int j = 0; j < coasts[i].counter; j++)
                {
                    hexTile2 temp = coasts[i].neighbors[j].GetComponent<hexTile2>();
                    if (temp.isLand)
                    {
                        temp.setTerrain(hexTile2.biomes.OCEAN);
                        temp.setModifier(hexTile2.modifiers.COAL, false);
                        temp.setCity(false);
                        temp.isCoast = true;
                    }
                }
                coasts[i].isCoast = false;
            }
        }
    }
    void changeIfIsland(GameObject tile)
    {
        int numOfMou = 0;
        int numOfDes = 0;
        int numOfPla = 0;
        int numOfVal = 0;
        int numOfHil = 0;
        int numOfMar = 0;
        int numOfFor = 0;
        int numOfTun = 0;
        hexTile2 temp = tile.GetComponent<hexTile2>();
        for (int i = 0; i < temp.counter; i++)
        {
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[0])
            {
                numOfMou++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[1])
            {
                numOfDes++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[2])
            {
                numOfPla++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[3])
            {
                numOfVal++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[4])
            {
                numOfHil++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[5])
            {
                numOfMar++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[6])
            {
                numOfFor++;
            }
            if (temp.neighbors[i].GetComponent<hexTile2>().biomeTypes[7])
            {
                numOfTun++;
            }
        }
        if (numOfMou + numOfDes + numOfPla + numOfVal + numOfHil + numOfMar + numOfFor + numOfTun == 0)
        {
            temp.setTerrain(hexTile2.biomes.OCEAN);
        }
        if ((temp.biomeTypes[0] && numOfMou == 0) ||
            (temp.biomeTypes[1] && numOfDes == 0) ||
            (temp.biomeTypes[2] && numOfPla == 0) ||
            (temp.biomeTypes[3] && numOfVal == 0) ||
            (temp.biomeTypes[4] && numOfHil == 0) ||
            (temp.biomeTypes[5] && numOfMar == 0) ||
            (temp.biomeTypes[6] && numOfFor == 0) ||
            (temp.biomeTypes[7] && numOfTun == 0))
        {
            int num = Random.Range(1,(numOfMou+numOfDes+numOfPla+numOfVal+numOfHil+numOfMar+numOfFor+numOfTun+1));
            if (num <= numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.MOUNTAIN);
            }
            else if (num <= numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.DESERT);
            }
            else if (num <= numOfPla + numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.PLAINS);
            }
            else if (num <= numOfVal + numOfPla + numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.VALLEY);
            }
            else if (num <= numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.HILLS);
            }
            else if (num <= numOfMar + numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.MARSHES);
            }
            else if (num <= numOfFor + numOfMar + numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.FOREST);
            }
            else if (num <= numOfTun + numOfFor + numOfMar + numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                temp.setTerrain(hexTile2.biomes.TUNDRA);
            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }
    }
    void setPolarCaps()
    {
        polarWater = new List<GameObject>(); 
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if ((j < _height / 5 || j > _height - _height / 5) && tiles[i][j].GetComponent<hexTile2>().isOcean &&
                    !tiles[i][j].GetComponent<hexTile2>().isFrozen)
                {
                    polarWater.Add(tiles[i][j]);
                }
            }
        }
        maxNumFrozenCaps = (int)(environmentHealth * percentFrozenCaps * polarWater.Count);
        while (numFrozenCaps < maxNumFrozenCaps)
        {        
            int i = Random.Range(0, polarWater.Count - 1);
            biomeSizeHolder = new List<hexTile2>();
            genPolarCaps(polarWater[i]);
        }
    }
    void genPolarCaps(GameObject start)
    {
        if (biomeSizeHolder.Count >= 10)
        {
            return;
        }
        hexTile2 temp = start.GetComponent<hexTile2>();
        temp.setTerrain(hexTile2.biomes.GLACIER);
        temp.isChecked = true;
        numFrozenCaps++;
        biomeSizeHolder.Add(temp);
        List<GameObject> acceptableNeighbors = new List<GameObject>();
        for (int i = 0; i < temp.counter; i++)
        {
            if (!temp.neighbors[i].GetComponent<hexTile2>().isChecked && temp.neighbors[i].GetComponent<hexTile2>().isOcean 
                && (temp.neighbors[i].GetComponent<hexTile2>().heightInArray < _height/5 || 
                temp.neighbors[i].GetComponent<hexTile2>().heightInArray > _height- _height/5))
            {
                acceptableNeighbors.Add(temp.neighbors[i]);
            }
        }
        int numTurning = Random.Range(0, acceptableNeighbors.Count);
        if (numTurning != 0)
        {
            for (int i = 0; i < numTurning; i++)
            {
                int neighbor = Random.Range(0,acceptableNeighbors.Count-1);
                polarWater.Remove(start);
                genPolarCaps(acceptableNeighbors[neighbor]);
            }
        }
    }
    void modifyPolarCap()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                hexTile2 temp = tiles[i][j].GetComponent<hexTile2>();
                if (temp.isFrozen)
                {
                    if (temp.heightInArray > (int)(_height / 5 * environmentHealth) -1 || temp.heightInArray < (int)(_height - _height / 5 * environmentHealth) +1)
                    {
                        temp.setTerrain(hexTile2.biomes.OCEAN);
                    }
                    if (temp.heightInArray == (int)(_height / 5 * environmentHealth) -1 || temp.heightInArray == (int)( _height - _height / 5 * environmentHealth) +1)
                    {
                        temp.setTerrain(hexTile2.biomes.ICEBERG);
                    }
                    if (temp.heightInArray < (int)(_height / 5 * environmentHealth) -1 || temp.heightInArray > (int)(_height - _height / 5 * environmentHealth) +1)
                    {
                        temp.setTerrain(hexTile2.biomes.GLACIER);
                    }
                }
                if (temp.isFrozen)
                {
                    int notFrozenNeighbors = 0;
                    for (int k = 0; k < temp.counter; k++)
                    {
                        hexTile2 tempNeighbor = temp.neighbors[k].GetComponent<hexTile2>();
                        if (tempNeighbor.isOcean && !tempNeighbor.isFrozen)
                        {
                            notFrozenNeighbors++;
                        }
                    }
                    if (notFrozenNeighbors == 3 || notFrozenNeighbors == 4)
                    {
                        temp.setTerrain(hexTile2.biomes.ICEBERG);
                    }
                    if (notFrozenNeighbors >= 5)
                    {
                        temp.setTerrain(hexTile2.biomes.OCEAN);
                    }
                    
                }
                if (temp.isOcean && !temp.isFrozen && (temp.heightInArray < (int)(_height / 5 * (environmentHealth) - 2) || temp.heightInArray > (int)(_height - _height / 5 * (environmentHealth) + 2)))
                {
                    temp.setTerrain(hexTile2.biomes.GLACIER);
                }
            }
        }
    }
    void createSpecialBiomes()
    { 
        int heightChunk = _height / 5;

        for(int i =0; i < _height; i++)
        {
            if (i >= (_height - heightChunk - biomeBleedHeight) || i <= heightChunk + biomeBleedHeight)
            {
                rangeOfTundra.Add(i);
            }
            if ((i <= (_height - heightChunk + biomeBleedHeight - 2) && i >= (_height - heightChunk * 2) - biomeBleedHeight + 2) ||
                    (i >= heightChunk - biomeBleedHeight + 2 && i <= heightChunk * 2 + biomeBleedHeight - 2))
            {
                rangeOfForest.Add(i);
            }
            if (i <= (_height - heightChunk * 2 + biomeBleedHeight) && i >= (heightChunk * 2) - biomeBleedHeight)
            {
                rangeOfDesert.Add(i);
            }
        }

        int counter = 0;
        int counterMax = 500;
        while (biomeNums[1] < tileBiomeNum)
        {
            int y = Random.Range(0, rangeOfDesert.Count);
            y = rangeOfDesert[y];
            acceptableX = new List<int>();
            for (int i = 0; i < _width; i++)
            {
                if (tiles[i][y].GetComponent<hexTile2>().isLand)
                {
                    acceptableX.Add(i);
                }
            }
            if (acceptableX.Count == 0)
            {
                rangeOfDesert.RemoveAt(y);
                continue;
            }
            int x = Random.Range(0, acceptableX.Count);
            x = acceptableX[x];
            if (tiles[x][y].GetComponent<hexTile2>().isLand && !tiles[x][y].GetComponent<hexTile2>().isChecked)
            {
                genBiome(tiles[x][y], hexTile2.biomes.DESERT);
            }
            if (rangeOfDesert.Count == 0 || counter > counterMax)
            {
                break;
            }
            counter++;
        }
        counter = 0;
        while (biomeNums[6] < tileBiomeNum)
        {
            int y = Random.Range(0, rangeOfForest.Count);
            y = rangeOfForest[y];
            acceptableX = new List<int>();
            for (int i = 0; i < _width; i++)
            {
                if (tiles[i][y].GetComponent<hexTile2>().isLand)
                {
                    acceptableX.Add(i);
                }
            }
            if (acceptableX.Count == 0)
            {
                rangeOfForest.RemoveAt(y);
                continue;
            }
            int x = Random.Range(0, acceptableX.Count);
            x = acceptableX[x];
            if (tiles[x][y].GetComponent<hexTile2>().isLand && !tiles[x][y].GetComponent<hexTile2>().isChecked)
            {
                genBiome(tiles[x][y], hexTile2.biomes.FOREST);
            }
            if (rangeOfForest.Count == 0 || counter > counterMax)
            {
                break;
            }
            counter++;
        }
        counter = 0;
        while (biomeNums[7] < tileBiomeNum)
        {
            int y = Random.Range(0, rangeOfTundra.Count-1);
            y = rangeOfTundra[y];
            acceptableX = new List<int>();
            for (int i = 0; i < _width; i++)
            {
                if (tiles[i][y].GetComponent<hexTile2>().isLand)
                {
                    acceptableX.Add(i);
                }
            }
            if (acceptableX.Count == 0)
            {
                rangeOfTundra.RemoveAt(y);
                continue;
            }
            int x = Random.Range(0, acceptableX.Count - 1);
            x = acceptableX[x];
            if (tiles[x][y].GetComponent<hexTile2>().isLand && !tiles[x][y].GetComponent<hexTile2>().isChecked)
            {
                genBiome(tiles[x][y], hexTile2.biomes.TUNDRA);
            }
            if (rangeOfTundra.Count == 0 || counter > counterMax)
            {
                break;
            }
            counter++;
        }
    }
    void createBiomes()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                hexTile2 temp = tiles[i][j].GetComponent<hexTile2>();
                if (!temp.isChecked && !temp.isOcean)
                {
                    selectBiome(tiles[i][j]);
                }
            }
        }
    }
    void selectBiome(GameObject start)
    {
        biomeSizeHolder = new List<hexTile2>();
        List<int> notFullBiomes = new List<int>();
        for (int i = 0; i < biomeNums.Length; i++)
        {
            if (biomeNums[i] < tileBiomeNum)
            {
                notFullBiomes.Add(i);
            }
        }
        int biome = Random.Range(0, notFullBiomes.Count);
        biome = notFullBiomes[biome];
        switch (biome)
        {
            case 0:
                genBiome(start, hexTile2.biomes.MOUNTAIN);
                break;
            case 1:
                genBiome(start, hexTile2.biomes.DESERT);
                break;
            case 2:
                genBiome(start, hexTile2.biomes.PLAINS);
                break;
            case 3:
                genBiome(start, hexTile2.biomes.VALLEY);
                break;
            case 4:
                genBiome(start, hexTile2.biomes.HILLS);
                break;
            case 5:
                genBiome(start, hexTile2.biomes.MARSHES);
                break;
            case 6:
                genBiome(start, hexTile2.biomes.FOREST);
                break;
            case 7:
                genBiome(start, hexTile2.biomes.TUNDRA);
                break;
        }
    }
    void genBiome(GameObject start, hexTile2.biomes biome)
    {
        hexTile2 temp = start.GetComponent<hexTile2>();
        if (isRealistic)
        {
            if (!checkBiomeLocation(temp, biome))
            {
                return;
            }
            if (biome == hexTile2.biomes.DESERT || biome == hexTile2.biomes.TUNDRA || biome == hexTile2.biomes.FOREST)
            {
                acceptableX.Remove(temp.widthInArray);
            }
        }
        temp.setTerrain(biome);
        biomeNums[(int)biome - 1]++;
        temp.isChecked = true;
        biomeSizeHolder.Add(temp);
        List<GameObject> acceptableNeighbors = new List<GameObject>();
        for(int i = 0; i < temp.counter; i++)
        {
            if(!temp.neighbors[i].GetComponent<hexTile2>().isChecked && !temp.neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors.Add(temp.neighbors[i]);
            }
        }
        int numTurning = Random.Range(0, acceptableNeighbors.Count);
        if (numTurning != 0)
        {
            for (int i = 0; i < numTurning; i++)
            {
                int neighbor = Random.Range(0, acceptableNeighbors.Count - 1);

                if (biomeNums[(int)biome-1] == tileBiomeNum || biomeSizeHolder.Count >= 10)
                {
                    return;
                }
                genBiome(acceptableNeighbors[neighbor], biome);
            }
        }
    }
    bool checkBiomeLocation(hexTile2 tile, hexTile2.biomes biome)
    {
        int heightChunk = _height / 5;
        int heightInArray = tile.heightInArray;
        switch (biome)
        {
            case hexTile2.biomes.TUNDRA:
                if (heightInArray >= (_height - heightChunk - biomeBleedHeight) || heightInArray <= heightChunk + biomeBleedHeight)
                {
                    return true;
                }
                break;
            case hexTile2.biomes.FOREST:
                if ((heightInArray <= (_height - heightChunk + biomeBleedHeight) && heightInArray >= (_height - heightChunk * 2) - biomeBleedHeight) ||
                    (heightInArray >= heightChunk - biomeBleedHeight && heightInArray <= heightChunk * 2 + biomeBleedHeight))
                {
                    return true;
                }
                break;
            case hexTile2.biomes.DESERT:
                if(heightInArray <= (_height - heightChunk*2 + biomeBleedHeight) && heightInArray >= (heightChunk*2) - biomeBleedHeight)
                {
                    return true;
                }
                break;
            default:
                return true;
        }
        return false;
    }
    void generateContinents()
    {
        for (int i = 0; i < land.Count; i++)
        {
            if (land.Count >= (_width * _height * percentLand))
            {
                return;
            }
            hexTile2 temp = land[i].GetComponent<hexTile2>();
            List<GameObject> neighbors = new List<GameObject>();
            for (int j = 0; j < temp.counter; j++)
            {
                neighbors.Add(temp.neighbors[j]);
            }
            int num = Random.Range(0, neighbors.Count);
            int counter = 0;
            while (counter < num)
            {
                int selected = Random.Range(0, neighbors.Count);
                bool neighborContinent = onOtherContinent(neighbors[selected], temp);
                bool addIfNeighborContinent = true;
                if (neighborContinent)
                {
                    int r = Random.Range(0, 100);
                    if (r >= addNeighborChance)
                    {
                        addIfNeighborContinent = true;
                    }
                    else
                    {
                        addIfNeighborContinent = false;
                    }
                }
                if (!land.Contains(neighbors[selected]) && addIfNeighborContinent)
                {
                    land.Add(neighbors[selected]);
                    neighbors[selected].GetComponent<hexTile2>().setTerrain(hexTile2.biomes.MOUNTAIN);
                    neighbors[selected].GetComponent<hexTile2>().isLand = true;
                    landCounter++;
                    addToContinent(neighbors[selected], temp);
                }
                neighbors.RemoveAt(selected);
                counter++;
            }
        }
        if(land.Count < (_width*_height*percentLand))
        {
            generateContinents();
        }
    }
    GameObject[] addToBack(GameObject obj, GameObject[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == null)
            {
                arr[i] = obj;
                return arr;
            }
        }
        return arr;
    }
    bool contains(GameObject obj, GameObject[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == obj)
            {
                return true;
            }
        }
        return false;
    }
    bool onOtherContinent(GameObject obj, hexTile2 origional)
    {
        for (int i = 0; i < landMasses.Count; i++)
        {
            if (landMasses[i].lands.Contains(origional))
            {
                continue;
            }
            else
            {
                for (int j = 0; j < obj.GetComponent<hexTile2>().counter; j++)
                {
                    if (landMasses[i].lands.Contains(obj.GetComponent<hexTile2>().neighbors[j].GetComponent<hexTile2>()))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    void addToContinent(GameObject obj, hexTile2 origional)
    {
        for (int i = 0; i < landMasses.Count; i++)
        {
            if(landMasses[i].lands.Contains(origional))
            {
                landMasses[i].lands.Add(obj.GetComponent<hexTile2>());
                break;
            }
        }
    }

    void startGame()
    {
        ExecuteEvents.Execute<IPointerClickHandler>(this.GetComponent<keyListener>().menus[9].GetComponent<updateTileUI>().buttons[6].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        this.GetComponent<keyListener>().toggleMenu(0);


        List <GameObject> cities = new List<GameObject>();
        cities = getCities();
        List<IStatusEntity> cityEntities = new List<IStatusEntity>();
        foreach (GameObject item in cities)
        {
            CityStatusEntity cse = new CityStatusEntity();
            cse.population = item.GetComponent<cityHexManager>().population;
            cse.powerStatus = 0;
            cityEntities.Add(cse);
        }
        ResearchDevelopmentStatusEntity rdse = new ResearchDevelopmentStatusEntity();
        rdse.listOfPowerPlantUpgrades = new List<IStatusEntity>();
        for (int i = 0; i < 10; i++)
        {
            PowerPlantUpgradeStatusEntity ppuse = new PowerPlantUpgradeStatusEntity();
            ppuse.researchState = 0;
            ppuse.listOfUpgrades = new List<IStatusEntity>();
            for (int j = 0; j < 5; j++)
            {
                ResearchDevelopmentUpgradeStatusEntity rduse = new ResearchDevelopmentUpgradeStatusEntity();
                rduse.researchState = 0;
                ppuse.listOfUpgrades.Add(rduse);
            }
            rdse.listOfPowerPlantUpgrades.Add(ppuse);
        }
        manager.GetComponent<GameManager>().StartGame(cityEntities, rdse);
    }

    public List<GameObject> getCities()
    {
        List<GameObject> cities = new List<GameObject>();
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (tiles[i][j].GetComponent<hexTile2>().hasCity)
                {
                    cities.Add(tiles[i][j]);
                }
            }
        }
        return cities;
    }
    public List<GameObject> getCitiesInRange(int x, int y, int range)
    {
        List<GameObject> cities = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(tiles[x][y].transform.position, (float)(10 * range));
        foreach (Collider item in hitColliders)
        {
            if (item.gameObject.GetComponent<hexTile2>())
            {
                if (item.gameObject.GetComponent<hexTile2>().hasCity)
                {
                    cities.Add(item.gameObject);
                }
            }
        }
        return cities;
    }
    public List<GameObject> getPPs()
    {
        List<GameObject> pps = new List<GameObject>();
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (tiles[i][j].GetComponent<hexTile2>().hasPowerPlant)
                {
                    pps.Add(tiles[i][j]);
                }
            }
        }
        return pps;
    }
    public void zoom(int i, int j)
    {
        tiles[i][j].GetComponent<hexTile2>().zoom();
    }

}
