﻿using Assets.Core;
using UnityEngine.UI;

public class BasicProjectInfoView : View
{
    public Text CEONameLabel;
    public Text PublicityStatus;

    void RenderCEO()
    {
        var human = Humans.Get(Q, SelectedCompany.cEO.HumanId).human;
        string name = SelectedCompany.isControlledByPlayer ? "YOU" : $"{human.Name} {human.Surname}";

        CEONameLabel.text = Visuals.Link($"CEO: {name}");

        CEONameLabel.gameObject.GetComponent<LinkToHuman>().SetHumanId(human.Id);
    }


    void RenderCompanyStatus()
    {
        //PublicityStatus.text = SelectedCompany.isPublicCompany ? "Is public company" : "Is private company";
    }

    void Render()
    {
        RenderCompanyStatus();

        RenderCEO();
    }
}
