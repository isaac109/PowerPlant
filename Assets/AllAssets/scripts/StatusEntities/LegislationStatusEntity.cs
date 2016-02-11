using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegislationStatusEntity : BaseStatusEntity
{
    public List<int> politicalPartyOfPresident = new List<int>();
    public List<bool> presidentViews = new List<bool>();
    public double presidentCorruption = 0;
    public List<IStatusEntity> senatorList = new List<IStatusEntity>(); //politician
}
