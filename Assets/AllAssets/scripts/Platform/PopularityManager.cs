using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopularityManager : BaseManager {

    private PopularityStatusEntity popularity = new PopularityStatusEntity();

    public PopularityStatusEntity getPopularity()
    {
        return popularity;
    }

    public void UpdateGame(List<IStatusEntity> list)
    {
        double count = 0;
        double totalHappiness = 0;
        foreach (CityStatusEntity cse in list)
        {
            totalHappiness += cse.happiness;
            ++count;
        }
        popularity.percentHappiness = totalHappiness / count;
    }
}
