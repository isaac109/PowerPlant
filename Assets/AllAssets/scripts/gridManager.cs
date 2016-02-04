using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gridManager : MonoBehaviour {

    public GameObject testInfoHolder;
    public GameObject hex;
    public bool done = false;
    static int width = 30;
    static int height = 60;
    public int cwidth = 0;
    public GameObject[][] tiles = new GameObject[height][];

    float percentLand = .5f;
    float percentLandSeed = 10;
    int minLandSeeds = 5;
    int percentModifier = 5;
    float percentCity = .025f;
    int cityNum = 0;
    int cityCounter = 0;
    public GameObject[] land;
    int landCounter = 0;
    bool landCreated = false;
    int allOceanNum = 0;
    int maxAllOceanNum = 550;

    int tileBiomeNum = 0;
    int[] biomeNums = new int[8];//mountain, desert, plains, valley, hills,marsh,forest,tundra

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
        testInfoHolder = GameObject.Find("testInfoHolder");
        landMasses = new List<continents>();
        foreach (int i in biomeNums)
        {
            biomeNums[i] = 0;
        }
        int newPercentLand = (int)((float)(height * width) * percentLand);
        land = new GameObject[(int)newPercentLand];
        for (int i = 0; i < height; i++)
        {
            tiles[i] = new GameObject[width];
            for (int j = 0; j < width; j++)
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
                tiles[i][j] = temp;
                float num = Random.Range(0f, (float)(height*width));
                if (num <= percentLandSeed && land.Length > landCounter)
                {
                    temp.GetComponent<hexTile2>().setTerrain(hexTile2.biomes.MOUNTAIN);
                    land[landCounter] = temp;
                    landCounter++;
                    continents newContinent = new continents();
                    newContinent.lands.Add(temp.GetComponent<hexTile2>());
                    landMasses.Add(newContinent);
                }
                else
                {
                    temp.GetComponent<hexTile2>().setTerrain(hexTile2.biomes.OCEAN);
                }
            }
        }
        if (landCounter < minLandSeeds)
        {
            moreLandSeeds();
        }
        maxHeight = tiles[height - 1][width - 1].transform.position.z;
        maxWidth = tiles[height - 1][width - 1].transform.position.x;
        cityNum = (int)((float)(height * width)*(float)(percentCity));
        cwidth = width;
        cMaxHeight = maxHeight;
        cMaxWidth = maxWidth;
        done = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (tiles[height - 1][width - 1].GetComponent<hexTile2>().searched && !landCreated)
        {
            //establishBorderNeighbors();
            generateContinents();
            landCreated = true;
            checkOceanSize();
            createBiomes();
            clearIslandsSetCoasts();
            setModifiers();
            setCitys();
            createExtraMaps();
        }
	}
    void checkOceanSize()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
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
        testInfoHolder.GetComponent<testInfo>().itterationNum ++;
        testInfoHolder.GetComponent<testInfo>().newMap();
    }
    void moreLandSeeds()
    {
        while (landCounter < minLandSeeds)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if(!contains(tiles[y][x], land))
            {
                tiles[y][x].GetComponent<hexTile2>().setTerrain(hexTile2.biomes.MOUNTAIN);
                land[landCounter] = tiles[y][x];
                landCounter++;
                continents newContinent = new continents();
                newContinent.lands.Add(tiles[y][x].GetComponent<hexTile2>());
                landMasses.Add(newContinent);
            }
        }
    }
    void setCitys()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
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
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
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
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c1";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i][j + width-5], new Vector3(tiles[i][j + width-5].transform.position.x, tiles[i][j + width-5].transform.position.y, tiles[i][j + width-5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c2";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c3";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + height-5][j], new Vector3(tiles[i + height-5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + height-5][j].transform.position.y, tiles[i + height-5][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
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
                temp = Instantiate(tiles[i + height - 5][j], new Vector3(tiles[i + height - 5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + height - 5][j].transform.position.y, tiles[i + height - 5][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c6";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i][j + width - 5], new Vector3(tiles[i][j + width - 5].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j + width - 5].transform.position.y, tiles[i][j + width - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c7";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + height - 5][j + width - 5], new Vector3(tiles[i + height - 5][j + width - 5].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + height - 5][j + width - 5].transform.position.y, tiles[i + height - 5][j + width - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c8";
                temp.GetComponent<hexTile2>().setBorder();
                temp.GetComponent<hexTile2>().modLayer.SetActive(false);
                temp.GetComponent<hexTile2>().buildingLayer.SetActive(false);
            }
        }
    }
    /*void establishBorderNeighbors()
    {
        for(int i=0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                if (j == width-1)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[i][0];
                    if (i % 2 != 0 && i != height-1)
                    {
                        tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[i - 1][0];
                        tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[i + 1][0];
                    }
                }
                if (j == 0)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[i][width-1];
                    if (i % 2 == 0 && i != 0)
                    {
                        tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[i - 1][width - 1];
                        tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[i + 1][width - 1];
                    }
                }
                if (i == 0 && (j != 0 && j != width - 1))
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[height - 1][j];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[height - 1][j - 1];
                }
                if (i == height - 1 && (j != 0 && j != width - 1))
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[0][j];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[5] = tiles[0][j + 1];
                }
                if (i == 0 && j == 0)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[2] = tiles[1][width - 1];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[height - 1][width - 1];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[height - 1][0];
                }
                if (i == height - 1 && j == width - 1)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[2] = tiles[height - 2][0];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[0][0];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[0][width - 1];
                }
                if (i == height - 1 && j == 0)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[0][0];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[0][1];
                }
                if (i == 0 && j == width - 1)
                {
                    tiles[i][j].GetComponent<hexTile2>().neighbors[3] = tiles[height - 1][width - 1];
                    tiles[i][j].GetComponent<hexTile2>().neighbors[4] = tiles[height - 1][width - 2];
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
            if (temp.neighbors[i].GetComponent<hexTile2>().isOcean && !temp.isOcean)
            {
                temp.isCoast = true;
                break;
            }
        }
    }
    void clearIslandsSetCoasts()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
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
    void createBiomes()
    {
        float percentBiome = .125f;
        tileBiomeNum = (int)(((float)landCounter * percentBiome)+(height*width*percentBiome*percentLand*.4f));
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                hexTile2 temp = tiles[i][j].GetComponent<hexTile2>();
                if (!temp.isChecked && !temp.isOcean)
                {
                    selectBiome(tiles[i][j]);
                }
            }
        }
    }
    void selectBiomeAlt(GameObject start)
    {
        int biome = Random.Range(0, 8);
        if (biomeNums[0] != tileBiomeNum && biome == 0)
        {
            genBiome(start, hexTile2.biomes.MOUNTAIN);
        }
        else if (biomeNums[1] != tileBiomeNum && biome == 1)
        {
            genBiome(start, hexTile2.biomes.DESERT);
        }
        else if (biomeNums[2] != tileBiomeNum && biome == 2)
        {
            genBiome(start, hexTile2.biomes.PLAINS);
        }
        else if (biomeNums[3] != tileBiomeNum && biome == 3)
        {
            genBiome(start, hexTile2.biomes.VALLEY);
        }
        else if (biomeNums[4] != tileBiomeNum && biome == 4)
        {
            genBiome(start, hexTile2.biomes.HILLS);
        }
        else if (biomeNums[5] != tileBiomeNum && biome == 5)
        {
            genBiome(start, hexTile2.biomes.MOUNTAIN);
        }
        else if (biomeNums[6] != tileBiomeNum && biome == 6)
        {
            genBiome(start, hexTile2.biomes.FOREST);
        }
        else if (biomeNums[7] != tileBiomeNum && biome == 7)
        {
            genBiome(start, hexTile2.biomes.TUNDRA);
        }
        else
        {
            selectBiomeAlt(start);
        }
       

    }
    void selectBiome(GameObject start)
    {
        biomeSizeHolder = new List<hexTile2>();
        int biome = Random.Range(0, 8);
        switch (biome)
        {
            case 0:
                if (biomeNums[0] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.MOUNTAIN);
                break;
            case 1:
                if (biomeNums[1] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.DESERT);
                break;
            case 2:
                if (biomeNums[2] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.PLAINS);
                break;
            case 3:
                if (biomeNums[3] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.VALLEY);
                break;
            case 4:
                if (biomeNums[4] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.HILLS);
                break;
            case 5:
                if (biomeNums[5] >= tileBiomeNum)
                {
                    selectBiomeAlt(start); 
                    break;
                }
                genBiome(start, hexTile2.biomes.MARSHES);
                break;
            case 6:
                if (biomeNums[6] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.FOREST);
                break;
            case 7:
                if (biomeNums[7] >= tileBiomeNum)
                {
                    selectBiomeAlt(start);
                    break;
                }
                genBiome(start, hexTile2.biomes.TUNDRA);
                break;
        }
    }
    void genBiome(GameObject start, hexTile2.biomes biome)
    {
        hexTile2 temp = start.GetComponent<hexTile2>();
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
                    selectBiome(acceptableNeighbors[neighbor]);
                    return;
                }
                genBiome(acceptableNeighbors[neighbor], biome);
            }
        }
    }
    void generateContinents()
    {
        for (int i = 0; i < land.Length; i++)
        {
            hexTile2 temp = land[i].GetComponent<hexTile2>();
            GameObject[] neighbors = new GameObject[6];
            neighbors = temp.neighbors;
            float num = Random.Range(0f, (float)temp.counter); 
            for (int j = 0; j < num; j++)
            {
                bool neighborContinent = oonOtherContinent(neighbors[j], temp);
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
                if (!contains(neighbors[j], land) && addIfNeighborContinent)
                {
                    land = addToBack(neighbors[j], land);
                    neighbors[j].GetComponent<hexTile2>().setTerrain(hexTile2.biomes.MOUNTAIN);
                    landCounter++;
                    addToContinent(neighbors[j], temp);
                }
            }
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
    bool oonOtherContinent(GameObject obj, hexTile2 origional)
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
