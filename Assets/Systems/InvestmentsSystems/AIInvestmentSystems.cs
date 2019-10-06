﻿using Assets.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInvestmentSystems : OnHalfYear
{
    public AIInvestmentSystems(Contexts contexts) : base(contexts)
    {
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in CompanyUtils.GetAIManagingCompanies(gameContext))
            TakeInvestments(e);

        foreach (var e in CompanyUtils.GetAIProducts(gameContext))
            TakeInvestments(e);
    }

    bool InvestorIsNotRelatedToPlayer (InvestmentProposal proposal)
    {
        var investor = InvestmentUtils.GetInvestorById(gameContext, proposal.ShareholderId);

        var isRelatedToPlayer = investor.hasCompany && CompanyUtils.IsCompanyRelatedToPlayer(gameContext, investor);

        return !isRelatedToPlayer;
    }

    void TakeInvestments(GameEntity company)
    {
        CompanyUtils.StartInvestmentRound(company, gameContext);

        var companyId = company.company.Id;

        var suitableProposals = CompanyUtils.GetInvestmentProposals(gameContext, companyId)
            .Where(InvestorIsNotRelatedToPlayer);

        foreach (var s in suitableProposals)
        {
            var investorShareholderId = s.ShareholderId;
            var shareholderName = CompanyUtils.GetInvestorName(gameContext, investorShareholderId);

            CompanyUtils.AcceptProposal(gameContext, companyId, investorShareholderId);

            Format.Print($"Took investments from {shareholderName}. Offer: {Format.Money(s.Offer)}", company);
        }
    }
}