using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mapMakerChangeMenu : MonoBehaviour {

    public GameObject hud;
    public GameObject changeMenu;
    public Camera mainCamera;
    public List<GameObject> tilesToChange = new List<GameObject>();
    public List<GameObject> origionalTiles;
    public List<Material> origionalMaterials;
    public bool canSelect = true;
    public bool changeTiles = true;
	// Use this for initialization

    

	void Start () {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    
    public void zoom()
    {
        hud.SetActive(false);
        changeMenu.SetActive(true);
        changeSelection();
        mainCamera.gameObject.GetComponent<mapMakerCameraController>().cameraDistance = mainCamera.gameObject.GetComponent<mapMakerCameraController>().cameraDistanceMax;
        mainCamera.GetComponent<mapMakerCameraController>().canControl = false;
    }

    public void closeCameras()
    {
        mainCamera.GetComponent<mapMakerCameraController>().canControl = true;
        hud.SetActive(true);
        changeMenu.SetActive(false);
        canSelect = true;
        changeSelection();
    }

    public void select()
    {
        zoom();
        canSelect = false;
        origionalTiles = new List<GameObject>();
        origionalMaterials = new List<Material>();
        for (int i = 0; i < tilesToChange.Count; i++)
        {
            UnityEditorInternal.ComponentUtility.CopyComponent(tilesToChange[i].GetComponent<mapMakerTile>());
            GameObject temp = new GameObject();
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(temp);
            origionalTiles.Add(temp);
            origionalMaterials.Add(tilesToChange[i].GetComponent<Renderer>().material);
        }
    }

    public void clearList()
    {
        for (int i = 0; i < tilesToChange.Count; i++)
        {
            tilesToChange[i].GetComponent<mapMakerTile>().isSelected = false;
            Destroy(origionalTiles[i]);
        }
        tilesToChange.Clear();
        origionalTiles.Clear();
        origionalMaterials.Clear();
    }

    public void changeBiomes(mapMakerTile.biomes biome)
    {
        for (int i = 0; i < tilesToChange.Count; i++)
        {
            tilesToChange[i].GetComponent<mapMakerTile>().setTerrain(biome);
        }
    }

    public void declineChanges()
    {
        for (int i = 0; i < tilesToChange.Count; i++)
        {
            tilesToChange[i].GetComponent<Renderer>().material = origionalMaterials[i];
            UnityEditorInternal.ComponentUtility.CopyComponent(origionalTiles[i].GetComponent<mapMakerTile>());
            UnityEditorInternal.ComponentUtility.PasteComponentValues(tilesToChange[i].GetComponent<mapMakerTile>());           
        }
    }

    public void changeSelection()
    {
        this.GetComponent<mapMakerManager>().changeSelection();
    }
}
