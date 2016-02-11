using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerPlantStatusEntity : BaseStatusEntity
{
    public ResourceDrainStatusEntity resourceDrain = new ResourceDrainStatusEntity();
    public int typeOfPowerPlant;
    public double powerCostPerTurn;
    public double powerOutput;
    public int buildStartTurn;
    public int turnsToBuild;
    public bool built;
    List<bool> listOfUpgrades = new List<bool>();
    List<IStatusEntity> citiesPowered = new List<IStatusEntity>();
}
