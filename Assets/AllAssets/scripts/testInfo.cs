using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testInfo : MonoBehaviour {

    public int numOfOcean = 0;
    public int[] biomeNums = new int[8];
    public int itterationNum = 0;
    public int maxItteration = 10;
    public List<int> itterations = new List<int>();
    public List<int[]> biomeItterations = new List<int[]>();

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(this);
        if (Application.loadedLevel == 0)
        {
            Application.LoadLevel(1);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void newMap()
    {
        //itterations.Add(numOfOcean);
        biomeItterations.Add(biomeNums);
        biomeNums = new int[8];
        //numOfOcean = 0;
        Application.LoadLevel(1);
    }
    public void print()
    {
        /*itterations.Sort();
        int average = 0;
        foreach (int i in itterations)
        {
            average += i;
            Debug.Log(i);
        }
        average /= itterations.Count;
        Debug.Log("avarage" + average);*/
        biomeNums = new int[8];
        for (int i = 0; i < biomeItterations.Count; i++)
        {
            for (int j = 0; j < biomeNums.Length; j++)
            {
                biomeNums[j] += biomeItterations[i][j];
            }
        }
        for (int j = 0; j < biomeNums.Length; j++)
        {
            biomeNums[j] /= biomeItterations.Count;
            Debug.Log("biome: " + j + ": " + biomeNums[j]);
        }
    }
}
