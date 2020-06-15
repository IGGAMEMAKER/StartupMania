﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamView : View
{
    public Text TeamName;
    public TeamType TeamType;
    public GameObject RemoveTeam;

    public TeamInfo TeamInfo;

    public AddTeamTaskListView AddTeamTaskListView;
    public TeamTaskListView TeamTaskListView;

    public void SetEntity(TeamInfo info, int teamId)
    {
        TeamInfo = info;
        TeamType = info.TeamType;

        RemoveTeam.GetComponent<RemoveTeamController>().TeamId = teamId;
        TeamName.text = info.Name;

        var max = 5;

        var company = Flagship;

        var chosenSlots = company.team.Teams[teamId].Tasks.Count;
        var freeSlots = max - chosenSlots;

        AddTeamTaskListView.FreeSlots = freeSlots;
        AddTeamTaskListView.SetEntity(teamId);

        TeamTaskListView.ChosenSlots = chosenSlots;
        TeamTaskListView.SetEntity(teamId);
    }
}