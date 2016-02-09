using UnityEngine;
using System.Collections;

public class hexTile2 : MonoBehaviour {

    public GameObject canvas;
    public GameObject[] borders = new GameObject[6];
    public GameObject modLayer;
    public GameObject buildingLayer;
    public Material normal;
    public Material highlighted;
    public Material selected;
   
    
    public int counter = 0;
    public gridManager gm;
    public bool searched = false;
    public Collider[] objects;
    public GameObject[] neighbors = new GameObject[6];
    public Material[] baseTiles = new Material[11];
    public Material[] modTiles = new Material[8];

    public Material border;

    public Material city;

    public bool isOcean = false;
    public bool isLand = false;
    public bool isFrozen = false;

    //isMountain, isDesert, isPlains, isValley, isHills, isMarshes, isForest, isTundra
    public bool[] biomeTypes = { false, false, false, false, false, false, false, false };
    //hasVolcano, hasHotSpot, hasHighWinds, hasRiver, hasEarthquake, hasNaturalGas, hasDenseTrees, hasCoal
    public bool[] modifierTypes = { false, false, false, false, false, false, false, false };
    
    public bool isCoast = false;
    public bool hasCity = false;

    public bool isChecked = false;
    public bool isMouseOver = false;
    public bool isSelected = false;

    
    
    public Camera mainCamera;
    public Camera menuCamera;
    public float cameraDistance;

    public int heightInArray;
    public int widthInArray;

    public enum biomes
    {
        OCEAN,
        MOUNTAIN,
        DESERT,
        PLAINS,
        VALLEY,
        HILLS,
        MARSHES,
        FOREST,
        TUNDRA,
        GLACIER,
        ICEBERG
    }

    public enum modifiers
    {
        VOLCANO,
        HOT_SPOT,
        HIGH_WIND,
        RIVER,
        EARTHQUAKE,
        NATURAL_GAS,
        DENSE_TREES,
        COAL
    }
	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
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
        if (isMouseOver && Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
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
        canvas.SetActive(false);
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
    public void setTerrain(biomes biome)
    {
        isOcean = false;
        isFrozen = false;
        for (int j = 0; j < biomeTypes.Length; j++)
        {
            biomeTypes[j] = false;
        }
        switch (biome)
        {
            case biomes.OCEAN:
                this.GetComponent<Renderer>().material = baseTiles[0];
                isOcean = true;
                break;
            case biomes.MOUNTAIN:
                this.GetComponent<Renderer>().material = baseTiles[1];
                biomeTypes[0] = true;
                break;
            case biomes.DESERT:
                this.GetComponent<Renderer>().material = baseTiles[2];
                biomeTypes[1] = true;
                break;
            case biomes.PLAINS:
                this.GetComponent<Renderer>().material = baseTiles[3];
                biomeTypes[2] = true;
                break;
            case biomes.VALLEY:
                this.GetComponent<Renderer>().material = baseTiles[4];
                biomeTypes[3] = true;
                break;
            case biomes.HILLS:
                this.GetComponent<Renderer>().material = baseTiles[5];
                biomeTypes[4] = true;
                break;
            case biomes.MARSHES:
                this.GetComponent<Renderer>().material = baseTiles[6];
                biomeTypes[5] = true;
                break;
            case biomes.FOREST:
                this.GetComponent<Renderer>().material = baseTiles[7];
                biomeTypes[6] = true;
                break;
            case biomes.TUNDRA:
                this.GetComponent<Renderer>().material = baseTiles[8];
                biomeTypes[7] = true;
                break;
            case biomes.GLACIER:
                this.GetComponent<Renderer>().material = baseTiles[9];
                isFrozen = true;
                isOcean = true;
                break;
            case biomes.ICEBERG:
                this.GetComponent<Renderer>().material = baseTiles[10];
                isFrozen = true;
                isOcean = true;
                break;
        }
    }

    public void setBorder()
    {
        this.gameObject.GetComponent<Renderer>().material = border;
        Destroy(this);
    }

    public void setModifier(modifiers mod, bool set)
    {
        for (int j = 0; j < modifierTypes.Length; j++)
        {
            modifierTypes[j] = false;
        }
        if (set)
        {
            switch (mod)
            {
                case modifiers.VOLCANO:
                    modLayer.GetComponent<Renderer>().material = modTiles[0];
                    modifierTypes[0] = true;
                    break;
                case modifiers.HOT_SPOT:
                    modLayer.GetComponent<Renderer>().material = modTiles[1];
                    modifierTypes[1] = true;
                    break;
                case modifiers.HIGH_WIND:
                    modLayer.GetComponent<Renderer>().material = modTiles[2];
                    modifierTypes[2] = true;
                    break;
                case modifiers.RIVER:
                    modLayer.GetComponent<Renderer>().material = modTiles[3];
                    modifierTypes[3] = true;
                    break;
                case modifiers.EARTHQUAKE:
                    modLayer.GetComponent<Renderer>().material = modTiles[4];
                    modifierTypes[4] = true;
                    break;
                case modifiers.NATURAL_GAS:
                    modLayer.GetComponent<Renderer>().material = modTiles[5];
                    modifierTypes[5] = true;
                    break;
                case modifiers.DENSE_TREES:
                    modLayer.GetComponent<Renderer>().material = modTiles[6];
                    modifierTypes[6] = true;
                    break;
                case modifiers.COAL:
                    modLayer.GetComponent<Renderer>().material = modTiles[7];
                    modifierTypes[7] = true;
                    break;
            }
            modLayer.SetActive(true);
        }
        else
        {
            modLayer.SetActive(false);
        }
    }
    public void setCity(bool set)
    {
        if (set)
        {
            buildingLayer.GetComponent<Renderer>().material = city;
            buildingLayer.SetActive(true);
            hasCity = true;
        }
        else
        {
            buildingLayer.SetActive(false);
            hasCity = false;
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
        mainCamera.GetComponent<cameraControl>().cameraDistance = mainCamera.GetComponent<cameraControl>().tempCameradistance;
        //mainCamera.GetComponent<cameraControl>().tempCameradistance = 0;
        mainCamera.GetComponent<cameraControl>().canControl = true;
        mainCamera.GetComponent<cameraControl>().currTile = null;
        canvas.SetActive(true);
        OnMouseExit();
    }
    public void removeScript()
    {
        Destroy(this);
    }
}
