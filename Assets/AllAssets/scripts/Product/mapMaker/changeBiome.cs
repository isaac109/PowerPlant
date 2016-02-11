using UnityEngine;
using System.Collections;

public class changeBiome : MonoBehaviour {

    public mapMakerTile.biomes biome;
    public mapMakerChangeMenu mmChangeMenu;

    public void change()
    {
        mmChangeMenu.changeBiomes(biome);
    }
}
