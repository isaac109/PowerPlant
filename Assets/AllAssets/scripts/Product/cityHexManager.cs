using UnityEngine;
using System.Collections;

public class cityHexManager : MonoBehaviour {

    public double population = 0;
    public string cityName = "";
    public float powerRec = 0;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int getPwrLvl()
    {
        if (powerRec >= population)
        {
            return 2;//powered
        }
        else if (powerRec == 0)
        {
            return 0;//not powered
        }
        else
        {
            return 1;//under powered
        }
    }

}
