using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

    float horSpeed = 20f;
    float verspeed = 20f;

    float cameraDistanceMax = 360f;
    float cameraDistanceMin = 20f;
    public float cameraDistance = 100f;
    float scrollSpeed = 100f;

    public bool canControl = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (canControl)
        {
            if (Input.GetMouseButton(1))
            {
                float h = horSpeed * Input.GetAxis("Mouse X");
                float v = verspeed * Input.GetAxis("Mouse Y");
                this.transform.Translate(-h, -v, 0);
            }
            cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            this.transform.position = new Vector3(this.transform.position.x, cameraDistance, this.transform.position.z);
            cameraBounds();
        }
	}

    void cameraBounds()
    {
        //cxmin = (xcent - xmin) / (cameraDistance - cameraDistanceMin+1);
    }

}
