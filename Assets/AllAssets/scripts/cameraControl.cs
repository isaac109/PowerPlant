using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

    float horSpeed = 20f;
    float verspeed = 20f;

    float cameraDistanceMax = 360f;
    float cameraDistanceMin = 20f;
    float cameraDistance = 100f;
    float scrollSpeed = 100f;

    /*float xmin = 12f;
    float xmax = 672f;
    float ymin = -5f;
    float ymax = -310f;
    float xcent = 345f;
    float zcent = -150f;

    float cxmin;*/

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
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

    void cameraBounds()
    {
        //cxmin = (xcent - xmin) / (cameraDistance - cameraDistanceMin+1);
    }

}
