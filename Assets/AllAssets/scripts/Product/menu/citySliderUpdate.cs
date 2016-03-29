using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class citySliderUpdate : MonoBehaviour {

    public int position = 0;
    public ppHexManager tile;
    public GameObject cityTile;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void updateMax(float value)
    {
        tile.updateCurrentPowerOutput(position);      
    }
    public void updateCity(float initValue, float newValue)
    {
        cityTile.GetComponent<cityHexManager>().powerRec -= initValue;
        cityTile.GetComponent<cityHexManager>().powerRec += newValue;
    }
}
