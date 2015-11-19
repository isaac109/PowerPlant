using UnityEngine;
using System.Collections;

public class hexManagement : MonoBehaviour {

    public bool show = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        if (show)
        {
            GUILayout.BeginArea(new Rect(10, Screen.height / 2 + 10, 200, Screen.height / 2 + 50));
            GUILayout.BeginVertical();
            if (GUI.Button(new Rect(0, 0, 50, 50), "Close"))
            {
                this.GetComponent<hexTile2>().closeCameras();
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
