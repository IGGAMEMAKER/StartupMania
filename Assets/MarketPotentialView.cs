﻿using UnityEngine.UI;

public class MarketPotentialView : View
{
    NicheType NicheType;

    public Text PotentialMarketSize;
    public Text PotentialAudienceSize;
    public Text PotentialIncomeSize;
    public Text IterationCost;

    public void SetEntity(NicheType niche)
    {
        NicheType = niche;

        PotentialMarketSize.text = "10M ... 100M";
        PotentialAudienceSize.text = "10M ... 100M";
        PotentialIncomeSize.text = "1$ ... 10$";

        IterationCost.text = "100";
    }
}
