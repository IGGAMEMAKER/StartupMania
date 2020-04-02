﻿using Assets.Core;
using UnityEngine;


public enum TutorialFunctionality
{
    MarketingMenu,
    CompetitorView,
    PossibleInvestors,
    LinkToProjectViewInInvestmentRounds,
    FirstAdCampaign,

    GoalFirstUsers,
    GoalPrototype,

    GoalBecomeMarketFit,
    GoalRelease,

    GoalBecomeProfitable,

    IPO,

    NeverShow,

    CompletedFirstGoal,
    
    ClickOnRaiseMoneyLink,
    ClickOnDevelopmentLink,
    ClickOnGroupLink,
}


public class TutorialController : View
{
    [Tooltip("This components will show up when this tutorial functionality will be unlocked")]
    public TutorialFunctionality TutorialFunctionality;

    public GameObject[] HideableObjects;

    public override void ViewRender()
    {
        base.ViewRender();

        var show = TutorialUtils.IsOpenedFunctionality(Q, TutorialFunctionality);

        foreach (var obj in HideableObjects)
            obj.SetActive(show);
    }
}
