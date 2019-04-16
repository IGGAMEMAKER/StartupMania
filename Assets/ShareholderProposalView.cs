﻿using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ShareholderProposalView : View
{
    public Image Panel;
    public Text Name;
    public Image Icon;

    public Hint Motivation;
    public Text InvestorType;

    public Text Offer;

    public GameObject AcceptProposal;
    public GameObject RejectProposal;

    public Text Valuation;

    GameEntity shareholder;
    GameEntity company;

    public void SetEntity(InvestmentProposal proposal)
    {
        shareholder = CompanyUtils.GetInvestorById(GameContext, proposal.ShareholderId);
        company = SelectedCompany;

        Render(shareholder.shareholder.Name, proposal);
    }

    void SetButtons(int investorId)
    {
        AcceptProposal.SetActive(investorId != MyGroupEntity.shareholder.Id);

        AcceptProposal.GetComponent<BuyShares>().ShareholderId = investorId;
        AcceptProposal.GetComponent<CanBuySharesController>().Render(investorId);
    }

    void Render(string name, InvestmentProposal proposal)
    {
        Name.text = name;
        InvestorType.text = "Venture investor";
        Motivation.SetHint("Motivation: 20% growth");

        long Cost = CompanyUtils.GetCompanyCost(GameContext, company.company.Id);

        long offer = proposal.Offer;
        long futureShareSize = offer * 100 / (offer + Cost);


        Offer.text = $"${ValueFormatter.Shorten(offer)} ({futureShareSize}%)"; // CompanyUtils.GetShareSize(GameContext, SelectedCompany.company.Id, investorId) + "%";

        long valuation = proposal.Valuation;
        Valuation.text = "$" + ValueFormatter.Shorten(valuation);

        SetButtons(proposal.ShareholderId);
    }
}
