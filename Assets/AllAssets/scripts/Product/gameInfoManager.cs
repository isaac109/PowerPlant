using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameInfoManager : MonoBehaviour {

    //geo, solar, wind, hydro, tectonic, gas, bio, coal, nuclear, tidal
    public List<bool> ownedPlants = new List<bool>();
    public List<bool> researchedPlants = new List<bool>();
    public List<upgrades> ownedUpgrades = new List<upgrades>();
    public List<upgrades> researchedUpgrades = new List<upgrades>();
    public double environmentHealth = 100;
    public double playerWealth = 100;
    public double percentHappiness = 100;
    public double coal = 100;
    public double gas = 100;

    public int turnNum = 1;

    public class upgrades
    {
        public List<bool> ups = new List<bool>();
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
        {
            ownedPlants.Add(false);
            researchedPlants.Add(false);
            upgrades temp = new upgrades();
            upgrades temp2 = new upgrades();
            for (int j = 0; j < 5; j++)
            {
                temp.ups.Add(false);
                temp2.ups.Add(false);
            }
            ownedUpgrades.Add(temp);
            researchedUpgrades.Add(temp2);
        }
        DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool updateOwnedPlants(int plant)
    {
        if (ownedPlants[plant])
        {
            ownedPlants[plant] = false;
            return true;
        }
        else
        {
            int count = 0;
            foreach (bool item in ownedPlants)
            {
                if (item)
                {
                    count++;
                }
            }
            if (count < 2)
            {
                ownedPlants[plant] = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
