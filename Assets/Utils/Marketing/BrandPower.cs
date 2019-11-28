﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Utils
{
    public static partial class MarketingUtils
    {
        public static void AddBrandPower(GameEntity company, float power)
        {
            var brandPower = Mathf.Clamp(company.branding.BrandPower + power, 0, 100);

            company.ReplaceBranding(brandPower);
        }

        public static Bonus GetMonthlyBrandPowerChange(GameEntity product, GameContext gameContext)
        {
            var conceptStatus = ProductUtils.GetConceptStatus(product, gameContext);

            var isOutOfMarket = conceptStatus == ConceptStatus.Outdated;
            var isInnovator = conceptStatus == ConceptStatus.Leader;

            var percent = 4;
            var baseDecay = -product.branding.BrandPower * percent / 100;


            var isMarketingAggressively = product.financing.Financing[Financing.Marketing] == 3;
            var isMonopolist = NicheUtils.GetCompetitorsAmount(product, gameContext) == 1;

            var partnershipBonuses = GetPartnershipBonuses(product, gameContext);

            var BrandingChangeBonus = new Bonus("Brand power change")
                .AppendAndHideIfZero("Monopolist", isMonopolist ? 2 : 0)
                .AppendAndHideIfZero(percent + "% Decay", (int)baseDecay)
                .Append("Partnerships", (int)partnershipBonuses)
                .AppendAndHideIfZero("Aggressive marketing", isMarketingAggressively ? 2 : 0)
                .AppendAndHideIfZero("Outdated app", isOutOfMarket ? -1 : 0)
                .AppendAndHideIfZero("Is Innovator", isInnovator ? 5 : 0)
                ;

            return BrandingChangeBonus;
        }

        public static List<int> GetPartnersOf(GameEntity company, GameContext gameContext)
        {
            //return NicheUtils.GetProductsOnMarket(gameContext, product);

            if (company.isIndependentCompany)
                return company.partnerships.Companies;

            var parent = CompanyUtils.GetParentCompany(gameContext, company);

            return parent.partnerships.Companies;
        }

        public static List<GameEntity> GetPartnerList(GameEntity company, GameContext gameContext)
        {
            var partners = GetPartnersOf(company, gameContext);

            return partners
                .Select(p => CompanyUtils.GetCompanyById(gameContext, p))
                .Where(p => p.hasProduct)
                .ToList();
        }

        private static float GetPartnershipBonuses(GameEntity product, GameContext gameContext)
        {
            var partners = GetPartnerList(product, gameContext);

            var partnersInSameIndustry = partners
                .Where(p =>
                NicheUtils.GetIndustry(p.product.Niche, gameContext) ==
                NicheUtils.GetIndustry(product.product.Niche, gameContext)
                );


            float value = 0;
            foreach (var p in partnersInSameIndustry)
            {
                var marketShare = CompanyUtils.GetMarketShareOfCompanyMultipliedByHundred(product, gameContext);
                var marketSize = NicheUtils.GetMarketRating(gameContext, product.product.Niche);
                
                value += marketShare * marketSize / 100f;
            }

            return Mathf.Clamp(value, 0, 1.5f);
        }
    }
}
