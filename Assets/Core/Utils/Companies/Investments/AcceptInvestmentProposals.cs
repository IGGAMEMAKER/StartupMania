﻿namespace Assets.Core
{
    partial class Companies
    {
        public static void AcceptInvestmentProposal(GameContext gameContext, GameEntity company, int investorId)
        {
            var p = GetInvestmentProposal(company, investorId);

            // calculating new shares size
            long cost = Economy.GetCompanyCost(gameContext, company);

            var allShares = (long)GetTotalShares(company);
            long shares = allShares * p.Investment.Offer / cost;

            // update shareholders list
            var investor = GetInvestorById(gameContext, investorId);
            AddShareholder(company, investor, (int)shares);


            var portion = p.Investment.Portion;
            Economy.IncreaseCompanyBalance(company, portion);
            Economy.DecreaseInvestmentFunds(investor, portion);

            MarkProposalAsAccepted(company, investorId);
        }

        static void MarkProposalAsAccepted(GameEntity company, int investorId)
        {
            var proposals = GetInvestmentProposals(company);

            var index = proposals.FindIndex(p => p.ShareholderId == investorId);

            proposals[index].WasAccepted = true;

            var investments = company.shareholders.Shareholders[investorId].Investments;

            if (investments == null)
                investments = new System.Collections.Generic.List<Investment>();

            investments.Add(proposals[index].Investment);

            company.ReplaceInvestmentProposals(proposals);
        }
    }
}
