﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompaniesInIndustryListView : ListView
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

        var markets = Markets.GetNichesInIndustry(industry, Q).Select(m => m.niche.NicheType).ToArray();

        // get independent companies, that are interested in this industry

        var companies = Companies.GetIndependentCompanies(Q)
            .Where(Companies.IsNotFinancialStructure)
            .Where(c => Companies.IsInSphereOfInterest(c, markets))
            .OrderByDescending(c => Economy.GetCompanyCost(Q, c));

        SetItems(companies);
    }
}