using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class scrollViewButtons : MonoBehaviour {
    
    public GameObject cityButton;
    public gridManager gm;
    float x = 91.5f;
    public List<GameObject> ppOptions = new List<GameObject>();
    public List<GameObject> instantiatedButtons = new List<GameObject>();
    int counter = 0;
	
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void populateCities()
    {
        List<GameObject> cities = gm.getCities();
        for (int i = 0; i < cities.Count; i++)
        {
            GameObject temp = Instantiate(cityButton) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(x, -30 * (i+1), 0);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.GetComponent<buttinMoveInfo>().x = cities[i].GetComponent<hexTile2>().widthInArray;
            temp.GetComponent<buttinMoveInfo>().y = cities[i].GetComponent<hexTile2>().heightInArray;
            temp.GetComponent<buttinMoveInfo>().gm = GameObject.Find("grid").GetComponent<gridManager>();
            
            foreach (Transform t in temp.transform)
            {
                if (t.name == "name")
                {
                    t.GetComponent<Text>().text = cities[i].name;
                }
            }
            instantiatedButtons.Add(temp);

        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (cities.Count+2) * 30);
    }

    public void populatePowerPlants()
    {
    }
    public void populatePowerPlantOptions()
    {
        if(instantiatedButtons.Count != 0)
        {
            destroyButtons();
        }
        for (int i = 0; i < ppOptions.Count; i++)
        {
            GameObject temp = Instantiate(ppOptions[i]) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            if (counter == 0)
            {
                temp.GetComponent<RectTransform>().localPosition = new Vector3(x + 8.5f, -30 * (i + 1), 0);
            }
            else
            {
                temp.GetComponent<RectTransform>().localPosition = new Vector3(x, -30 * (i + 1), 0);
            }
            counter++;
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            instantiatedButtons.Add(temp);
        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (ppOptions.Count + 1) * 30);
        
    }
    public void destroyButtons()
    {
        if (instantiatedButtons.Count != 0)
        {
            foreach (GameObject item in instantiatedButtons)
            {
                Destroy(item);
            }
            instantiatedButtons.Clear();
        }
    }
}
