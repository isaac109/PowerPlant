using UnityEngine;
using System.Collections;

public class gridManager : MonoBehaviour {

    public GameObject hex;
    public bool done = false;
    static int width = 30;
    static int height = 60;
    public int cwidth = 0;
    public GameObject[][] tiles = new GameObject[height][];

    float percentLand = .5f;
    float percentLandSeed = 10;
    int percentModifier = 5;
    float percentCity = .025f;
    int cityNum = 0;
    int cityCounter = 0;
    public GameObject[] land;
    int landCounter = 0;
    bool landCreated = false;

    int tileBiomeNum = 0;
    int mountainNum = 0;
    int desertNum = 0;
    int plainsNum = 0;
    int valleyNum = 0;
    int hillsNum = 0;
    int marshNum = 0;
    int forestNum = 0;
    int tundraNum = 0;

    public float maxHeight = 0;
    public float maxWidth = 0;
	// Use this for initialization
	void Start () {
        int newPercentLand = (int)((float)(height * width) * percentLand);
        Debug.Log("land" + percentLand);
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
                    temp.GetComponent<hexTile2>().setTerrain(2);
                    land[landCounter] = temp;
                    landCounter++;
                }
                else
                {
                    temp.GetComponent<hexTile2>().setTerrain(1);
                }
            }
        }
        maxHeight = tiles[height - 1][width - 1].transform.position.z;
        maxWidth = tiles[height - 1][width - 1].transform.position.x;
        cityNum = (int)((float)(height * width)*(float)(percentCity));
        Debug.Log("cityNum " + cityNum.ToString());
        cwidth = width;
        done = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (tiles[height - 1][width - 1].GetComponent<hexTile2>().searched && !landCreated)
        {
            establishBorderNeighbors();
            createOcean();
            landCreated = true;
            createLand();
            clearIslands();
            setCostal();
            setModifiers();
            setCitys();
            createExtraMaps();
        }
	}
    void setCitys()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (!tiles[i][j].GetComponent<hexTile2>().hasCity && cityCounter < cityNum && !tiles[i][j].GetComponent<hexTile2>().isOcean)
                {
                    int r = Random.Range(0, 99);
                    if (r <= percentCity*100)
                    {
                        tiles[i][j].GetComponent<hexTile2>().setCity();
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
                if (!tiles[i][j].GetComponent<hexTile2>().isOcean)
                {
                    int mod = Random.Range(1, 9);
                    int r = Random.Range(0, 100);
                    if (r <= percentModifier)
                    {
                        tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                    }
                    else if (r <= 3 * percentModifier)
                    {
                        if (mod == 1 && tiles[i][j].GetComponent<hexTile2>().isMountain)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 2 && tiles[i][j].GetComponent<hexTile2>().isDesert)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 3 && tiles[i][j].GetComponent<hexTile2>().isPlains)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 4 && tiles[i][j].GetComponent<hexTile2>().isValley)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 5 && tiles[i][j].GetComponent<hexTile2>().isPlains)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 6 && tiles[i][j].GetComponent<hexTile2>().isMarshes)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 7 && tiles[i][j].GetComponent<hexTile2>().isForest)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
                        }
                        if (mod == 8 && tiles[i][j].GetComponent<hexTile2>().isTundra)
                        {
                            tiles[i][j].GetComponent<hexTile2>().setModifier(mod);
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
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i][j];
                temp = Instantiate(tiles[i][j + width-5], new Vector3(tiles[i][j + width-5].transform.position.x, tiles[i][j + width-5].transform.position.y, tiles[i][j + width-5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c2";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i][j + width-5];
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < width; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c3";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i][j];
                temp = Instantiate(tiles[i + height-5][j], new Vector3(tiles[i + height-5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + height-5][j].transform.position.y, tiles[i + height-5][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c4";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i + height - 5][j];
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c5";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i][j];
                temp = Instantiate(tiles[i + height - 5][j], new Vector3(tiles[i + height - 5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + height - 5][j].transform.position.y, tiles[i + height - 5][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c6";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i+height-5][j];
                temp = Instantiate(tiles[i][j + width - 5], new Vector3(tiles[i][j + width - 5].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j + width - 5].transform.position.y, tiles[i][j + width - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c7";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i][j+width-5];
                temp = Instantiate(tiles[i + height - 5][j + width - 5], new Vector3(tiles[i + height - 5][j + width - 5].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + height - 5][j + width - 5].transform.position.y, tiles[i + height - 5][j + width - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c8";
                temp.GetComponent<hexTile2>().removeScript();
                temp.AddComponent<mockHex>();
                temp.GetComponent<mockHex>().realHex = tiles[i+height-5][j+width-5];
            }
        }
    }
    void establishBorderNeighbors()
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
    }
    void setCostal()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                checkIfCoast(tiles[i][j]);
            }
        }
    }
    void checkIfCoast(GameObject tile)
    {
        for (int i = 0; i < tile.GetComponent<hexTile2>().counter; i++)
        {
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean && !tile.GetComponent<hexTile2>().isOcean)
            {
                tile.GetComponent<hexTile2>().isCoast = true;
                break;
            }
        }
    }
    void clearIslands()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                checkIfIsland(tiles[i][j]);
            }
        }
    }
    void checkIfIsland(GameObject tile)
    {
        int numOfMou = 0;
        int numOfDes = 0;
        int numOfPla = 0;
        int numOfVal = 0;
        int numOfHil = 0;
        int numOfMar = 0;
        int numOfFor = 0;
        int numOfTun = 0;
        for (int i = 0; i < tile.GetComponent<hexTile2>().counter; i++)
        {
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isMountain)
            {
                numOfMou++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isDesert)
            {
                numOfDes++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isPlains)
            {
                numOfPla++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isValley)
            {
                numOfVal++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isHills)
            {
                numOfHil++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isMarshes)
            {
                numOfMar++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isForest)
            {
                numOfFor++;
            }
            if (tile.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isTundra)
            {
                numOfTun++;
            }
        }
        if ((tile.GetComponent<hexTile2>().isMountain && numOfMou == 0) ||
            (tile.GetComponent<hexTile2>().isDesert && numOfDes == 0) ||
            (tile.GetComponent<hexTile2>().isPlains && numOfPla == 0) ||
            (tile.GetComponent<hexTile2>().isValley && numOfVal == 0) ||
            (tile.GetComponent<hexTile2>().isHills && numOfHil == 0) ||
            (tile.GetComponent<hexTile2>().isMarshes && numOfMar == 0) ||
            (tile.GetComponent<hexTile2>().isForest && numOfFor == 0) ||
            (tile.GetComponent<hexTile2>().isTundra && numOfTun == 0))
        {
            int num = Random.Range(1,(numOfMou+numOfDes+numOfPla+numOfVal+numOfHil+numOfMar+numOfFor+numOfTun+1));
            if (num <= numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(2);
            }
            else if (num <= numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(3);
            }
            else if (num <= numOfPla + numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(4);
            }
            else if (num <= numOfVal + numOfPla + numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(5);
            }
            else if (num <= numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(6);
            }
            else if (num <= numOfMar + numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(7);
            }
            else if (num <= numOfFor + numOfMar + numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(8);
            }
            else if (num <= numOfTun + numOfFor + numOfMar + numOfHil + numOfVal + numOfPla + numOfDes + numOfMou)
            {
                tile.GetComponent<hexTile2>().setTerrain(9);
            }
            else
            {
                Debug.Log("Something went wrong");
            }
        }
    }
    void createLand()
    {
        float percentBiome = .125f;
        tileBiomeNum = (int)((float)landCounter * percentBiome)+100;
        Debug.Log( " num " + tileBiomeNum);
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (!tiles[i][j].GetComponent<hexTile2>().isChecked && !tiles[i][j].GetComponent<hexTile2>().isOcean)
                {
                    generateBiome(tiles[i][j]);
                }
            }
        }
    }
    void generateBiomeAlt(GameObject start)
    {
        if (mountainNum != tileBiomeNum)
        {
            genMou(start);
        }
        else if (desertNum != tileBiomeNum)
        {
            genDes(start);
        }
        else if (plainsNum != tileBiomeNum)
        {
            genPla(start);
        }
        else if (valleyNum != tileBiomeNum)
        {
            genVal(start);
        }
        else if (hillsNum != tileBiomeNum)
        {
            genHi(start);
        }
        else if (marshNum != tileBiomeNum)
        {
            genMar(start);
        }
        else if (forestNum != tileBiomeNum)
        {
            genFor(start);
        }
        else if (tundraNum != tileBiomeNum)
        {
            genTun(start);
        }
       

    }
    void generateBiome(GameObject start)
    {

        int biome = Random.Range(0, 8);
        switch (biome)
        {
            case 0:
                if (mountainNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genMou(start);
                break;
            case 1:
                if (desertNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genDes(start);
                break;
            case 2:
                if (plainsNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genPla(start);
                break;
            case 3:
                if (valleyNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genVal(start);
                break;
            case 4:
                if (hillsNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genHi(start);
                break;
            case 5:
                if (marshNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start); 
                    break;
                }
                genMar(start);
                break;
            case 6:
                if (forestNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genFor(start);
                break;
            case 7:
                if (tundraNum >= tileBiomeNum)
                {
                    generateBiomeAlt(start);
                    break;
                }
                genTun(start);
                break;
        }
    }
    void genMou(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(2);
        mountainNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for(int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if(!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (mountainNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genMou(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genDes(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(3);
        desertNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (desertNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genDes(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genPla(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(4);
        plainsNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (plainsNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genPla(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genVal(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(5);
        valleyNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (valleyNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genVal(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genHi(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(6);
        hillsNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (hillsNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genHi(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genMar(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(7);
        marshNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (marshNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genMar(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genFor(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(8);
        forestNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (forestNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genFor(acceptableNeighbors[neighbor]);
            }
        }
    }
    void genTun(GameObject start)
    {
        start.GetComponent<hexTile2>().setTerrain(9);
        tundraNum++;
        start.GetComponent<hexTile2>().isChecked = true;
        GameObject[] acceptableNeighbors = new GameObject[6];
        int neighborNum = 0;
        for (int i = 0; i < start.GetComponent<hexTile2>().counter; i++)
        {
            if (!start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isChecked && !start.GetComponent<hexTile2>().neighbors[i].GetComponent<hexTile2>().isOcean)
            {
                acceptableNeighbors[neighborNum] = start.GetComponent<hexTile2>().neighbors[i];
                neighborNum++;
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
                neighbor = Random.Range(0, neighborNum);
                while (selectedNeighbors[neighbor] == true)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                if (tundraNum == tileBiomeNum)
                {
                    generateBiome(acceptableNeighbors[neighbor]);
                    break;
                }
                genTun(acceptableNeighbors[neighbor]);
            }
        }
    }
    void createOcean()
    {
        for (int i = 0; i < land.Length; i++)
        {
            GameObject[] neighbors = new GameObject[6];
            neighbors = land[i].GetComponent<hexTile2>().neighbors;
            float num = Random.Range(0f, (float)land[i].GetComponent<hexTile2>().counter); 
            for (int j = 0; j < num; j++)
            {
                if (!contains(neighbors[j], land))
                {
                    land = addToBack(neighbors[j], land);
                    neighbors[j].GetComponent<hexTile2>().setTerrain(2);
                    landCounter++;
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
}
