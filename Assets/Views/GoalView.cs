﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalView : View
{
    public Text Title;
    public Text Description;
    public ProgressBar ProgressBar;
    public GameObject Users;

    public GameObject CompetitionPanel;

    public override void ViewRender()
    {
        base.ViewRender();

        var clients = Marketing.GetClients(Flagship);
        var goal = Flagship.companyGoal.InvestorGoal;

        var targetAudience = Flagship.productTargetAudience.SegmentId;
        var loyalty = Marketing.GetSegmentLoyalty(Q, Flagship, targetAudience);

        var requirements = Investments.GetGoalRequirements(Flagship, Q);
        var req = requirements[0];

        var audienceInfos = Marketing.GetAudienceInfos()[targetAudience];


        switch (goal)
        {
            case InvestorGoal.Prototype:
                SetPanel("Make test audience loyal", req, "Loyalty");
                break;

            case InvestorGoal.BecomeMarketFit:
                SetPanel("Make audience extremely loyal", req, "Loyalty");
                break;

            case InvestorGoal.FirstUsers:
                SetPanel($"Accumulate {Format.Minify(req.need)} users", req, "Users");
                break;

            case InvestorGoal.Release:
                SetPanel("Release your product!", req, "Is not released");
                break;

            case InvestorGoal.BecomeProfitable:
            case InvestorGoal.Operationing:
                //SetPanel("Increase your income", req, $"Income from product");
                ShowCompetitionPanel();
                break;

            default:
                SetPanel("Default goal", req, goal.ToString());
                break;
        }
    }

    void ShowGoalPanel()
    {
        Show(Description);
        Show(ProgressBar);
        Show(Title);
        Show(Users);
        Hide(CompetitionPanel);
    }

    void ShowCompetitionPanel()
    {
        Hide(Description);
        Hide(ProgressBar);
        Hide(Title);
        Hide(Users);
        Show(CompetitionPanel);
    }

    void SetPanel(string title, GoalRequirements req, string tag)
    {
        SetPanel(title, req.have, req.need, tag);
    }
    void SetPanel(string title, long have, long requirement, string tag)
    {
        ShowGoalPanel();

        Description.text = title;
        ProgressBar.SetValue(have, requirement);
        ProgressBar.SetDescription(tag);
    }
}