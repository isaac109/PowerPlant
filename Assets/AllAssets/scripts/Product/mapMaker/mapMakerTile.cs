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
                isLand = true;
                break;
            case biomes.DESERT:
                this.GetComponent<Renderer>().material = baseTiles[2];
                biomeTypes[1] = true;
                isLand = true;
                break;
            case biomes.PLAINS:
                this.GetComponent<Renderer>().material = baseTiles[3];
                biomeTypes[2] = true;
                isLand = true;
                break;
            case biomes.VALLEY:
                this.GetComponent<Renderer>().material = baseTiles[4];
                biomeTypes[3] = true;
                isLand = true;
                break;
            case biomes.HILLS:
                this.GetComponent<Renderer>().material = baseTiles[5];
                biomeTypes[4] = true;
                isLand = true;
                break;
            case biomes.MARSHES:
                this.GetComponent<Renderer>().material = baseTiles[6];
                biomeTypes[5] = true;
                isLand = true;
                break;
            case biomes.FOREST:
                this.GetComponent<Renderer>().material = baseTiles[7];
                biomeTypes[6] = true;
                isLand = true;
                break;
            case biomes.TUNDRA:
                this.GetComponent<Renderer>().material = baseTiles[8];
                biomeTypes[7] = true;
                isLand = true;
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
