using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class hexTile2 : MonoBehaviour {

    public GameObject tileUI;
    public Button leaveTileUI;
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

    public Material sCity;
    public Material mCity;
    public Material lCity;

    public bool isOcean = false;
    public bool isLand = false;
    public bool isFrozen = false;

    //isMountain, isDesert, isPlains, isValley, isHills, isMarshes, isForest, isTundra
    public biomes biome = biomes.NONE;
    //hasVolcano, hasHotSpot, hasHighWinds, hasRiver, hasEarthquake, hasNaturalGas, hasDenseTrees, hasCoal
    public modifiers mod = modifiers.NONE;
    
    public bool isCoast = false;
    public bool hasCity = false;
    public bool hasPowerPlant = false;

    public bool isChecked = false;
    public bool isMouseOver = false;
    public bool isSelected = false;

    
    
    public Camera mainCamera;
    public Camera menuCamera;
    public float cameraDistance;

    public int heightInArray;
    public int widthInArray;

    public bool isSunBleached = false;

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
        ICEBERG,
        NONE
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
        COAL,
        NONE
    }
	// Use this for initialization
	void Start () {
        tileUI = GameObject.Find("tileUI");
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
        /*if (mainCamera.GetComponent<cameraControl>().currTile != null)
        {
            mainCamera.GetComponent<cameraControl>().currTile.GetComponent<hexManagement>().close();
        }*/
        menuCamera.enabled = true;
        mainCamera.rect = new Rect(0f, .5f, 1f, 2f);
        cameraDistance = mainCamera.GetComponent<cameraControl>().cameraSize;
        mainCamera.GetComponent<cameraControl>().cameraSize = 20;
        mainCamera.gameObject.transform.position = new Vector3(this.transform.position.x, mainCamera.gameObject.transform.position.y, this.transform.position.z);
        gm.GetComponent<keyListener>().toggleMenu(9) ;
        isSelected = true;
        mainCamera.GetComponent<cameraControl>().canControl = false;
        mainCamera.GetComponent<cameraControl>().currTile = this.gameObject;    
        updateUI();
    }

    public void updateUI()
    {
        tileUI.GetComponent<updateTileUI>().tile = this;
        tileUI.GetComponent<updateTileUI>().toggleViews(-1);
        if (hasCity)
        {
            tileUI.GetComponent<updateTileUI>().updateButtons(1);
        }
        else if (hasPowerPlant)
        {
            tileUI.GetComponent<updateTileUI>().updateButtons(2);
        }
        else if(!hasPowerPlant && !isOcean)
        {
            tileUI.GetComponent<updateTileUI>().updateButtons(0);
        }
        else if (!hasPowerPlant && isCoast)
        {
            tileUI.GetComponent<updateTileUI>().updateButtons(0);
        }
        else if (isOcean)
        {
            tileUI.GetComponent<updateTileUI>().updateButtons(-1);
        }
    }

    public void setTerrain(biomes b)
    {
        isOcean = false;
        isFrozen = false;
        biome = biomes.NONE;
        switch (b)
        {
            case biomes.OCEAN:
                this.GetComponent<Renderer>().material = baseTiles[0];
                isOcean = true;
                biome = biomes.OCEAN;
                break;
            case biomes.MOUNTAIN:
                this.GetComponent<Renderer>().material = baseTiles[1];
                biome = biomes.MOUNTAIN;
                break;
            case biomes.DESERT:
                this.GetComponent<Renderer>().material = baseTiles[2];
                biome = biomes.DESERT;
                break;
            case biomes.PLAINS:
                this.GetComponent<Renderer>().material = baseTiles[3];
                biome = biomes.PLAINS;
                break;
            case biomes.VALLEY:
                this.GetComponent<Renderer>().material = baseTiles[4];
                biome = biomes.VALLEY;
                break;
            case biomes.HILLS:
                this.GetComponent<Renderer>().material = baseTiles[5];
                biome = biomes.HILLS;
                break;
            case biomes.MARSHES:
                this.GetComponent<Renderer>().material = baseTiles[6];
                biome = biomes.MARSHES;
                break;
            case biomes.FOREST:
                this.GetComponent<Renderer>().material = baseTiles[7];
                biome = biomes.FOREST;
                break;
            case biomes.TUNDRA:
                this.GetComponent<Renderer>().material = baseTiles[8];
                biome = biomes.TUNDRA;
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

    public void setModifier(modifiers m, bool set)
    {
        mod = modifiers.NONE;
        if (set)
        {
            switch (m)
            {
                case modifiers.VOLCANO:
                    modLayer.GetComponent<Renderer>().material = modTiles[0];
                    mod = modifiers.VOLCANO;
                    break;
                case modifiers.HOT_SPOT:
                    modLayer.GetComponent<Renderer>().material = modTiles[1];
                    mod = modifiers.HOT_SPOT;
                    break;
                case modifiers.HIGH_WIND:
                    modLayer.GetComponent<Renderer>().material = modTiles[2];
                    mod = modifiers.HIGH_WIND;
                    break;
                case modifiers.RIVER:
                    modLayer.GetComponent<Renderer>().material = modTiles[3];
                    mod = modifiers.RIVER;
                    break;
                case modifiers.EARTHQUAKE:
                    modLayer.GetComponent<Renderer>().material = modTiles[4];
                    mod = modifiers.EARTHQUAKE;
                    break;
                case modifiers.NATURAL_GAS:
                    modLayer.GetComponent<Renderer>().material = modTiles[5];
                    mod = modifiers.NATURAL_GAS;
                    break;
                case modifiers.DENSE_TREES:
                    modLayer.GetComponent<Renderer>().material = modTiles[6];
                    mod = modifiers.DENSE_TREES;
                    break;
                case modifiers.COAL:
                    modLayer.GetComponent<Renderer>().material = modTiles[7];
                    mod = modifiers.COAL;
                    break;
            }
            modLayer.SetActive(true);
        }
        else
        {
            modLayer.SetActive(false);
        }
    }
    public void setCity(bool set, int size)
    {
        if (set)
        {
            switch(size)
            {
                case 0:
                    buildingLayer.GetComponent<Renderer>().material = sCity;
                    break;
                case 1:
                    buildingLayer.GetComponent<Renderer>().material = mCity;
                    break;
                case 2:
                    buildingLayer.GetComponent<Renderer>().material = lCity;
                    break;
            }
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
        this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        isMouseOver = true;
    }
    void OnMouseExit()
    {
        for (int i = 0; i < borders.Length; i++)
        {
            borders[i].gameObject.GetComponent<Renderer>().material = normal;
        }
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
        isMouseOver = false;
    }
    public void closeCameras()
    {
        menuCamera.enabled = false;
        
        this.GetComponent<hexManagement>().show = false;
        isSelected = false;
        mainCamera.GetComponent<cameraControl>().cameraSize = mainCamera.GetComponent<cameraControl>().tempCameradistance;
        //mainCamera.GetComponent<cameraControl>().tempCameradistance = 0;
        mainCamera.GetComponent<cameraControl>().canControl = true;
        mainCamera.GetComponent<cameraControl>().currTile = null;
        mainCamera.rect = new Rect(0f, 0f, 1f, 1f);
        OnMouseExit();
    }
    public void removeScript()
    {
        Destroy(this);
    }

}
