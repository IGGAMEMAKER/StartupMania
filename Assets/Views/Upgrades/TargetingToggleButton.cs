﻿using Assets.Core;

public class TargetingToggleButton : ProductUpgradeButton
{
    public override string GetButtonTitle() => $"Targeting campaign";
    public override string GetBenefits()
    {
        var clients = Marketing.GetTargetingCampaignGrowth(Flagship, Q);

        return Visuals.Positive($"+{clients}") + " users";
    }

    public override ProductUpgrade upgrade => ProductUpgrade.TargetingCampaign;
}