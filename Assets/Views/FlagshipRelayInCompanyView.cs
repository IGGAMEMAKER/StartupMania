﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagshipRelayInCompanyView : View
{
    // tabs
    public GameObject DevelopmentTab;
    public GameObject WorkerInteractions;
    public GameObject InvestmentTabs;
    public GameObject ManagersTabs;

    // selected worker
    bool roleWasSelected = false;
    WorkerRole SelectedWorkerRole;

    public GameObject RemoveTaskButton;
    public ChosenTeamTaskView ChosenTaskLabel;

    // buttons
    public int ChosenTeamId = -1;
    public int ChosenSlotId = 0;

    public void FillSlot(int teamId, int slotId)
    {
        ChosenSlotId = slotId;
        ChosenTeamId = teamId;
    }

    private void OnEnable()
    {
        ChooseWorkerInteractions();
    }

    public void RemoveTask()
    {
        Teams.RemoveTeamTask(Flagship, Q, ChosenTeamId, ChosenSlotId);
        Refresh();
    }

    public void ChooseWorkerInteractions()
    {
        Show(WorkerInteractions);

        Hide(DevelopmentTab);
        Hide(InvestmentTabs);
        Hide(ManagersTabs);
    }

    public void ChooseDevTab()
    {
        Show(DevelopmentTab);

        Hide(WorkerInteractions);
        Hide(InvestmentTabs);
        Hide(ManagersTabs);


        var tasks = Flagship.team.Teams[ChosenTeamId].Tasks;
        var hasTask = ChosenTeamId >= 0 && tasks.Count > ChosenSlotId;

        if (hasTask)
            ChosenTaskLabel.SetTask(tasks[ChosenSlotId]);

        Draw(ChosenTaskLabel, hasTask);
        Draw(RemoveTaskButton, hasTask);
    }

    public void ChooseInvestmentTab()
    {
        Show(InvestmentTabs);

        Hide(WorkerInteractions);
        Hide(DevelopmentTab);
        Hide(ManagersTabs);

        Hide(ChosenTaskLabel);
        Hide(RemoveTaskButton);
    }

    public void ChooseManagersTabs()
    {
        Show(ManagersTabs);

        Hide(WorkerInteractions);
        Hide(DevelopmentTab);
        Hide(InvestmentTabs);

        Hide(ChosenTaskLabel);
        Hide(RemoveTaskButton);
    }


    public bool IsRoleChosen(WorkerRole workerRole)
    {
        return roleWasSelected && SelectedWorkerRole == workerRole;
    }

    public void ToggleRole(WorkerRole role)
    {
        if (role == SelectedWorkerRole)
        {
            // toggling role
            roleWasSelected = !roleWasSelected;
        }
        else
        {
            // click on different role
            roleWasSelected = true;
            SelectedWorkerRole = role;

            // TODO unnecessary?
            //var up = CompanyUpgrades.GetComponent<ProductUpgradeButtons>();
            //up.WorkerRole = role;
            //up.ViewRender();
        }

        // enabled
        if (roleWasSelected)
        {
            ChooseWorkerInteractions();
            ScheduleUtils.PauseGame(Q);
        }
        else
        {
            ChooseDevTab();
            //ScheduleUtils.ResumeGame(Q);
        }

        MarkGameEventsAsSeen(role);

        //EnlargeOnDemand.StartAnimation();
    }


    void ClearEvents(GameEntity eventContainer, List<GameEventType> removableEvents)
    {
        var events = eventContainer.gameEventContainer.Events;

        events.RemoveAll(e => removableEvents.Contains(e.eventType));
        eventContainer.ReplaceGameEventContainer(events);
    }

    void MarkGameEventsAsSeen(WorkerRole role)
    {
        var marketingEvents = new List<GameEventType> { GameEventType.NewMarketingChannel };

        var events = NotificationUtils.GetGameEventContainerEntity(Q);

        if (role == WorkerRole.MarketingLead)
        {
            ClearEvents(events, marketingEvents);
        }
    }
}
