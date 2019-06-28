﻿using System.Collections.Generic;
using Assets.Utils;
using Assets.Utils.Formatting;
using UnityEngine;
using UnityEngine.UI;

public class CompanyPreviewView : View,
    IProductListener
{
    public Text CompanyNameLabel;
    public Text CompanyTypeLabel;
    public Image Panel;
    public Text CEOLabel;

    public Text ShareCostLabel;

    public GameEntity _entity;

    public void SetEntity(GameEntity entity)
    {
        _entity = entity;

        entity.AddProductListener(this);

        Render(entity);
    }

    void RenderPanel()
    {
        var inGroupScreens = CurrentScreen == ScreenMode.GroupManagementScreen || CurrentScreen == ScreenMode.ManageCompaniesScreen;

        Panel.color = GetPanelColor(_entity == SelectedCompany && inGroupScreens);
    }

    void Render(GameEntity e)
    {
        RenderPanel();

        CEOLabel.gameObject.SetActive(_entity.isControlledByPlayer);

        RenderCompanyName(e);
        RenderCompanyType(e);

        RenderCompanyCost(e);

        UpdateLinkToCompany(e);
    }

    void RenderCompanyType(GameEntity entity)
    {
        CompanyTypeLabel.text = EnumUtils.GetFormattedCompanyType(entity.company.CompanyType);
    }

    void RenderCompanyName(GameEntity entity)
    {
        CompanyNameLabel.text = entity.company.Name;
    }

    void UpdateLinkToCompany(GameEntity e)
    {
        var link = GetComponent<LinkToProjectView>();

        if (link != null)
            link.CompanyId = e.company.Id;
    }


    private void RenderCompanyCost(GameEntity e)
    {
        var cost = CompanyEconomyUtils.GetCompanyCost(GameContext, e.company.Id);

        if (ShareCostLabel)
            ShareCostLabel.text = Format.Money(cost);
    }

    void IProductListener.OnProduct(GameEntity entity, int id, NicheType niche, Dictionary<UserType, int> segments)
    {
        Render(entity);
    }
}