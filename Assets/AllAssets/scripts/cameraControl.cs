using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

    float horSpeed = 20f;
    float verspeed = 20f;

    float cameraDistanceMax = 260f;
    float cameraDistanceMin = 20f;
    float cameraWidthMax = 0;
    float cameraHeightMax = 0;
    public float cameraDistance = 100f;
    public float tempCameradistance = 0f;
    float scrollSpeed = 100f;

    public bool canControl = true;

    public gridManager gm;

    public GameObject currTile = null;

	// Use this for initialization
	void Start () {
        while(!gm.done)
        {
        }
        this.transform.position = new Vector3(gm.maxWidth/2, this.transform.position.y, gm.maxHeight/2);
        cameraDistanceMax = (float)gm.cwidth * (8f+(2.0f/3.0f));
        cameraDistance = cameraDistanceMax;
	
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
            tempCameradistance = cameraDistance;
            cameraWidthMax = (cameraDistance / cameraDistanceMax) * gm.maxWidth / 2;
            cameraHeightMax = (cameraDistance / cameraDistanceMax) * gm.maxHeight / 2;
            Debug.Log(cameraWidthMax.ToString());
            if (this.transform.position.x <= cameraWidthMax)
            {
                // Debug.Log("here");
                this.transform.position = new Vector3(cameraWidthMax, this.transform.position.y, this.transform.position.z);
                //Debug.Log("here2" + (gm.maxWidth - cameraWidthMax).ToString());
            }
            if (this.transform.position.x > gm.maxWidth - cameraWidthMax)
            {
                this.transform.position = new Vector3(gm.maxWidth - cameraWidthMax, this.transform.position.y, this.transform.position.z);
            }
            if (this.transform.position.z < cameraHeightMax)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, cameraHeightMax);
            }
            if (this.transform.position.z > gm.maxHeight - cameraHeightMax)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, gm.maxHeight - cameraHeightMax);
            }
        }
        
	}

    void cameraBounds()
    {
        //cxmin = (xcent - xmin) / (cameraDistance - cameraDistanceMin+1);
    }

}
