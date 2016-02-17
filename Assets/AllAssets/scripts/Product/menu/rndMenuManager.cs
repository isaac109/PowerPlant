using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class rndMenuManager : MonoBehaviour {

    public GameObject gameInfoManager;

    List<GameObject> plants = new List<GameObject>();
    List<plantUpgrades> plantUpgradesList = new List<plantUpgrades>();


    public class plantUpgrades
    {
        public GameObject parent;
        public List<GameObject> plantUpgradeList = new List<GameObject>();
    }

	// Use this for initialization
	void Start () {
        gameInfoManager = GameObject.Find("gameInfoManager");
        foreach (Transform item in this.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform)
        {
            if (item.tag == "R&DPlant")
            {
                plants.Add(item.gameObject);
            }
            if (item.tag == "R&DUpgrades")
            {
                plantUpgrades temp = new plantUpgrades();
                temp.parent = item.gameObject;
                foreach (Transform item2 in item.transform)
                {
                    temp.plantUpgradeList.Add(item2.gameObject);
                }
                temp.parent.SetActive(false);
                plantUpgradesList.Add(temp);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        updateButtons();
	}
    
    public void OnEnabled()
    {
        updateButtons();
    }

    public void newPlant(int i)
    {
        gameInfoManager.GetComponent<gameInfoManager>().ownedPlants[i] = true;
    }

    public void updateButtons()
    {
        for (int i = 0; i < plants.Count; i++)
        {
            if (gameInfoManager.GetComponent<gameInfoManager>().ownedPlants[i])
            {
                plants[i].GetComponent<Button>().interactable = false;
                plants[i].transform.Find("Cost").gameObject.SetActive(false);
                plants[i].transform.Find("CostSet").gameObject.SetActive(false);
                plants[i].transform.Find("State").GetComponent<Text>().text = "OWNED";
                plantUpgradesList[i].parent.SetActive(true);
            }
        }
    }
}
