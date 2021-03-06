﻿using System.Collections;
using System.Collections.Generic;
using Assets.Core;
using UnityEngine;
using UnityEngine.UI;

public class RenderProductGrowthKPI : View
{
    public Text AudienceChange;
    public Text Growth;
    public Text Churn;

    public override void ViewRender()
    {
        base.ViewRender();
        
        var product = SelectedCompany;

        if (!product.hasProduct)
            return;
        
        var growthBonus = Marketing.GetAudienceGrowthBonus(product);
        var growth = growthBonus.Sum();

        var amountOfChannels = growthBonus.bonusDescriptions.Count;
        
        Growth.text = Visuals.Positive(Format.Minify(growth) + $" users weekly\n\nActive in {amountOfChannels} channels"); //growthBonus.ToString()

        long churnUsers = 0;
        var segments = Marketing.GetAudienceInfos();

        var churnText = "";
        for (var i = 0; i < segments.Count; i++)
        {
            var churnInSegment = Marketing.GetChurnClients(product, i);
            churnUsers += churnInSegment;

            var churn = Marketing.GetChurnRate(product, i, true).SetTitle("Churn for " + segments[i].Name);

            bool IsSomewhatInterestedInSegment = Marketing.IsAimingForSpecificAudience(product, i) || churnInSegment > 0;

            if (churn.Sum() > 0 && IsSomewhatInterestedInSegment)
            {
                churnText += $"\n{churn.ToString(true)}";
            }
            //churnText += $"\n{churn.ToString(true)}";
        }

        Churn.text = Visuals.Negative(Format.Minify(churnUsers) + " users weekly\n") + churnText;

        var change = Marketing.GetAudienceChange(product, Q);
        AudienceChange.text = Visuals.PositiveOrNegativeMinified(change);
    }
}