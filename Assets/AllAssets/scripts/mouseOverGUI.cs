using UnityEngine;
using System.Collections;

public class mouseOverGUI : MonoBehaviour {

    public cameraControl camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseOver()
    {
        Debug.Log("over");
        camera.overGUI = true;
    }
    void OnMouseExit()
    {
        camera.overGUI = false;
    }
}
