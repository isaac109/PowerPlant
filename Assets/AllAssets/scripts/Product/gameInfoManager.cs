using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gameInfoManager : MonoBehaviour {

    //geo, solar, wind, hydro, tectonic, gas, bio, coal, nuclear, tidal
    public List<bool> ownedPlants = new List<bool>();
    public List<upgrades> ownedUpgrades = new List<upgrades>();

    public class upgrades
    {
        public List<bool> ups = new List<bool>();
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 10; i++)
        {
            ownedPlants.Add(false);
            upgrades temp = new upgrades();
            for (int j = 0; j < 5; j++)
            {
                temp.ups.Add(false);
            }
            ownedUpgrades.Add(temp);
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
