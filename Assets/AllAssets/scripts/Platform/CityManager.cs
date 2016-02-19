using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CityManager : BaseManager {

    private List<IStatusEntity> list = new List<IStatusEntity>();
    public double worldPopulation = 0;

    public void setCityList(List<IStatusEntity> list)
    {
        this.list = list;
    }

    public List<IStatusEntity> getCityList()
    {
        return list;
    }

    public void UpdateGame(List<IStatusEntity> list)
    {
        worldPopulation = 0;
        this.list = list;
        foreach (CityStatusEntity cse in list)
        {
            worldPopulation += cse.population;
            if (cse.mayor == null)
            {
                cse.mayor.politicalPartyList.Add(1);//o
                cse.mayor.politicalPartyList.Add(2);//y
                cse.mayor.politicalPartyList.Add(3);//d
                cse.mayor.politicalParty = UnityEngine.Random.Range(0, 3);

                for (int i = 0; i < 50; i++)
                {
                    cse.mayor.politicianViews.Add(Convert.ToBoolean(UnityEngine.Random.Range(0,1)));
                }

                //TODO: Implement Candidate Name
            }
            if (cse.powerStatus == -1)
            {
                cse.powerStatus = 0;
            }
        }
    }

    public List<IStatusEntity> GetGame()
    {
        return null;
    }
}
