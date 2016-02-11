using UnityEngine;
using System.Collections;

public class EnvironmentManager : BaseManager {

    private EnvironmentStatusEntity environment = new EnvironmentStatusEntity();

    public EnvironmentStatusEntity getEnvironment()
    {
        return environment;
    }
}
