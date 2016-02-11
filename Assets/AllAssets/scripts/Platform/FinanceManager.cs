using UnityEngine;
using System.Collections;

public class FinanceManager : BaseManager {

    private FinanceStatusEntity finance = new FinanceStatusEntity();

    public FinanceStatusEntity getFinances()
    {
        return finance;
    }
}
