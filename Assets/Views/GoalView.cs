﻿using Assets.Core;
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

        var clients = Marketing.GetUsers(Flagship);
        var goals = Flagship.companyGoal.Goals;

        if (goals.Count == 0)
            return;

        var goal = goals[0];

        //var requirements = Investments.GetGoalRequirements(Flagship, Q, goal);
        //var req = requirements[0];

        //switch (goal.InvestorGoalType)
        //{
        //    case InvestorGoalType.ProductPrototype:
        //        SetPanel("Make test audience loyal", req, "Loyalty");
        //        break;

        //    case InvestorGoalType.ProductBecomeMarketFit:
        //        SetPanel("Make audience extremely loyal", req, "Loyalty");
        //        break;

        //    case InvestorGoalType.ProductFirstUsers:
        //        SetPanel($"Accumulate {Format.Minify(req.need)} users", req, "Users");
        //        break;

        //    case InvestorGoalType.ProductRelease:
        //        SetPanel("Release your product!", req, "Is not released");
        //        break;

        //    case InvestorGoalType.BecomeProfitable:
        //    case InvestorGoalType.Operationing:
        //        //SetPanel("Increase your income", req, $"Income from product");
        //        ShowCompetitionPanel();
        //        break;

        //    default:
        //        SetPanel("Default goal", req, goal.ToString());
        //        break;
        //}
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
