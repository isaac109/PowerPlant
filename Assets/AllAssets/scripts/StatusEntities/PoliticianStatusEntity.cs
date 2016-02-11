using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoliticianStatusEntity : BaseStatusEntity
{
    public List<int> politicalPartyList = new List<int>();
    public List<bool> politicianViews = new List<bool>();
    public string candidateName;
    public double corruptionLevel = 0;
    public int politicalParty;
}
