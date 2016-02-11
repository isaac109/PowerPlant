using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class buttonFader : MonoBehaviour {

    public bool faded = false;

    Image buttonImage;
    Text txt;
    Color buttonColor;
    Color txtColor;
    bool startFade = false;
    float smooth = 0;
    bool initialized = false;


	// Use this for initialization
	void Start () {
        Initialize();
	}

    void Initialize()
    {
        startFade = false;
        faded = false;
        buttonImage = GetComponent<Image>();
        buttonColor = buttonImage.color;
        if (GetComponentInChildren<Text>())
        {
            txt = GetComponentInChildren<Text>();
            txtColor = txt.color;
        }
        initialized = true;

    }

	// Update is called once per frame
	void Update () {

        if (startFade)
        {
            fade(smooth);
            if (buttonColor.a > 0.9)
            {
                faded = true;
            }
        }
	}
    public void fade(float rate)
    {
        if (!initialized)
        {
            Initialize();
        }
        smooth = rate;
        startFade = true;

        buttonColor.a += rate;
        buttonImage.color = buttonColor;
        if (txt)
        {
            txtColor.a += rate;
            txt.color = txtColor;
        }
    }
}
