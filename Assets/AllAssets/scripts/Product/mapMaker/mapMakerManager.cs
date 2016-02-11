using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapMakerManager : MonoBehaviour {

    public GameObject hex;
    public bool done = false;
    static int _height = 30;
    static int _width = 60;
    public int cwidth = 0;
    public GameObject[][] tiles = new GameObject[_width][];

    public List<GameObject> land;
    int landCounter = 0;
    bool landCreated = false;
   
    int tileBiomeNum = 0;
    int[] biomeNums = new int[8];//mountain, desert, plains, valley, hills,marsh,forest,tundra

    List<GameObject> polarWater;
    float percentFrozenCaps = .5f;
    float environmentHealth = 1f;
    public int maxNumFrozenCaps;
    public int numFrozenCaps;

    public float maxHeight = 0;
    public float maxWidth = 0;
    public float cMaxHeight = 0;
    public float cMaxWidth = 0;

    void Start()
    {
        Random.seed = (int)System.DateTime.Now.Ticks;
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
                    temp = Instantiate(hex, new Vector3((5 * Mathf.Sqrt(3) * i), 0, (10 * j)), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                }
                else
                {
                    temp = Instantiate(hex, new Vector3((5 * Mathf.Sqrt(3) * i), 0, (5 + (10 * j))), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                }
                temp.name = "hex" + i.ToString() + "00" + j.ToString();
                temp.GetComponent<mapMakerTile>().gm = this.GetComponent<mapMakerManager>();
                temp.GetComponent<mapMakerTile>().heightInArray = j;
                temp.GetComponent<mapMakerTile>().widthInArray = i;
                tiles[i][j] = temp;
                temp.GetComponent<mapMakerTile>().setTerrain(mapMakerTile.biomes.OCEAN);

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
	void Update () {
        if (!landCreated && tiles[_width - 1][_height - 1].GetComponent<mapMakerTile>().searched)
        {
            landCreated = true;
            createExtraMaps();
        }
	}

    public void changeSelection()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                tiles[i][j].GetComponent<mapMakerTile>().isSelected =
                    !tiles[i][j].GetComponent<mapMakerTile>().isSelected;
            }
        }
    }

    void createExtraMaps()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c1";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i][j + _height - 5], new Vector3(tiles[i][j + _height - 5].transform.position.x, tiles[i][j + _height - 5].transform.position.y, tiles[i][j + _height - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c2";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c3";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + _width - 5][j], new Vector3(tiles[i + _width - 5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + _width - 5][j].transform.position.y, tiles[i + _width - 5][j].transform.position.z), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c4";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject temp = Instantiate(tiles[i][j], new Vector3(tiles[i][j].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j].transform.position.y, tiles[i][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c5";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + _width - 5][j], new Vector3(tiles[i + _width - 5][j].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + _width - 5][j].transform.position.y, tiles[i + _width - 5][j].transform.position.z + maxHeight + 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c6";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i][j + _height - 5], new Vector3(tiles[i][j + _height - 5].transform.position.x + 5 * Mathf.Sqrt(3) + maxWidth, tiles[i][j + _height - 5].transform.position.y, tiles[i][j + _height - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c7";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
                temp = Instantiate(tiles[i + _width - 5][j + _height - 5], new Vector3(tiles[i + _width - 5][j + _height - 5].transform.position.x - 5 * Mathf.Sqrt(3) - maxWidth, tiles[i + _width - 5][j + _height - 5].transform.position.y, tiles[i + _width - 5][j + _height - 5].transform.position.z - maxHeight - 5), Quaternion.Euler(new Vector3(270, 180, 0))) as GameObject;
                temp.name = "hex" + i.ToString() + "00" + j.ToString() + "c8";
                temp.GetComponent<mapMakerTile>().setBorder();
                temp.GetComponent<mapMakerTile>().modLayer.SetActive(false);
                temp.GetComponent<mapMakerTile>().buildingLayer.SetActive(false);
            }
        }
    }
}
