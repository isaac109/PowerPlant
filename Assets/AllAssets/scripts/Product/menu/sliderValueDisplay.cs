using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class sliderValueDisplay : MonoBehaviour {

    public Text value;

    public void updateValue(float val)
    {
        if (val == 0 || val == null)
        {
            value.text = "0";
        }
        else
        {
            value.text = (val * 1000).ToString();
        }
    }
}
