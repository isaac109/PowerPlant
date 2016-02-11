using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerPlantUpgradeStatusEntity : BaseStatusEntity
{
    public double patentedPrice;
    public double price;
    public int turnsToResearch;
    public int turnsToBuild;
    public int researchStartTurn = -1;
    //0 unresearched, 1 patented, 2 researched
    public int researchState = -1;
    public List<IStatusEntity> listOfUpgrades = new List<IStatusEntity>(); //researchdevupgradestatent
}
