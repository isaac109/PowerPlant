using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class scrollViewButtons : MonoBehaviour {
    
    public GameObject uiObj;
    public gridManager gm;
    public updateTileUI upTile;
    public GameObject gameInfoManager;
    float x = 91.5f;
    public List<GameObject> ppOptions = new List<GameObject>();
    public List<GameObject> instantiatedButtons = new List<GameObject>();
	
    // Use this for initialization
	void Start () {
        gameInfoManager = GameObject.Find("gameInfoManager");
        gm = GameObject.Find("grid"  ).GetComponent<gridManager>();
        upTile = GameObject.Find("tileUI").GetComponent<updateTileUI>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void populateCities()
    {
        List<GameObject> cities = gm.getCities();
        for (int i = 0; i < cities.Count; i++)
        {
            GameObject temp = Instantiate(uiObj) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(x, -30 * (i+1), 0);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.GetComponent<buttinMoveInfo>().x = cities[i].GetComponent<hexTile2>().widthInArray;
            temp.GetComponent<buttinMoveInfo>().y = cities[i].GetComponent<hexTile2>().heightInArray;
            temp.GetComponent<buttinMoveInfo>().gm = GameObject.Find("grid").GetComponent<gridManager>();
            foreach (Transform t in temp.transform)
            {
                if (t.name == "name")
                {
                    t.GetComponent<Text>().text = cities[i].GetComponent<cityHexManager>().cityName;
                }
            }
            instantiatedButtons.Add(temp);

        }
        
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (cities.Count+2) * 30);
    }

    public void populatePowerPlants()
    {
        List<GameObject> pps = gm.getPPs();
        for (int i = 0; i < pps.Count; i++)
        {
            GameObject temp = Instantiate(uiObj) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(x, -30 * (i + 1), 0);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.GetComponent<buttinMoveInfo>().x = pps[i].GetComponent<hexTile2>().widthInArray;
            temp.GetComponent<buttinMoveInfo>().y = pps[i].GetComponent<hexTile2>().heightInArray;
            temp.GetComponent<buttinMoveInfo>().gm = GameObject.Find("grid").GetComponent<gridManager>();
            
            foreach (Transform t in temp.transform)
            {
                if (t.name == "name")
                {
                    t.GetComponent<Text>().text = pps[i].name;
                }
            }
            instantiatedButtons.Add(temp);

        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (pps.Count + 2) * 30);
    }
    public void populatePowerPlantOptions()
    {
        if(instantiatedButtons.Count != 0)
        {
            destroyButtons();
        }
        for (int i = 0; i < ppOptions.Count; i++)
        {
            GameObject temp = Instantiate(ppOptions[i]) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(x, -30 * (i + 1), 0);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.GetComponent<Button>().interactable = gameInfoManager.GetComponent<gameInfoManager>().ownedPlants[i];
            instantiatedButtons.Add(temp);
        }
        if (upTile.tile.isCoast)
        {
            for (int i = 0; i < instantiatedButtons.Count - 1; i++)
            {
                instantiatedButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            instantiatedButtons[9].GetComponent<Button>().interactable = false;
        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (ppOptions.Count + 1) * 30);
        
    }

    public void populatePowerCities()
    {
        if (instantiatedButtons.Count != 0)
        {
            destroyButtons();
        }
        List<GameObject> citiesInRange = new List<GameObject>();
        citiesInRange = upTile.tile.GetComponent<ppHexManager>().citiesInRange;
        for (int i = 0; i < citiesInRange.Count; i++)
        {
            GameObject temp = Instantiate(uiObj) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(x, -40 * (i + 1), 0);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.GetComponent<Slider>().maxValue = upTile.tile.GetComponent<ppHexManager>().maxPowerOutput;
            temp.GetComponent<citySliderUpdate>().position = i;
            temp.GetComponent<citySliderUpdate>().tile = upTile.tile.GetComponent<ppHexManager>();
            temp.GetComponent<citySliderUpdate>().cityTile = citiesInRange[i];
            if(!upTile.tile.GetComponent<ppHexManager>().citiesSliders.Contains(temp.GetComponent<Slider>()))
            {
                upTile.tile.GetComponent<ppHexManager>().citiesSliders.Add(temp.GetComponent<Slider>());
            }
            foreach (Transform t in temp.transform)
            {
                if (t.name == "name")
                {
                    t.GetComponent<Text>().text = citiesInRange[i].GetComponent<cityHexManager>().cityName;
                }
                
            }
            instantiatedButtons.Add(temp);
        }
        upTile.tile.GetComponent<ppHexManager>().updateCurrentPowerOutput(-1);
        upTile.tile.GetComponent<ppHexManager>().initializeSliders();
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (citiesInRange.Count + 1) * 30);

    }

    public void addUpgrade(int i)
    {
        upTile.tile.GetComponent<ppHexManager>().upgrades[i] = true;
        setUpgradeOptions();
    }

    public void setUpgradeOptions()
    {
        for (int i = 0; i < upTile.tile.GetComponent<ppHexManager>().upgrades.Count; i++)
        {
            if (upTile.tile.GetComponent<ppHexManager>().upgrades[i] ||
                !gameInfoManager.GetComponent<gameInfoManager>().ownedUpgrades[(int)upTile.tile.GetComponent<ppHexManager>().plant].ups[i])
            {
                ppOptions[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                ppOptions[i].GetComponent<Button>().interactable = true;
            }
        }
        updateText();
    }

    public void updateText()
    {
        string text = "This ";
        switch (upTile.tile.GetComponent<ppHexManager>().plant)
        {
            case ppHexManager.Plants.BIOMAS:
                text += "Biomas station";
                break;
            case ppHexManager.Plants.COAL:
                text += "Coal burning plant";
                break;
            case ppHexManager.Plants.GAS:
                text += "Natural Gas burning plant";
                break;
            case ppHexManager.Plants.GEOTHERMAL:
                text += "Geothermal station";
                break;
            case ppHexManager.Plants.HYDRO:
                text += "Hydro-electric dam";
                break;
            case ppHexManager.Plants.NUCLEAR:
                text += "Nuclear power plant";
                break;
            case ppHexManager.Plants.SOLAR:
                text += "Solar farm";
                break;
            case ppHexManager.Plants.TECTONIC:
                text += "Tectonic power station";
                break;
            case ppHexManager.Plants.TIDAL:
                text += "Tidal power station";
                break;
            case ppHexManager.Plants.WIND:
                text += "Wind farm";
                break;
        }
        text += " is in possesion of:\n";
        bool hasUpgrades = false;     
        for (int i = 0; i < upTile.tile.GetComponent<ppHexManager>().upgrades.Count; i++)
        {
            if (upTile.tile.GetComponent<ppHexManager>().upgrades[i])
            {
                text += "Upgrade " + (i+1).ToString() + "\n";
                hasUpgrades = true;
            }
        }
        if (!hasUpgrades)
        {
            text += "No current Upgrades";
        }
        uiObj.GetComponent<Text>().text = text;
    }

    public void populateTileInfo()
    {
        string text = "This area of the world is home to ";
        switch (upTile.tile.biome)
        {
            case hexTile2.biomes.DESERT:
                text += "a harsh,hot desert. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.FOREST:
                text += "a beautiful forest streatching for miles. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.HILLS:
                text += "soft rolling hills. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.MARSHES:
                text += "a thick, wet marsh. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.MOUNTAIN:
                text += "towering mountains. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.OCEAN:
                text += "a large rolling ocean. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else if(upTile.tile.isCoast)
                {
                    text += "Here is an ideal location for PLANT TYPE, unfortunatly no other plant can feasably be located here. ";
                }
                break;
            case hexTile2.biomes.PLAINS:
                text += "flat, smooth plains. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.TUNDRA:
                text += "freezing cold tundra. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
            case hexTile2.biomes.VALLEY:
                text += "deep cut of valley. ";
                if (upTile.tile.hasCity)
                {
                    text += "It is home to the SIZE city of NAME. It has a population of POPULATION. ";
                }
                else if (upTile.tile.hasPowerPlant)
                {
                    text += "Located here is a PLANT TYPE, capable of producing ENERGY OUTPUT. ";
                }
                else
                {
                    text += "Here is an ideal location for PLANT TYPE, however PLANT TYPE is sure to function poorly in these conditions. ";
                }
                break;
        }
        this.GetComponent<Text>().text = text;
    }

    public void destroyButtons()
    {
        if (instantiatedButtons.Count != 0)
        {
            foreach (GameObject item in instantiatedButtons)
            {
                Destroy(item);
            }
            instantiatedButtons.Clear();
        }
    }
}
