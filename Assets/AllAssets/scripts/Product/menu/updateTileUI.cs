using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class updateTileUI : MonoBehaviour {

    public List<GameObject> buttons = new List<GameObject>();
    public List<GameObject> scrollViews = new List<GameObject>();
    public hexTile2 tile;
    int curView = -1;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateButtons(int i)
    {
        foreach (GameObject item in buttons)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in scrollViews)
        {
            if (item.GetComponentInChildren<scrollViewButtons>().instantiatedButtons.Count != 0)
            {
                item.GetComponentInChildren<scrollViewButtons>().destroyButtons();
            }
        }
        switch (i)
        {
            case -1:
                buttons[0].SetActive(true);
                buttons[1].SetActive(true);
                buttons[1].GetComponent<RectTransform>().localPosition = new Vector3(-453, 105, 0);
                break;
            case 0:
                buttons[0].SetActive(true);
                buttons[1].SetActive(true);
                buttons[1].GetComponent<RectTransform>().localPosition = new Vector3(-453, 75, 0);
                buttons[6].SetActive(true);
                break;
            case 1:
                buttons[0].SetActive(true);
                buttons[1].SetActive(true);
                buttons[1].GetComponent<RectTransform>().localPosition = new Vector3(-453, 45, 0);
                buttons[2].SetActive(true);
                buttons[3].SetActive(true);
                break;
            case 2:
                buttons[0].SetActive(true);
                buttons[1].SetActive(true);
                buttons[1].GetComponent<RectTransform>().localPosition = new Vector3(-453, 45, 0);
                buttons[4].SetActive(true);
                buttons[5].SetActive(true);
                break;
        }
    }

    public void toggleViews(int i)
    {
        foreach (GameObject item in scrollViews)
        {
            item.SetActive(false);
        }
        if(i != -1)
        {
            scrollViews[i].SetActive(true);
        }
    }
    public void cloaseCameras()
    {
        tile.closeCameras();
    }
}
