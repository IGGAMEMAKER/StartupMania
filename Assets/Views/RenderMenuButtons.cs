﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderMenuButtons : View
{
    public GameObject Main;
    public GameObject Date;
    public GameObject Cash;
    public GameObject Stats;
    public GameObject Messages;

    public GameObject Culture;
    public Image CultureIcon;

    public GameObject Investments;
    public Image InvestmentsIcon;

    public GameObject Separator1;

    public GameObject Quit;

    public override void ViewRender()
    {
        base.ViewRender();

        bool hasProduct = Companies.IsHasDaughters(Q, MyCompany);
        bool hasReleasedProducts = Companies.IsHasReleasedProducts(Q, MyCompany);

        bool isFirstYear = CurrentIntDate < 360;

        bool showStats = false && !isFirstYear;
        bool showMessages = false && hasProduct;

        Main.SetActive(hasProduct);
        Stats.SetActive(showStats);

        // messages
        Messages.SetActive(showMessages);

        // culture
        var hasCultureCooldown = Cooldowns.HasCorporateCultureUpgradeCooldown(Q, MyCompany);
        CultureIcon.color = Visuals.GetColorFromString(hasCultureCooldown ? Colors.COLOR_NEUTRAL : Colors.COLOR_POSITIVE);
        Culture.SetActive(false && hasProduct && hasReleasedProducts && !hasCultureCooldown);


        // investments
        bool isRoundActive = MyCompany.hasAcceptsInvestments;
        var canRaiseInvestments = !isRoundActive;
        //InvestmentsIcon.color = Visuals.GetColorFromString(canRaiseInvestments ? Colors.COLOR_NEUTRAL : Colors.COLOR_POSITIVE);
        //Investments.SetActive(false && hasProduct && canRaiseInvestments);
    }
}
