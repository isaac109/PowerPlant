using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ppHexManager : MonoBehaviour {

    public int maxPowerOutput = 0;
    public int powerRange = 0;
    public int powerPrice = 0;
    public List<GameObject> citiesInRange = new List<GameObject>();
    public List<Slider> citiesSliders = new List<Slider>();
    public Plants plant;

    public enum Plants
    {
        GEOTHERMAL,
        SOLAR,
        WIND,
        HYDRO,
        TECTONIC,
        GAS,
        BIOMAS,
        COAL,
        NUCLEAR,
        TIDAL
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void getCitiesInRange()
    {
        citiesInRange = this.gameObject.GetComponent<hexTile2>().gm.getCitiesInRange(
            this.gameObject.GetComponent<hexTile2>().widthInArray,
            this.gameObject.GetComponent<hexTile2>().heightInArray,
            powerRange);
        foreach (GameObject item in citiesInRange)
        {
            item.GetComponent<cityHexManager>().powerRec = maxPowerOutput / citiesInRange.Count;
        }
    }


    public void removeSliders()
    {
        List<Slider> newSlider = new List<Slider>();
        foreach (Slider item in citiesSliders)
        {
            if (item != null)
            {
                newSlider.Add(item);
            }
        }
        citiesSliders = newSlider;
        while (citiesSliders.Count > citiesInRange.Count)
        {
            citiesSliders.RemoveAt(0);
        }
    }

    public void updateCurrentPowerOutput()
    {
        removeSliders();
        foreach (Slider item in citiesSliders)
        {
            int max = maxPowerOutput;
            if (citiesSliders.Count > 1)
            {
                foreach (Slider item2 in citiesSliders)
                {
                    if (item2 != item)
                    {
                        max -= (int)item2.value;
                    }
                }
            }
            item.maxValue = max;
            
            item.gameObject.GetComponent<citySliderUpdate>().updateCity(item.value);
        }
    }
}
