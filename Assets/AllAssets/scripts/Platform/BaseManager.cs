using UnityEngine;
using System.Collections;

public abstract class BaseManager : MonoBehaviour {

    protected int turnNumber = 0;
    protected BaseStatusEntity bse = new BaseStatusEntity();
    public BaseManager()
    {

    }                                                                                                                                                                                                                                                                       

    public virtual string ToString()
    {
        return null;
    }

    public virtual void VerifyIntegrity(IStatusEntity ise)
    {

    }

}
