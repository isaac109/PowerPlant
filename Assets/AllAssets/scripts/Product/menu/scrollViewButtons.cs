using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class scrollViewButtons : MonoBehaviour {
    
    public GameObject uiObj;
    public gridManager gm;
    public updateTileUI upTile;
    float x = 91.5f;
    public List<GameObject> ppOptions = new List<GameObject>();
    public List<GameObject> instantiatedButtons = new List<GameObject>();
	
    // Use this for initialization
	void Start () {
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
            instantiatedButtons.Add(temp);
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
            temp.GetComponent<citySliderUpdate>().tile = upTile.tile.GetComponent<ppHexManager>();
            temp.GetComponent<citySliderUpdate>().cityTile = citiesInRange[i];
            temp.GetComponent<Slider>().value = citiesInRange[i].GetComponent<cityHexManager>().powerRec;
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
        upTile.tile.GetComponent<ppHexManager>().updateCurrentPowerOutput();
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (citiesInRange.Count + 1) * 30);

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
