﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TOPCompaniesListView : ListView
{
    public override void SetItem<T>(Transform t, T entity, object data = null)
    {
        var company = entity as GameEntity;

        var cost = Economy.GetCompanyCost(Q, company);
        //t.GetComponent<MockText>().SetEntity($"{company.company.Name} ({Format.Money(cost)})");
        t.GetComponent<CompanyInIndustryView>().SetEntity(company.company.Id);
    }

    public override void ViewRender()
    {
        base.ViewRender();

        var industry = MyCompany.companyFocus.Industries[0];

        // get all independent nonfund companies
        var companies = Companies.GetIndependentCompanies(Q)
            .Where(Companies.IsNotFinancialStructure)
            .OrderByDescending(c => Economy.GetCompanyCost(Q, c));

        SetItems(companies);
    }
}