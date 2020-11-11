﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionRelay : View
{
    public GameObject ActiveMissions;
    public GameObject NewMissions;

    public GameObject GoalsListView;
    public GameObject PickGoalsListView;

    int myGoalsCounter => MyCompany.companyGoal.Goals.Count;
    int newGoalsCounter => Investments.GetNewGoals(MyCompany, Q).Count;

    public void RenderButtons()
    {
        var myGoals = myGoalsCounter;
        var newGoals = newGoalsCounter;

        ActiveMissions.GetComponentInChildren<TextMeshProUGUI>().text = $"ACTIVE ({myGoalsCounter})";
        NewMissions.GetComponentInChildren<TextMeshProUGUI>().text = $"NEW ({newGoalsCounter})";

        Draw(ActiveMissions, myGoals > 0);
        Draw(NewMissions, newGoals > 0);
    }

    private void OnEnable()
    {
        if (myGoalsCounter != 0)
            ShowActiveMissions();
        else
            ShowNewMissions();
    }

    public void ShowActiveMissions()
    {
        Show(GoalsListView);
        Hide(PickGoalsListView);

        RenderButtons();
    }

    public void ShowNewMissions()
    {
        Hide(GoalsListView);
        Show(PickGoalsListView);

        RenderButtons();
    }
}
