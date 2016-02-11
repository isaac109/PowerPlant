using UnityEngine;
using System.Collections;

public class menuResize : MonoBehaviour {

    public int menuX;
    public int menuY;
    public GameObject window;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        window.GetComponent<RectTransform>().sizeDelta = new Vector2(menuX, menuY);
    }
}
