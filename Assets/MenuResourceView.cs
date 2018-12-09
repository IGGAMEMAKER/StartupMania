﻿using Assets.Classes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuResourceView : MonoBehaviour {
    bool loaded = false;

    GameObject MoneyView;
    GameObject ProgrammingView;
    GameObject MarketingView;
    GameObject ManagerView;
    GameObject IdeaView;
    GameObject ClientView;
    GameObject ScheduleView;

    void LoadViews()
    {
        MoneyView = gameObject.transform.Find("Money").gameObject;
        ProgrammingView = gameObject.transform.Find("ProgrammingPoints").gameObject;
        MarketingView = gameObject.transform.Find("SalesPoints").gameObject;
        ManagerView = gameObject.transform.Find("ManagerPoints").gameObject;
        IdeaView = gameObject.transform.Find("Ideas").gameObject;
        ClientView = gameObject.transform.Find("Clients").gameObject;
        ScheduleView = gameObject.transform.Find("Date").gameObject;
        loaded = true;
    }
	
    string GetHint<T> (T value)
    {
        string valueSigned = "";

        if (long.Parse(value.ToString()) > 0)
            valueSigned = "+" + value.ToString();
        else
            valueSigned = value.ToString();

        return String.Format("Monthly change \n\n {0}", valueSigned);
    }

    public void RedrawResources(TeamResource teamResource, TeamResource resourceMonthChanges, Audience audience, string currentDate)
    {
        if (!loaded)
            LoadViews();

        string hint;

        // resources
        hint = GetHint(resourceMonthChanges.money);
        MoneyView.GetComponent<ResourceView>()
            .UpdateResourceValue("Money", teamResource.money, hint);

        hint = GetHint(resourceMonthChanges.programmingPoints);
        ProgrammingView.GetComponent<ResourceView>()
            .UpdateResourceValue("Programming Points", teamResource.programmingPoints, hint);

        hint = GetHint(resourceMonthChanges.managerPoints);
        ManagerView.GetComponent<ResourceView>()
            .UpdateResourceValue("Manager Points", teamResource.managerPoints, hint);

        hint = GetHint(resourceMonthChanges.salesPoints);
        MarketingView.GetComponent<ResourceView>()
            .UpdateResourceValue("Marketing points", teamResource.salesPoints, hint);

        hint = GetHint(resourceMonthChanges.ideaPoints);
        IdeaView.GetComponent<ResourceView>()
            .UpdateResourceValue("Ideas", teamResource.ideaPoints, hint);


        // audience
        hint = String.Format(
            "We will lose {0} clients this month due to:\n\n Churn rate: {1}%",
            audience.GetChurnClients(),
            (int) (audience.GetChurnRate() * 100)
        );
        ClientView.GetComponent<ResourceView>()
            .UpdateResourceValue("Clients", audience.clients, hint);

        // date
        ScheduleView.GetComponent<ResourceView>()
            .UpdateResourceValue("Space: Pause/Unpause\n\n +/-: Faster/Slower", currentDate, "Day");
    }
}
