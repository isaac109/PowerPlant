using UnityEngine;
using System.Collections;

public class hexTile2 : MonoBehaviour {

    public GameObject[] borders = new GameObject[6];
    public Material normal;
    public Material highlighted;
   
    
    public int counter = 0;
    public gridManager gm;
    public bool searched = false;
    public Collider[] objects;
    public GameObject[] neighbors = new GameObject[6];


    public Material mountain;
    public Material water;

    public bool isOcean = false;
    public bool isMountain = false;
    public bool isChecked = false;
	// Use this for initialization
	void Start () {
        this.gameObject.tag = "Tile";
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].gameObject.GetComponent<Renderer>().material = normal;
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.done && !searched)
        {
            objects = Physics.OverlapSphere(this.transform.position, 7f);
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].gameObject.tag == "Tile" && objects[i].gameObject.name != this.gameObject.name)
                {
                    neighbors[counter] = objects[i].gameObject;
                    counter++;
                }
            }
            searched = true;
        }
	}

    public void setTerrain(int i)
    {
        switch (i)
        {
            case 1:
                this.GetComponent<Renderer>().material = water;
                isOcean = true;
                break;
            case 2:
                this.GetComponent<Renderer>().material = mountain;
                isMountain = true;
                break;
        }
    }

    void OnMouseOver()
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].gameObject.GetComponent<Renderer>().material = highlighted;
        }
    }
    void OnMouseExit()
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].gameObject.GetComponent<Renderer>().material = normal;
        }
    }
}
