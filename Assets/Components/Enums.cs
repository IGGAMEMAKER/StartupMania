﻿// advices

// iterate
// foreach (InvestorGoal goal in (InvestorGoal[])Enum.GetValues(typeof(InvestorGoal)))

// cast
// https://stackoverflow.com/questions/29482/cast-int-to-enum-in-c-sharp
// var investorGoal = (InvestorGoal) Enum.Parse(typeof(InvestorGoal), arg0.ToString());

// cast 2
// https://stackoverflow.com/questions/3260762/can-i-cast-from-a-generic-type-to-an-enum-in-c


public enum NicheType
{
    //None,

    SocialNetwork,
    Messenger,
    Dating,
    Blogs,
    Forums,
    Email,


    SearchEngine,

    CloudComputing,

    OSDesktop,
    //OSSciencePurpose,
}

public enum IndustryType
{
    Communications,
    Fundamental,
    //OS,
    //CloudComputing
}

public enum CompanyType
{
    ProductCompany,

    FinancialGroup,

    Group,
    Holding,
    Corporation
}

public enum CooldownType
{
    TargetingActivity,
    
    // fame or loyalty
    MarketingFocus,

    // loyalty, ideas, 
    ProductFocus,

    // ads
    BrandingCampaign,
    TestCampaign,

    CompanyGoal,

    // TODO REMOVE
    StealIdeas,

    ImproveSegment
}

public class Cooldown
{
    public CooldownType CooldownType;
    public int EndDate;

    public bool Compare (CooldownType cooldownType)
    {
        return Compare(new Cooldown { CooldownType = cooldownType });
    }

    public bool Compare (Cooldown cooldown)
    {
        if (CooldownType != cooldown.CooldownType)
            return false;

        return IsEqual(cooldown);
    }

    public virtual bool IsEqual(Cooldown comparable)
    {
        return true;
    }
}

public class CooldownStealIdeas : Cooldown
{
    public int targetCompanyId;

    public CooldownStealIdeas(int targetId)
    {
        targetCompanyId = targetId;
        CooldownType = CooldownType.StealIdeas;
    }

    public override bool IsEqual(Cooldown comparable)
    {
        return (comparable as CooldownStealIdeas).targetCompanyId == targetCompanyId;
    }
}

public class CooldownImproveSegment : Cooldown
{
    public UserType userType;

    public CooldownImproveSegment(UserType userType)
    {
        this.userType = userType;
        CooldownType = CooldownType.ImproveSegment;
    }

    public override bool IsEqual(Cooldown comparable)
    {
        return (comparable as CooldownImproveSegment).userType == userType;
    }
}


public enum InvestorType
{
    // both value long term solutions
    // aka team effeciency
    // product quality
    // branding
    // Founder - human with strategic goals, Strategic - company with strategic goals
    Founder,

    // round d
    Strategic,

    // gives money and wants more investors to come (aka rounds) round a, b, c
    VentureInvestor,

    // wants dividends
    StockExchange,

    // seed
    Angel,

    // friends family fools - preseed
    FFF
}

public enum InvestorGoal
{
    Prototype,
    FirstUsers,

    BecomeMarketFit,
    Release,
    BecomeProfitable,
        //BecomeBestByTech, // 
        //GrowClientBase, // 

        //ProceedToNextRound, // 

    GrowCompanyCost, // +20%
    //GrowProfit, // +10%

    IPO,
    
    // products only
    Operationing
}

public enum InvestmentRound
{
    Preseed,
    Seed,
    A,
    B,
    C,
    D,
    E
}


public enum UserType
{
    Core,
    Regular,
    Mass
}

public enum Pricing
{
    Free = 0,
    Low = 100,
    Medium = 170,
    High = 200
}

public enum MarketingFinancing
{
    Low,
    Medium,
    High
}
