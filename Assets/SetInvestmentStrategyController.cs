﻿using Assets.Core;

public class SetInvestmentStrategyController : ButtonController
{
    public InvestorInterest InvestorInterest;

    public override void Execute()
    {
        Investments.SetExitStrategy(MyCompany, InvestorInterest);
    }
}
