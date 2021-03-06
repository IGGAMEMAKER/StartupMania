﻿using Assets.Core;
using System.Linq;

public class AudienceDetailsView : ParameterView
{
    int segmentId;
    public override string RenderValue()
    {
        var text = "";

        var audienceInfos = Marketing.GetAudienceInfos();

        var company = Flagship;

        var info = audienceInfos[segmentId];

        var incomePerUser = (double)Economy.GetBaseIncomeByMonetizationType(company);

        var maxIncome = (long)(info.Size * incomePerUser);

        var audienceIsPrimary = Marketing.IsTargetAudience(company, segmentId);
        var audienceColor = audienceIsPrimary ? Colors.COLOR_GOLD : Colors.COLOR_WHITE;

        var primaryAudience = audienceIsPrimary ? " (Our target audience)" : "";
        text = $"<b>" +
            $"{Visuals.Colorize(info.Name, audienceColor)}" +
            $"{primaryAudience}" +
            $"</b>";
        text += $"\n\n<b>Potential Income (Audience)</b>" +
            $"\n{Format.Money(maxIncome, true)} ({Format.Minify(info.Size)} users)";
        //text += $"<b>Audience specs</b>";

        foreach (var b in info.Bonuses)
        {
            if (b.Max == 0)
                continue;

            var isGood = b.Max >= 0;


            if (b.isAcquisitionFeature)
            {
                //var value = Visuals.Colorize(b.Max.ToString("0.0") + "%", isGood);
                var value = Visuals.Colorize(b.Max.ToString("+#.#;-#.#;0") + "%", isGood);

                text += "\n\n<b>Growth</b>\n" + value;
            }

            if (b.isMonetisationFeature)
            {
                //var value = Visuals.Colorize(b.Max.ToString("0.0"), isGood);
                var value = b.Max.ToString("0.0");
                var income = incomePerUser * (100f + b.Max) / 100f;

                //text += "\n\n<b>Monetisation</b>\n" + income.ToString("+#.##;-#.##;0") + "$ / user";
                text += "\n\n<b>Monetisation</b>\n" + income.ToString("0.00") + "$ / user";
            }

            //if (b.isRetentionFeature)
            //{
            //    text += "\nLoyalty: " + value;
            //}
        }

        text += "\n\n";

        var companies = Companies.GetCompetitorsOf(company, Q, false).Where(c => Marketing.IsTargetAudience(c, segmentId));
        var expenses = companies.Select(c => Economy.GetProductMaintenance(c, Q));

        var maxBudget = expenses.Count() > 0 ? expenses.Max() : 0;


        var averageBudget = expenses.Count() > 0 ? expenses.Average() : 0;

        //$"\n{Random.Range(0, 4)} companies";
        if (averageBudget != 0)
            text += $"<b>Average budget</b>\n{Format.Money(averageBudget, true)} / week  ({Format.Money(maxBudget, true)} max)";
        else
            text += "\n" + Visuals.Positive("It's a free segment, you can take it easily!");

        return text;
    }

    internal void SetAudience(int ind)
    {
        segmentId = ind;

        ViewRender();
    }
}
