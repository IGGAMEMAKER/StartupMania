﻿using Assets.Core;
using System.Collections.Generic;

public class WorkerListView : StaffListView
{
    public override bool DrawAsEmployee() => false;

    public override Dictionary<int, WorkerRole> Workers()
    {
        bool isOnHoldingScreen = CurrentScreen == ScreenMode.HoldingScreen;

        var company = isOnHoldingScreen ? Flagship : SelectedCompany;

        return company.team.Managers;
    }
}
