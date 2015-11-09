using UnityEngine;
using System.Collections;

public class gridManager : MonoBehaviour {

    public GameObject hex;
    public bool done = false;
    static int width = 30;
    static int height = 60;
    public GameObject[][] tiles = new GameObject[height][];

    float percentLand = .25f;
    float percentLandSeed = 10;
    public GameObject[] land;
    int landCounter = 0;
    bool landCreated = false;
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
