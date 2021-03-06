﻿using Assets.Core;

public class AudienceGainFromChannelsView : ParameterView
{
    public override string RenderValue()
    {
        var gain = Marketing.GetAudienceGrowth(Flagship, Q);

        if (gain > 0)
        {
            return "Active channels";
        }

        return $"We get {Visuals.Negative("ZERO")} users now.\n\nAdd more channels to get users!";
    }
}
