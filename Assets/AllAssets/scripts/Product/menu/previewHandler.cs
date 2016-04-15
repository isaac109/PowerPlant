using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class previewHandler : MonoBehaviour {

    public updateTileUI tileUI;
    public hexTile2 tile;

    public Sprite[] biomes = new Sprite[9];

	void Update () {
        tile = tileUI.tile;
        switch (tile.biome)
        {
            case hexTile2.biomes.MOUNTAIN:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite= biomes[0];
                break;
            case hexTile2.biomes.DESERT:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[1];
                break;
            case hexTile2.biomes.PLAINS:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[2];
                break;
            case hexTile2.biomes.VALLEY:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[3];
                break;
            case hexTile2.biomes.HILLS:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[4];
                break;
            case hexTile2.biomes.MARSHES:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[5];
                break;
            case hexTile2.biomes.FOREST:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[6];
                break;
            case hexTile2.biomes.TUNDRA:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[7];
                break;
            case hexTile2.biomes.OCEAN:
                this.gameObject.transform.Find("Biome").GetComponent<Image>().sprite = biomes[8];
                break;

        }
	}
}
