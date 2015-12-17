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
    public Material desert;
    public Material plains;
    public Material valley;
    public Material hills;
    public Material marsh;
    public Material forest;
    public Material tundra;

    public Material volcano;
    public Material highWind;
    public Material denseTrees;
    public Material hotSpot;
    public Material coal;
    public Material naturalGas;
    public Material earthQuake;
    public Material river;

    public Material border;

    public Material city;

    public bool isOcean = false;
    public bool isLand = false;
    //isMountain, isDesert, isPlains, isValley, isHills, isMarshes, isForest, isTundra
    public bool[] biomeTypes = { false, false, false, false, false, false, false, false };
    //hasVolcano, hasHighWinds, hasDenseTrees, hasHotSpot, hasCoal, hasNaturalGas, hasEarthQuake, hasRiver
    public bool[] modifierTypes = { false, false, false, false, false, false, false, false };


    //public bool isMountain = false;
    //public bool isDesert = false;
    //public bool isPlains = false;
    //public bool isValley = false;
    //public bool isHills = false;
    //public bool isMarshes = false;
    //public bool isForest = false;
    //public bool isTundra = false;
    
    public bool isCoast = false;
    public bool hasCity = false;

    public bool isChecked = false;
    public bool isMouseOver = false;
    public bool isSelected = false;

    
    //public bool hasVolcano = false;
    //public bool hasHighWinds = false;
    //public bool hasDenseTrees = false;
    //public bool hasHotSpot = false;
    //public bool hasCoal = false;
    //public bool hasNarutalGas = false;
    //public bool hasEarthQuake = false;
    //public bool hasRiver = false;

    
    
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
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].SetActive(false);
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
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            zoom();
        }
        if (isSelected)
        {
            for (int i = 0; i < borders.Length; i++)
            {
                borders[i].gameObject.GetComponent<Renderer>().material = selected;
            }
        }
	}
    public void zoom()
    {
        if (mainCamera.GetComponent<cameraControl>().currTile != null)
        {
            mainCamera.GetComponent<cameraControl>().currTile.GetComponent<hexManagement>().close();
        }
        menuCamera.enabled = true;
        mainCamera.rect = new Rect(0f, .5f, 1f, 2f);
        cameraDistance = mainCamera.gameObject.transform.position.y;
        mainCamera.GetComponent<cameraControl>().cameraDistance = 20;
        mainCamera.gameObject.transform.position = new Vector3(this.transform.position.x, 20, this.transform.position.z);
        this.GetComponent<hexManagement>().show = true;
        isSelected = true;
        mainCamera.GetComponent<cameraControl>().canControl = false;
        mainCamera.GetComponent<cameraControl>().currTile = this.gameObject;
    }
    public void setTerrain(int i)
    {
        isOcean = false;
        for (int j = 0; j < biomeTypes.Length; j++)
        {
            biomeTypes[j] = false;
        }
        //isMountain = false;
        //isDesert = false;
        //isPlains = false;
        //isValley = false;
        //isHills = false;
        //isMarshes = false;
        //isForest = false;
        //isTundra = false;
        switch (i)
        {
            case 1:
                this.GetComponent<Renderer>().material = water;
                isOcean = true;
                break;
            case 2:
                this.GetComponent<Renderer>().material = mountain;
                biomeTypes[0] = true;
                break;
            case 3:
                this.GetComponent<Renderer>().material = desert;
                biomeTypes[1] = true;
                break;
            case 4:
                this.GetComponent<Renderer>().material = plains;
                biomeTypes[2] = true;
                break;
            case 5:
                this.GetComponent<Renderer>().material = valley;
                biomeTypes[3] = true;
                break;
            case 6:
                this.GetComponent<Renderer>().material = hills;
                biomeTypes[4] = true;
                break;
            case 7:
                this.GetComponent<Renderer>().material = marsh;
                biomeTypes[5] = true;
                break;
            case 8:
                this.GetComponent<Renderer>().material = forest;
                biomeTypes[6] = true;
                break;
            case 9:
                this.GetComponent<Renderer>().material = tundra;
                biomeTypes[7] = true;
                break;
        }
    }

    public void setBorder()
    {
        this.gameObject.GetComponent<Renderer>().material = border;
        Destroy(this);
    }

    public void setModifier(int i)
    {
        for (int j = 0; j < modifierTypes.Length; j++)
        {
            modifierTypes[j] = false;
        }
        //hasVolcano = false;
        //hasHotSpot = false;
        //hasHighWinds = false;
        //hasRiver = false;
        //hasEarthQuake = false;
        //hasNarutalGas = false;
        //hasDenseTrees = false;
        //hasCoal = false;
        switch (i)
        {
            case 1:
                this.GetComponent<Renderer>().material = volcano;
                modifierTypes[0] = true;
                break;
            case 2:
                this.GetComponent<Renderer>().material = hotSpot;
                modifierTypes[1] = true;
                break;
            case 3:
                this.GetComponent<Renderer>().material = highWind;
                modifierTypes[2] = true;
                break;
            case 4:
                this.GetComponent<Renderer>().material = river;
                modifierTypes[3] = true;
                break;
            case 5:
                this.GetComponent<Renderer>().material = earthQuake;
                modifierTypes[4] = true;
                break;
            case 6:
                this.GetComponent<Renderer>().material = naturalGas;
                modifierTypes[5] = true;
                break;
            case 7:
                this.GetComponent<Renderer>().material = denseTrees;
                modifierTypes[6] = true;
                break;
            case 8:
                this.GetComponent<Renderer>().material = coal;
                modifierTypes[7] = true;
                break;
        }
    }
    public void setCity()
    {
        this.GetComponent<Renderer>().material = city;
        hasCity = true;
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
        mainCamera.GetComponent<cameraControl>().cameraDistance = mainCamera.GetComponent<cameraControl>().tempCameradistance;
        //mainCamera.GetComponent<cameraControl>().tempCameradistance = 0;
        mainCamera.GetComponent<cameraControl>().canControl = true;
        mainCamera.GetComponent<cameraControl>().currTile = null;
        OnMouseExit();
    }
    public void removeScript()
    {
        Destroy(this);
    }
}
