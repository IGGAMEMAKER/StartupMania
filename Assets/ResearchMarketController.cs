﻿using Assets.Utils;

public class ResearchMarketController : ButtonController
{
    public override void Execute()
    {
        var niche = NicheUtils.GetNicheEntity(GameContext, SelectedNiche);

        var res = new Assets.Classes.TeamResource(0, 15, 0, 0, 0);
        //CooldownUtils.AddCooldownAndSpendResources(GameContext, MyCompany, CooldownType.MarketResearch, 15, res);

        niche.AddResearch(1);
    }
}