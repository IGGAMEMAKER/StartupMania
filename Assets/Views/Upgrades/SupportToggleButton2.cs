﻿using Assets.Core;

public class SupportToggleButton2 : ProductUpgradeButton
{
    public override string GetButtonTitle() => $"Product support (II)";
    public override string GetBenefits()
    {
        return Visuals.Positive($"-2% Churn");
    }

    public override ProductUpgrade upgrade => ProductUpgrade.Support2;
}
