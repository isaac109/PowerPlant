using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class citySliderUpdate : MonoBehaviour {

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
        tile.updateCurrentPowerOutput();      
    }
    public void updateCity(float value)
    {
        cityTile.GetComponent<cityHexManager>().powerRec = value;
    }
}
