﻿using Assets.Utils.Tutorial;
using UnityEngine;


public enum TutorialFunctionality
{
    MarketingMenu,
    CompetitorView,
    PossibleInvestors,
    LinkToProjectViewInInvestmentRounds,
    FirstAdCampaign,

    FirstUsers,
    Prototype
}

public class TutorialController : View
{
    [Tooltip("This components will be hidden until this tutorial functionality will be unlocked")]
    public TutorialFunctionality TutorialFunctionality;

    public GameObject[] HideableObjects;

    public override void ViewRender()
    {
        base.ViewRender();

        var show = TutorialUtils.IsOpenedFunctionality(GameContext, TutorialFunctionality);

        foreach (var obj in HideableObjects)
            obj.SetActive(show);
    }
}
