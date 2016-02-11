using UnityEngine;
using System.Collections;

public class showHideButtons : MonoBehaviour {

    public GameObject buttons;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Hide()
    {
        buttons.SetActive(false);
    }
    public void Show()
    {
        buttons.SetActive(true);
    }
}
