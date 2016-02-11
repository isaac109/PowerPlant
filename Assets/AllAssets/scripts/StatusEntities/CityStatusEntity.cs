using UnityEngine;
using System.Collections;

public class CityStatusEntity : BaseStatusEntity, IStatusEntity
{
    public double population = 0;
    public double happiness = 50;
    public PoliticianStatusEntity mayor;
    //0 = unpowered, 1 = underpowered, 2 = powered
    public int powerStatus = -1;
    //0 = small, 1 = med, 2 = large
    public int citySize = -1;
}
