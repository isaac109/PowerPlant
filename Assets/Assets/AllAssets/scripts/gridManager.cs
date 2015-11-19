using UnityEngine;
using System.Collections;

public class gridManager : MonoBehaviour {

    public GameObject hex;
    public bool done = false;
    static int width = 30;
    static int height = 60;
    public GameObject[][] tiles = new GameObject[height][];

    float percentLand = .5f;
    float percentLandSeed = 10;
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
	// Use this for initialization
	void Start () {
        percentLand = (int)((float)(height * width) * percentLand);
        land = new GameObject[(int)percentLand];
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
        done = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (tiles[height - 1][width - 1].GetComponent<hexTile2>().searched && !landCreated)
        {
            createOcean();
            landCreated = true;
            createLand();
        }
	}
    void createLand()
    {
        float percentBiome = .125f;
        tileBiomeNum = (int)((float)(width*height)*percentBiome);
        Debug.Log(tileBiomeNum);
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
    void generateBiome(GameObject start)
    {
        
        int biome = Random.Range(0, 7);
        biome = 1;
        switch (biome)
        {
            case 0:
                if (mountainNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genMou(start);
                break;
            case 1:
                if (desertNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genDes(start);
                break;
            case 2:
                if (plainsNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genPla(start);
                break;
            case 3:
                if (valleyNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genVal(start);
                break;
            case 4:
                if (hillsNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genHi(start);
                break;
            case 5:
                if (marshNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genMar(start);
                break;
            case 6:
                if (forestNum == tileBiomeNum)
                {
                    //generateBiome(start);
                    break;
                }
                genFor(start);
                break;
            case 7:
                if (tundraNum == tileBiomeNum)
                {
                    //generateBiome(start);
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                genMou(acceptableNeighbors[neighbor]);
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                genMou(acceptableNeighbors[neighbor]);
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                genMou(acceptableNeighbors[neighbor]);
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                genMou(acceptableNeighbors[neighbor]);
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                genMou(acceptableNeighbors[neighbor]);
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
                while (selectedNeighbors[neighbor] == false)
                {
                    neighbor = Random.Range(0, neighborNum);
                }
                selectedNeighbors[neighbor] = true;
                genMou(acceptableNeighbors[neighbor]);
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
