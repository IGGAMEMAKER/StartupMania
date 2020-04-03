﻿public enum TutorialFunctionality
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


public enum Risk
{
    Guaranteed,
    Risky,
    TooRisky
}

public enum ScreenMode
{
    DevelopmentScreen = 0,
    MarketingScreen = 1, // deprecated
    ProjectScreen = 2,
    TeamScreen = 3,
    StatsScreen = 4,
    CharacterScreen = 5,
    GroupManagementScreen = 6,
    InvesmentsScreen = 7,
    InvesmentProposalScreen = 8,
    IndustryScreen = 9,
    NicheScreen = 10,
    InvestmentOfferScreen = 11,
    JobOfferScreen = 12,
    CompanyGoalScreen = 13,
    EmployeeScreen = 14,
    BuySharesScreen = 15,
    ManageCompaniesScreen = 16,
    CompanyEconomyScreen = 17,

    MarketExplorationScreen = 18,
    CompanyExplorationScreen = 19,
    LeaderboardScreen = 20,

    ExplorationScreen = 21,
    NicheInfoScreen = 22,
    AcquisitionScreen = 23,
    SalesScreen = 24,
    PotentialCompaniesScreen = 25,
    AnnualReportScreen = 26,

    StartCampaignScreen = 27,
    GroupScreen = 28,
    HoldingScreen = 29,
    CorporationScreen = 30,
    AcquirableCompaniesOnNicheScreen = 31,
    JoinCorporationScreen = 32,
    FormStrategicPartnershipScreen = 33,
    TrendsScreen = 34,
    MessageScreen = 35
}

public struct ProductCompanyResult
{
    public long clientChange;
    public float MarketShareChange;
    public ConceptStatus ConceptStatus;
    public int CompanyId;
}