﻿using Assets.Core;
using System.Linq;

public class LinkToMainNiche : ButtonController
{
    public override void Execute()
    {
        var focus = MyCompany.companyFocus.Niches;

        if (focus.Count == 0)
            return;

        var mostValuableNiche = focus
            .OrderByDescending(n => Companies.GetMarketImportanceForCompany(Q, MyCompany, n))
            .First();

        NavigateToNiche(mostValuableNiche);
    }
}
