using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

    public bool overGUI = false;

    private Vector3 dragOrigin;
    Vector3 pos = Vector3.zero;

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
        this.transform.position = new Vector3(gm.cMaxWidth/2, this.transform.position.y, gm.cMaxHeight/2);
        cameraDistanceMax = (float)gm.cwidth * (8f+(2.0f/3.0f)) + 5;
        cameraDistance = cameraDistanceMax;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (canControl)
        {
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null)
                    {
                        pos = hit.point;
                    }
                }
                if (dragOrigin == new Vector3(-100f, -100f, -100f))
                {
                    dragOrigin = pos;
                }
                else
                {
                    this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (dragOrigin.x - pos.x), this.gameObject.transform.position.y, this.gameObject.transform.position.z + (dragOrigin.z - pos.z));
                }
            }
            else
            {
                dragOrigin = new Vector3(-100f, -100f, -100f);
            }

            cameraDistance += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
            this.transform.position = new Vector3(this.transform.position.x, cameraDistance, this.transform.position.z);

            tempCameradistance = cameraDistance;
            cameraWidthMax = (cameraDistance / cameraDistanceMax) * gm.cMaxWidth / 2;
            cameraHeightMax = (cameraDistance / cameraDistanceMax) * gm.cMaxHeight / 2;
            float cBorderPercent = 0;
            if ((cameraDistance / cameraDistanceMax) != 1)
            {
                cBorderPercent = ((100f - ((cameraDistance / cameraDistanceMax) * 100)) / 100) * 6;
            } 
            if (this.transform.position.x <= cameraWidthMax - cBorderPercent)
            {
                this.transform.position = new Vector3(cameraWidthMax - cBorderPercent, this.transform.position.y, this.transform.position.z);
            }
            if (this.transform.position.x > gm.cMaxWidth - cameraWidthMax + cBorderPercent)
            {
                this.transform.position = new Vector3(gm.cMaxWidth - cameraWidthMax+5, this.transform.position.y, this.transform.position.z);
            }
            if (this.transform.position.z < cameraHeightMax - cBorderPercent)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, cameraHeightMax-cBorderPercent);
            }
            if (this.transform.position.z > gm.cMaxHeight - cameraHeightMax + cBorderPercent)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, gm.cMaxHeight - cameraHeightMax + cBorderPercent);
            }
        }
	}
}
