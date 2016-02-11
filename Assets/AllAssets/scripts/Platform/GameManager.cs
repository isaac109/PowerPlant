using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : BaseManager {

    private CityManager cityManager = null;
    private EnvironmentManager environmentManager = null;
    private FinanceManager financeManager = null;
    private LegislationManager legislationManager = null;
    private PopularityManager popularityManager = null;
    private PowerPlantManager powerPlantManager = null;
    private ResearchDevelopmentManager researchDevelopmentManager = null;
    private ResourceManager resourceManager = null;
    private bool management = false;
    private int count = 0;

    void Update ()
    {
        while (!management)
        {
            UpdateManagers();
            if (count >= 600)
            {
                management = true;
            }
        }

    }

    public void StartGame(List<IStatusEntity> cityList, ResearchDevelopmentStatusEntity rdse)
    {
        cityManager.UpdateGame(cityList);
        popularityManager.UpdateGame(cityManager.getCityList());
        researchDevelopmentManager.setResearchAndDevelopment(rdse);
        researchDevelopmentManager.UpdateGame();

    }

    public void UpdateGameState(List<IStatusEntity> cityList, ResearchDevelopmentStatusEntity rdse, LegislationStatusEntity legse, List<IStatusEntity> powerPlantList)
    {
    
    }

    public List<List<IStatusEntity>> GetGameState()
    {
        //List<IStatusEntity> listise = new List<IStatusEntity>();
        //listise.Add(new CityStatusEntity());
        //List<List<IStatusEntity>> listlist = new List<List<IStatusEntity>>();
        //listlist.Add(new List<IStatusEntity>());
        //listlist[0].Add(new CityStatusEntity);
        
        return null;
    }




    private void UpdateManagers()
    {
        cityManager = this.gameObject.GetComponent<CityManager>();
        environmentManager = this.gameObject.GetComponent<EnvironmentManager>();
        financeManager = this.gameObject.GetComponent<FinanceManager>();
        legislationManager = this.gameObject.GetComponent<LegislationManager>();
        popularityManager = this.gameObject.GetComponent<PopularityManager>();
        powerPlantManager = this.gameObject.GetComponent<PowerPlantManager>();
        researchDevelopmentManager = this.gameObject.GetComponent<ResearchDevelopmentManager>();
        resourceManager = this.gameObject.GetComponent<ResourceManager>();
        management = true;
        ++count;
    }
}
