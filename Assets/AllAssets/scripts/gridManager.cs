using UnityEngine;
using System.Collections;

public class gridManager : MonoBehaviour {

    public GameObject hex;
    public bool done = false;
    static int width = 30;
    static int height = 60;
    public GameObject[][] tiles = new GameObject[height][];

    public float percentOcean = .5f;
    public float percentOceanSeed = 10;
    public GameObject[] ocean;
    int oceanCounter = 0;
    bool oceanCreated = false;
	// Use this for initialization
	void Start () {
        percentOcean = (float)(height * width) * percentOcean / 100;
        ocean = new GameObject[(int) percentOcean];
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
                if (num <= percentOceanSeed)
                {
                    temp.GetComponent<hexTile2>().setTerrain(1);
                    ocean[oceanCounter] = temp;
                    oceanCounter++;
                }
                else
                {
                    temp.GetComponent<hexTile2>().setTerrain(2);
                }
            }
        }
        done = true;
	}

    // Update is called once per frame
    void Update()
    {
        if (tiles[height - 1][width - 1].GetComponent<hexTile2>().searched && !oceanCreated)
        {
            createOcean();
            oceanCreated = true;
        }
	}
    void createOcean()
    {
        for (int i = 0; i < ocean.Length; i++)
        {
            GameObject[] neighbors = new GameObject[6];
            neighbors = ocean[i].GetComponent<hexTile2>().neighbors;
            float num = Random.Range(0f, (float)ocean[i].GetComponent<hexTile2>().counter); 
            for (int j = 0; j < num; j++)
            {
                if (!contains(neighbors[j], ocean))
                {
                    ocean = addToBack(neighbors[j], ocean);
                    neighbors[j].GetComponent<hexTile2>().setTerrain(1);
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
