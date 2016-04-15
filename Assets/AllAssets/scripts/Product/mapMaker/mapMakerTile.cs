using UnityEngine;
using System.Collections;

public class mapMakerTile : MonoBehaviour {

    public GameObject canvas;
    public GameObject modLayer;
    public GameObject buildingLayer;
    public Material normal;
    public Material highlighted;
    public Material selected;


    public int counter = 0;
    public mapMakerManager gm;
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
    public cities hasCity = cities.NONE;


    public bool isChecked = false;
    public bool isMouseOver = false;
    public bool isSelected = false;


    public int heightInArray;
    public int widthInArray;

    public bool isSunBleached = false;
    public bool canBeChanged = true;

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
    public enum cities
    {
        SMALL,
        MEDIUM,
        LARGE,
        NONE
    }
    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        this.gameObject.tag = "Tile";
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
        if (gm.GetComponent<mapMakerChangeMenu>().canSelect && isMouseOver && Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (canBeChanged)
            {
                gm.gameObject.GetComponent<mapMakerChangeMenu>().changeTiles = true;
            }
            else
            {
                gm.gameObject.GetComponent<mapMakerChangeMenu>().changeTiles = false;
            }
            
        }
        if (gm.GetComponent<mapMakerChangeMenu>().canSelect && isMouseOver && Input.GetMouseButton(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            if (gm.gameObject.GetComponent<mapMakerChangeMenu>().changeTiles)
            {
                isSelected = true;
                if (!gm.gameObject.GetComponent<mapMakerChangeMenu>().tilesToChange.Contains(this.gameObject))
                {
                    gm.gameObject.GetComponent<mapMakerChangeMenu>().tilesToChange.Add(this.gameObject);
                }
            }
            else
            {
                isSelected = false;
                if (gm.gameObject.GetComponent<mapMakerChangeMenu>().tilesToChange.Contains(this.gameObject))
                {
                    gm.gameObject.GetComponent<mapMakerChangeMenu>().tilesToChange.Remove(this.gameObject);
                }
            }
        }
        if (isSelected && this.GetComponent<Renderer>())
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.red;
            canBeChanged = false;
        }
        if (!isSelected && this.GetComponent<Renderer>())
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.white;
            canBeChanged = true;
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
    public void setCity(bool set, cities size)
    {
        hasCity = cities.NONE;
        if (set)
        {
            switch (size)
            {
                case cities.SMALL:
                    buildingLayer.GetComponent<Renderer>().material = sCity;
                    hasCity = cities.SMALL;
                    break;
                case cities.MEDIUM:
                    buildingLayer.GetComponent<Renderer>().material = mCity;
                    hasCity = cities.MEDIUM;
                    break;
                case cities.LARGE:
                    buildingLayer.GetComponent<Renderer>().material = lCity;
                    hasCity = cities.LARGE;
                    break;
            }
            buildingLayer.SetActive(true);
        }
        else
        {
            buildingLayer.SetActive(false);
        }
    }

    void OnMouseOver()
    {
        isMouseOver = true;
        this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }
    void OnMouseExit()
    {
        isMouseOver = false;
        this.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }
    
    public void removeScript()
    {
        Destroy(this);
    }
}
