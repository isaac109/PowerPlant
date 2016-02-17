using UnityEngine;
using System.Collections;

public class ppPlacement : MonoBehaviour {

    public updateTileUI tileUI;
    public Material ppMaterial;
    public ppHexManager.Plants plant;

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
        tileUI.tile.gameObject.AddComponent<ppHexManager>();
        switch (plant)
        {
            case ppHexManager.Plants.BIOMAS:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.BIOMAS;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.COAL:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 1;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.COAL;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.GAS:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.GAS;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.GEOTHERMAL:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.GEOTHERMAL;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.HYDRO:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.HYDRO;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.NUCLEAR:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.NUCLEAR;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.SOLAR:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.SOLAR;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.TECTONIC:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.TECTONIC;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.TIDAL:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.TIDAL;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
            case ppHexManager.Plants.WIND:
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerPrice = 30;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().powerRange = 2;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().plant = ppHexManager.Plants.WIND;
                tileUI.tile.gameObject.GetComponent<ppHexManager>().maxPowerOutput = 30000;
                break;
        }
        tileUI.tile.gameObject.GetComponent<ppHexManager>().getCitiesInRange();
        tileUI.tile.updateUI();
    }
}
