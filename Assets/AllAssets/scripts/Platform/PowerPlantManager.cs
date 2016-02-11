using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerPlantManager : BaseManager {

    private List<IStatusEntity> list = new List<IStatusEntity>();//ppse

    public void setPowerPlantList(List<IStatusEntity> list)
    {
        this.list = list;
    }

    public List<IStatusEntity> getPowerPlantList()
    {
        return list;
    }
}
