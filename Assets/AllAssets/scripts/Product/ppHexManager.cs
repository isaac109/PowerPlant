using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ppHexManager : MonoBehaviour {

    public int maxPowerOutput = 0;
    public int powerRange = 0;
    public int powerPrice = 0;
    public List<GameObject> citiesInRange = new List<GameObject>();
    public List<int> cityPower = new List<int>();
    public List<Slider> citiesSliders = new List<Slider>();
    public List<bool> upgrades = new List<bool>();
    public Plants plant;
    public int buildStartTurn = 1;
    public bool built = false;

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

        for (int i = 0; i < 5; i++)
        {
            upgrades.Add(false);
        }
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
        for (int i = 0; i < citiesInRange.Count; i++)
		{
            int power =  maxPowerOutput / citiesInRange.Count;
			citiesInRange[i].GetComponent<cityHexManager>().powerRec += power;
            cityPower.Add(power);
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

    public void initializeSliders()
    {
        for (int i = 0; i < citiesSliders.Count; i++)
        {
            citiesSliders[i].value = cityPower[i];
        }
    }

    public void updateCurrentPowerOutput(int position)
    {
        removeSliders();
        if (position == -1)
        {
            return;
        }
        foreach (Slider item in citiesSliders)
        {
            item.gameObject.GetComponent<citySliderUpdate>().updateCity(item.value, item.value);
        }
        int total = 0;
        if (citiesSliders.Count > 1)
        {
            foreach (Slider item in citiesSliders)
            {
                total += (int)item.value;
            }
            if (total > maxPowerOutput)
            {
                total -= maxPowerOutput;
                total /= (citiesSliders.Count - 1);
                for (int i = 0; i < citiesSliders.Count; i++)
                {
                    if (i != position)
                    {
                        citiesSliders[i].value -= total;
                        citiesSliders[i].gameObject.GetComponent<citySliderUpdate>().updateCity(cityPower[i], citiesSliders[i].value);
                        cityPower[i] = (int)citiesSliders[i].value;
                    }
                }
            }
            
        }
        citiesSliders[position].gameObject.GetComponent<citySliderUpdate>().updateCity(cityPower[position], citiesSliders[position].value);
        cityPower[position] = (int)citiesSliders[position].value;
        

        /*foreach (Slider item in citiesSliders)
        {
            int total = 0;
            if (citiesSliders.Count > 1)
            {
                foreach (Slider item2 in citiesSliders)
                {
                    total += (int)item2.value;
                }
                if (total > maxPowerOutput)
                {
                    total -= (int)item.value;
                    total /= (citiesSliders.Count - 1);
                    foreach (Slider item3 in citiesSliders)
                    {
                        item3.value = total;
                        item3.gameObject.GetComponent<citySliderUpdate>().updateCity(item3.value);
                    }
                }
            }
            
            item.gameObject.GetComponent<citySliderUpdate>().updateCity(item.value);
        }*/
    }

    public int getPowerConsumption()
    {
        switch (plant)
        {
            case Plants.COAL:
                return 1;
            case Plants.GAS:
                return 2;
            default:
                return 0;
        }
    }
    public int getPowerOutput()
    {
        int output = 0;
        foreach (GameObject item in citiesInRange)
        {
            output += (int)item.GetComponent<cityHexManager>().powerRec;
        }
        return output;
    }
    public int getPotantialIncome()
    {
        int money = 0;
        foreach (int item in cityPower)
        {
            money += (item *1000 * 1000 * powerPrice / 100);
        }
        return money;
    }
}
