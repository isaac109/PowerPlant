using UnityEngine;
using System.Collections;

public class ResearchDevelopmentUpgradeStatusEntity : BaseStatusEntity
{
    public int patentPrice;
    public int price;
    public int researchStartTurn;
    public int turnsToResearch;
    //0 = unreseached, 1 = patented, 2 = researched
    public int researchState = -1;
}
