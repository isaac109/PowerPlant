using UnityEngine;
using System.Collections;

public class changeModifier : MonoBehaviour {

    public mapMakerTile.modifiers mod;
    public mapMakerChangeMenu mmChangeMenu;

    public void change()
    {
        mmChangeMenu.changeMod(mod);
    }
}
