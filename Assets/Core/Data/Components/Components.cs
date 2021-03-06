﻿using Entitas;
using Entitas.CodeGeneration.Attributes;
using System.Collections.Generic;

public interface IEventListener
{
    void RegisterListeners(IEntity entity);
}

[Game]
public class UniversalListenerComponent : IComponent { }

[Game, Event(EventTarget.Any), Event(EventTarget.Self)]
public class DateComponent : IComponent
{
    public int Date;
    //public int Speed;
}

[Game]
public class SpeedComponent : IComponent
{
    public int Speed;
}



[Game, Event(EventTarget.Any)]
public class TimerRunningComponent : IComponent { }

[Game, Event(EventTarget.Any)]
public class GamePausedComponent : IComponent { }

[Game, Unique, Event(EventTarget.Any)]
public class TargetDateComponent : IComponent
{
    public int Date;
}



[Game, Event(EventTarget.Self)]
public class MenuComponent : IComponent
{
    public ScreenMode ScreenMode;
    public Dictionary<string, object> Data;
}

// only entity
[Game, Event(EventTarget.Self)]
public class NavigationHistoryComponent : IComponent
{
    public List<MenuComponent> Queries;
}


[Game, Event(EventTarget.Self)]
public class TutorialComponent : IComponent
{
    public Dictionary<TutorialFunctionality, bool> progress;
}

[Game, Event(EventTarget.Self)]
public class EventContainerComponent : IComponent
{
    public Dictionary<string, bool> progress;
}



public class GameEvent
{
    public GameEventType eventType;
}

public class WorkerSpecificGameEvent: GameEvent
{
    public WorkerRole WorkerRole;
    public WorkerSpecificGameEvent(WorkerRole workerRole)
    {
        this.WorkerRole = workerRole;
    }
}

public class GameEventNewMarketingChannel : GameEvent
{
}

public class GameEventManagerNeedsNewSalary : WorkerSpecificGameEvent
{
    public GameEventManagerNeedsNewSalary(WorkerRole workerRole) : base(workerRole)
    {

    }
}

public enum GameEventType
{
    NewMarketingChannel,

    ManagerNeedsBiggerSalary, // specify role?
    ManagerUpgradedLevel,
    ManagerGotContractFromCompetingCompany,

    DisloyalCEO,
    DisloyalTeamLead,
}

[Game, Event(EventTarget.Any)]
public class GameEventContainerComponent : IComponent
{
    // example

    // newChannel => false means that player got new event, but didn't see it
    // newChannel => true means that player got new event, AND seen it
    public List<GameEvent> Events;
}

public enum CampaignStat
{
    Acquisitions,
    Bankruptcies,

    SpawnedFunds,
    PromotedCompanies
}

public class CampaignStatsComponent : IComponent
{
    public Dictionary<CampaignStat, int> Stats;
}

[Game]
public class TestComponent : IComponent
{
    public Dictionary<LogTypes, bool> logs;
}

public enum LogTypes
{
    MyProductCompany,
    MyProductCompanyCompetitors
}

// TODO game data. move somewhere else
[Game]
public class ResearchComponent : IComponent
{
    public int Level;
}

public class OwningsComponent : IComponent
{
    public List<int> Holdings;
}

public class ShareholderComponent : IComponent
{
    public int Id;
    public string Name;
    public InvestorType InvestorType;
}

public enum InvestorBonus
{
    None,
    Expertise,
    Branding,
}

public class Investment
{
    public long Offer;
    public int Duration; // int in months
    public long Portion;

    public InvestmentGoal InvestmentGoal;

    public int RemainingPeriods; // int in periods
    public int StartDate; // you will not get money before that date

    public Investment(long Offer, int Duration, InvestmentGoal investmentGoal, int StartDate)
    {
        this.Offer = Offer;
        this.Duration = Duration;

        this.StartDate = StartDate;

        RemainingPeriods = Duration * 4;

        if (RemainingPeriods > 0)
        {
            Portion = Offer / RemainingPeriods;
        }
        else
        {
            Portion = Offer;
        }

        InvestmentGoal = investmentGoal;
    }
}


public class InvestmentProposal
{
    public int ShareholderId;

    public Investment Investment;

    public bool WasAccepted;

    //public InvestmentGoal InvestmentGoal;
    public int AdditionalShares;
}