using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class updateSlider : MonoBehaviour {

    public updateTileUI tileUI;
    public Text price;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnEnabled()
    {
        this.GetComponent<Slider>().value = tileUI.tile.GetComponent<ppHexManager>().powerPrice;
        price.text = "$" + this.GetComponent<Slider>().value.ToString();
    }

    public void getCurrentPrice()
    {
        this.GetComponent<Slider>().value = tileUI.tile.GetComponent<ppHexManager>().powerPrice;
        updateKWH(this.GetComponent<Slider>().value);
    }

    public void updateKWH(float value)
    {
        price.text = "$0." + value.ToString();
        tileUI.tile.GetComponent<ppHexManager>().powerPrice = (int)value;
    }
}
