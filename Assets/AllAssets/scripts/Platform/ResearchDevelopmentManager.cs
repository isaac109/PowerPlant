using UnityEngine;
using System.Collections;

public class ResearchDevelopmentManager : BaseManager {

    private ResearchDevelopmentStatusEntity rdse = new ResearchDevelopmentStatusEntity();

    public void setResearchAndDevelopment(ResearchDevelopmentStatusEntity rdse)
    {
        this.rdse = rdse;
    }
    
    public ResearchDevelopmentStatusEntity getResearchAndDevelopment()
    {
        return rdse;
    }

    public void UpdateGame ()
    {
        foreach (PowerPlantUpgradeStatusEntity ppuse in rdse.listOfPowerPlantUpgrades)
        {

        }
    }
}
