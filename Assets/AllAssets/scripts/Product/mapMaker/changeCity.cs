using UnityEngine;
using System.Collections;

public class changeCity : MonoBehaviour {

    public mapMakerTile.cities size;
    public mapMakerChangeMenu mmChangeMenu;

    public void change()
    {
        mmChangeMenu.changeCity(size);
    }
}
