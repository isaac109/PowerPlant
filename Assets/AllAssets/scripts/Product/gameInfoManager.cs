using UnityEngine;
using System.Collections;

public class gameInfoManager : MonoBehaviour {

    //geo, solar, wind, hydro, tectonic, gas, bio, coal, nuclear, tidal
    public bool[] ownedPlants = new bool[10];

	// Use this for initialization
	void Start () {
        for (int i = 0; i < ownedPlants.Length; i++)
        {
            ownedPlants[i] = false;
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
