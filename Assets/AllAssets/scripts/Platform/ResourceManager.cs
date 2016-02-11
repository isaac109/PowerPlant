using UnityEngine;
using System.Collections;

public class ResourceManager : BaseManager {

    private ResourceStatusEntity resource = new ResourceStatusEntity();

    public ResourceStatusEntity getResources()
    {
        return resource;
    }

}
