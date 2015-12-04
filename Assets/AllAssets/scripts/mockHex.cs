using UnityEngine;
using System.Collections;

public class mockHex : MonoBehaviour {

    public GameObject realHex;
    public bool isMouseOver = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            realHex.GetComponent<hexTile2>().zoom();
        }
	}

    void OnMouseOver()
    {
        isMouseOver = true;
    }
    void OnMouseExit()
    {
        isMouseOver = false;
    }
}
