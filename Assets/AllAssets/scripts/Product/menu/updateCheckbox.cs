using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class updateCheckbox : MonoBehaviour {

    public gameInfoManager gim;
    bool currState = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updatePlant(int plant)
    {
        if (gim.updateOwnedPlants(plant))
        {
            this.GetComponent<Toggle>().isOn = !currState;
            currState = !currState;
        }
        else
        {
            this.GetComponent<Toggle>().isOn = currState;
        }
    }
}
