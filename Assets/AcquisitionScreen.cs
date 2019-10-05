﻿using Assets.Utils;
using UnityEngine.UI;
using UnityEngine;

public class AcquisitionScreen : View
{
    public Text Title;
    public Text ProposalStatus;

    public Text Offer;
    public Text SellerPrice;

    public AcquisitionButtonView AcquisitionButtonView;

    public Text TriesRemaining;
    public Text DaysRemaining;

    public Toggle KeepFounderAsCEO;

    public Text SharePercentage;

    public InputField CashOfferInput;
    public InputField SharesOfferInput;

    public Slider Slider;

    public override void ViewRender()
    {
        base.ViewRender();

        Title.text = $"Acquisition of company {SelectedCompany.company.Name}";

        var progress = CompanyUtils.GetOfferProgress(GameContext, SelectedCompany.company.Id, MyCompany.shareholder.Id);

        ProposalStatus.text = CompanyUtils.IsCompanyWillAcceptAcquisitionOffer(GameContext, SelectedCompany.company.Id, MyCompany.shareholder.Id) ?
            Visuals.Positive(progress + "%") : Visuals.Negative(progress + "%");

        RenderOffer();
    }

    void RenderOffer()
    {
        string overpriceText = "";

        var cost = EconomyUtils.GetCompanyCost(GameContext, SelectedCompany.company.Id);

        var acquisitionOffer = AcquisitionOffer;

        var conditions = acquisitionOffer.AcquisitionConditions;
        long offer = conditions.BuyerOffer;

        if (offer > cost)
        {
            var overprice = Mathf.Ceil(offer * 10 / cost);
            overpriceText = $"  ({(overprice / 10)}x)";
        }

        Offer.text = Format.Money(offer) + overpriceText;
        SellerPrice.text = Format.Money(conditions.SellerPrice);



        CashOfferInput.text = offer.ToString();
        SharesOfferInput.text = conditions.ByShares.ToString();

        TriesRemaining.text = acquisitionOffer.RemainingTries.ToString();
        DaysRemaining.text = acquisitionOffer.RemainingDays + " days left";

        var isWillSell = CompanyUtils.IsCompanyWillAcceptAcquisitionOffer(GameContext, SelectedCompany.company.Id, MyCompany.shareholder.Id);
        AcquisitionButtonView.SetAcquisitionBid(offer, isWillSell);

        KeepFounderAsCEO.isOn = conditions.KeepLeaderAsCEO;

        var ourCompanyCost = EconomyUtils.GetCompanyCost(GameContext, MyCompany);


        var sharePercent = conditions.ByShares; // ;
        var maxAllowedShareCost = 25 * ourCompanyCost / 100;

        var shareCost = Mathf.Clamp(sharePercent * offer / 100, 0, maxAllowedShareCost);
        sharePercent = (int)(shareCost * 100 / offer);



        Slider.minValue = 0;
        Slider.maxValue = 100;


        Slider.maxValue = Mathf.Clamp(maxAllowedShareCost * 100 / offer, 0, 100);

        var sharePartOfCompany = shareCost * 100 / ourCompanyCost;

        var cash = offer - shareCost;
        SharePercentage.text = $"You will pay {Format.Money(cash)} with cash and give {sharePartOfCompany}% of your company shares (worth ${Format.Money(shareCost)})";
    }

    public void ToggleKeepFounderAsCEO()
    {
        AcquisitionOffer.AcquisitionConditions.KeepLeaderAsCEO = KeepFounderAsCEO.isOn;
    }

    void UpdateData()
    {
        CompanyUtils.TweakAcquisitionConditions(GameContext, SelectedCompany.company.Id, MyCompany.shareholder.Id, Conditions);

        ScreenUtils.UpdateScreenWithoutAnyChanges(GameContext);
    }

    // TODO move to ButtonController classes
    public void IncreaseShareOffer()
    {
        var newConditions = Conditions;

        newConditions.ByShares = Mathf.Clamp(newConditions.ByShares + 1, 0, 25);
        UpdateData();
    }
    public void DecreaseShareOffer()
    {
        var newConditions = Conditions;

        newConditions.ByShares = Mathf.Clamp(newConditions.ByShares - 1, 0, 25);

        UpdateData();
    }

    public void OnSharesOfferEdit()
    {
        var offer = int.Parse(SharesOfferInput.text);

        Conditions.ByShares = (int)Slider.value; // Mathf.Clamp(offer, 0, 25);

        UpdateData();
    }

    public void OnCashOfferEdit()
    {
        var offer = long.Parse(CashOfferInput.text);

        Conditions.BuyerOffer = offer;

        UpdateData();
    }

    AcquisitionConditions Conditions => AcquisitionOffer.AcquisitionConditions;

    AcquisitionOfferComponent AcquisitionOffer
    {
        get
        {
            return CompanyUtils.GetAcquisitionOffer(GameContext, SelectedCompany.company.Id, MyCompany.shareholder.Id).acquisitionOffer;
        }
    }
}
