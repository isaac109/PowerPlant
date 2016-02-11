using UnityEngine;
using System.Collections;

public class buttinMoveInfo : MonoBehaviour {

    public int x;
    public int y;
    public gridManager gm;

    public void OnClick()
    {
        gm.zoom(x, y);
    }
}
