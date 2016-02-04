using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testInfo : MonoBehaviour {

    public int numOfOcean = 0;
    public int itterationNum = 0;
    public int maxItteration = 10;
    public List<int> itterations = new List<int>();

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
        itterations.Add(numOfOcean);
        numOfOcean = 0;
        Application.LoadLevel(1);
    }
    public void print()
    {
        itterations.Sort();
        int average = 0;
        foreach (int i in itterations)
        {
            average += i;
            Debug.Log(i);
        }
        average /= itterations.Count;
        Debug.Log("avarage" + average);
    }
}
