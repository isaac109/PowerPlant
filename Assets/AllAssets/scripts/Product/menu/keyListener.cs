using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class keyListener : MonoBehaviour {

    public List<GameObject> menus = new List<GameObject>();
    public List<scrollViewButtons> scrollViews = new List<scrollViewButtons>();
    bool hIsPress = false;
    bool escIsPress = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            escPress();
        }
        if (Input.GetKeyUp(KeyCode.H) && !escIsPress)
        {
            if (hIsPress)
            {               
                menus[0].SetActive(true);
                hIsPress = false;
            }
            else
            {
                toggleMenu(0);
                menus[0].SetActive(false);
                
                hIsPress = true;
            }
        }
	}

    public void escPress()
    {
        if (escIsPress)
        {
            toggleMenu(0);
            if (hIsPress)
            {
                hIsPress = false;
            }
            escIsPress = false;
        }
        else
        {
            toggleMenu(1);
            escIsPress = true;
        }
    }

    public void toggleMenu(int j)
    {
        foreach (scrollViewButtons item in scrollViews)
        {
            item.destroyButtons();
        }
        for (int i = 0; i < menus.Count; i++)
        {
            if (i == j)
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
}
