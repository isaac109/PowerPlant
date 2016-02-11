using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class scrollViewButtons : MonoBehaviour {
    
    public GameObject button;
    public gridManager gm;
	
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
            GameObject temp = Instantiate(button) as GameObject;
            temp.gameObject.transform.SetParent(this.gameObject.transform);
            temp.GetComponent<RectTransform>().localPosition = new Vector3(91.5f, -30 * (i+1), 0);
            temp.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            temp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
            temp.GetComponent<buttinMoveInfo>().x = cities[i].GetComponent<hexTile2>().widthInArray;
            temp.GetComponent<buttinMoveInfo>().y = cities[i].GetComponent<hexTile2>().heightInArray;
            temp.GetComponent<buttinMoveInfo>().gm = GameObject.Find("grid").GetComponent<gridManager>();
            temp.GetComponent<Button>().onClick.AddListener(() =>
            {
                gm.gameObject.GetComponent<keyListener>().toggleMenu(0);
            });
            foreach (Transform t in temp.transform)
            {
                if (t.name == "name")
                {
                    t.GetComponent<Text>().text = cities[i].name;
                }
            }

        }
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (cities.Count+1) * 30);
    }
}
