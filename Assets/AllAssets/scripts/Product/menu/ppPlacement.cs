using UnityEngine;
using System.Collections;

public class ppPlacement : MonoBehaviour {

    public updateTileUI tileUI;
    public Material ppMaterial;

	// Use this for initialization
	void Start () {
        tileUI = GameObject.Find("tileUI").GetComponent<updateTileUI>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setPP()
    {
        tileUI.tile.hasPowerPlant = true;
        tileUI.tile.buildingLayer.SetActive(true);
        tileUI.tile.buildingLayer.GetComponent<Renderer>().material = ppMaterial;
        tileUI.tile.updateUI();
    }
}
