using UnityEngine;
using System.Collections;

public class initializeSizeLocation : MonoBehaviour {

    Vector3 position;
    Vector2 sizeDelta;

    public void initialize()
    {
        position = this.GetComponent<RectTransform>().position;
        Debug.Log(position);
        sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
        Debug.Log(sizeDelta);
    }
    public void reset()
    {
        this.GetComponent<RectTransform>().position = position;
        Debug.Log(position);
        this.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        Debug.Log(sizeDelta);
    }
	
}
