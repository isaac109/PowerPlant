using UnityEngine;
using System.Collections;

public class LegislationManager : BaseManager {

    private LegislationStatusEntity legislation = new LegislationStatusEntity();

    public void setLegislation(LegislationStatusEntity legislation)
    {
        this.legislation = legislation;
    }
    public LegislationStatusEntity getLegislation()
    {
        return legislation;
    }
}
