﻿using Assets.Utils;
using Assets.Utils.Formatting;
using UnityEngine.UI;

public class OurCompanyGlobalStrength : View
{
    void OnEnable()
    {
        var industries = MyCompany.companyFocus.Industries;

        var text = "";

        foreach (var ind in industries)
        {
            var strength = Companies.GetCompanyStrengthInIndustry(MyCompany, ind, GameContext);
            text += EnumUtils.GetFormattedIndustryName(ind) + "\n" + strength + " industry\n\n";
        }

        GetComponent<Text>().text = text;
    }
}
