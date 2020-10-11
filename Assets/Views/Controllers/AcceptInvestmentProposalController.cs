﻿using Assets.Core;

public class AcceptInvestmentProposalController : ButtonController
{
    public int InvestorId;

    public override void Execute()
    {
        Companies.AcceptInvestmentProposal(Q, MyCompany, InvestorId);
        Navigate(ScreenMode.InvesmentProposalScreen);
        //ReNavigate();
    }
}
