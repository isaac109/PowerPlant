using UnityEngine;
using System.Collections;

public class hexTile2 : MonoBehaviour {

    public GameObject[] borders = new GameObject[6];
    public Material normal;
    public Material highlighted;
    public Material selected;
   
    
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
    public bool isMouseOver = false;
    public bool isSelected = false;
    
    public Camera mainCamera;
    public Camera menuCamera;
    public float cameraDistance;
	// Use this for initialization
	void Start () {
        this.gameObject.tag = "Tile";
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].gameObject.GetComponent<Renderer>().material = normal;
        }
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        menuCamera = GameObject.Find("Camera").GetComponent<Camera>();
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
        if (isMouseOver && Input.GetMouseButtonDown(0) && menuCamera.enabled == false)
        {
            menuCamera.enabled = true;
            mainCamera.rect = new Rect(0f, .5f, 1f, 2f);
            cameraDistance = mainCamera.gameObject.transform.position.y;
            mainCamera.gameObject.transform.position = new Vector3(this.transform.position.x, 20, this.transform.position.z);
            this.GetComponent<hexManagement>().show = true;
            isSelected = true;
            mainCamera.GetComponent<cameraControl>().canControl = false;
        }
        if (isSelected)
        {
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].gameObject.GetComponent<Renderer>().material = selected;
            }
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
        isMouseOver = true;
    }
    void OnMouseExit()
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].gameObject.GetComponent<Renderer>().material = normal;
        }
        isMouseOver = false;
    }
    public void closeCameras()
    {
        menuCamera.enabled = false;
        mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
        this.GetComponent<hexManagement>().show = false;
        isSelected = false;
        mainCamera.GetComponent<cameraControl>().cameraDistance = cameraDistance;
        mainCamera.GetComponent<cameraControl>().canControl = true;
        OnMouseExit();
    }
}
