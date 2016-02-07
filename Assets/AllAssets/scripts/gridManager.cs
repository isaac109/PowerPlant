﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gridManager : MonoBehaviour {

    public GameObject testInfoHolder;
    public GameObject hex;
    public bool done = false;
    static int _height = 30;
    static int _width = 60;
    public int cwidth = 0;
    public GameObject[][] tiles = new GameObject[_width][];

    float percentLand = .55f;
    int minLandSeeds = 5;
    int maxLandSeeds = 10;
    int percentModifier = 5;
    float percentCity = .025f;
    int cityNum = 0;
    int cityCounter = 0;
    public List<GameObject> land;
    int landCounter = 0;
    bool landCreated = false;
    int allOceanNum = 0;
    int maxAllOceanNum = 550;

    int tileBiomeNum = 0;
    int[] biomeNums = new int[8];//mountain, desert, plains, valley, hills,marsh,forest,tundra
    int biomeBleedHeight = 1;
    List<int> rangeOfTundra = new List<int>();
    List<int> rangeOfForest = new List<int>();
    List<int> rangeOfDesert = new List<int>();
    List<int> acceptableX;

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
        cityNum = (int)((float)(_width * _height)*(float)(percentCity));
        cwidth = _height;
        cMaxHeight = maxHeight;
        cMaxWidth = maxWidth;

        

        done = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (tiles[_width - 1][_height - 1].GetComponent<hexTile2>().searched && !landCreated)
        {
            //establishBorderNeighbors();
            landSeeds();
            generateContinents();
            landCreated = true;
            checkOceanSize();
            float percentBiome = .125f;
            tileBiomeNum = (int)((float)landCounter * percentBiome + 10);
            createSpecialBiomes();
            createBiomes();
            clearIslandsSetCoasts();
            setModifiers();
            setCitys();
            createExtraMaps();
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
            Application.LoadLevel("map");
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
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                hexTile2 temp = tiles[i][j].GetComponent<hexTile2>();
                bool hasMod = false;
                bool hasNeighborCity = false;
                for( int k = 0; k < temp.modifierTypes.Length; k++)
                {
                    if(temp.modifierTypes[k] == true)
                    {
                        hasMod = true;
                    }
                }
                for (int k = 0; k < temp.counter; k++)
                {
                    if (temp.neighbors[k].GetComponent<hexTile2>().hasCity == true)
                    {
                        hasNeighborCity = true;
                    }
                }
                if (!temp.hasCity && cityCounter < cityNum && !temp.isOcean && !hasMod && !hasNeighborCity)
                {
                    int r = Random.Range(0, 99);
                    if (r <= percentCity*100)
                    {
                        temp.setCity();
                        cityCounter++;
                    }
                }
            }
        }
        if (cityCounter < cityNum)
        {
            setCitys();
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
                        temp.setModifier((hexTile2.modifiers)mod);
                    }
                    else if (r <= 3 * percentModifier)
                    {
                        if (mod == 1 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 2 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 3 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 4 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 5 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 6 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 7 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
                        }
                        if (mod == 8 && temp.modifierTypes[mod - 1])
                        {
                            temp.setModifier((hexTile2.modifiers)mod);
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
        if (!checkBiomeLocation(temp, biome))
        {
            return;
        }
        if (biome == hexTile2.biomes.DESERT || biome == hexTile2.biomes.TUNDRA || biome == hexTile2.biomes.FOREST)
        {
            acceptableX.Remove(temp.widthInArray);
        }
        temp.setTerrain(biome);
        biomeNums[(int)biome - 1]++;
        temp.isChecked = true;
        biomeSizeHolder.Add(temp);
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for(int i = 0; i < temp.counter; i++)
        {
            if(!temp.neighbors[i].GetComponent<hexTile2>().isChecked && !temp.neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = temp.neighbors[i];
                neighborNum ++;
            }
        }
        int numTurning = Random.Range(0, neighborNum);
        if (numTurning != 0)
        {
            bool[] selectedNeighbors = new bool[6];
            for (int k = 0; k < 6; k++)
            {
                selectedNeighbors[k] = false;
            }
            for (int i = 0; i < numTurning; i++)
            {
                int neighbor = 0;
                do
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                while (selectedNeighbors[neighbor] == true);

                selectedNeighbors[neighbor] = true;
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
}
