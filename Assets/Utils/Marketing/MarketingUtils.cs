﻿using System;
using System.Text;

namespace Assets.Utils
{
    public static partial class MarketingUtils
    {
        public static int GetClientLoyaltyBugPenalty(GameContext gameContext, int companyId)
        {
            int bugs = 15;

            return bugs;
        }

        public static int GetClientLoyalty(GameContext gameContext, int companyId, UserType userType)
        {
            var c = CompanyUtils.GetCompanyById(gameContext, companyId);

            int bugs = GetClientLoyaltyBugPenalty(gameContext, companyId);
            int app = GetAppLoyaltyBonus(gameContext, companyId);
            int pricing = GetPricingLoyaltyPenalty(gameContext, companyId);
            int marketRequirement = GetMarketSituationLoyaltyBonus(gameContext, companyId);

            return app - bugs - pricing - marketRequirement;
        }

        internal static void SetFinancing(GameContext gameContext, int companyId, MarketingFinancing marketingFinancing)
        {
            var c = CompanyUtils.GetCompanyById(gameContext, companyId);

            var f = c.finance;

            c.ReplaceFinance(f.price, marketingFinancing, f.salaries, f.basePrice);
        }

        public static int GetAppLoyaltyBonus(GameContext gameContext, int companyId)
        {
            var c = CompanyUtils.GetCompanyById(gameContext, companyId);

            return c.product.ProductLevel * 4;
        }

        public static int GetMarketSituationLoyaltyBonus(GameContext gameContext, int companyId)
        {
            return 10 * 4;
        }

        public static int GetChurnRateLoyaltyPart(GameContext gameContext, int companyId, UserType userType)
        {
            var loyalty = GetClientLoyalty(gameContext, companyId, userType);

            if (loyalty < -50) loyalty = -50;
            if (loyalty > 50) loyalty = 50;

            return (50 - loyalty) / 10;
        }

        public static int GetChurnRate(GameContext gameContext, int companyId, UserType userType)
        {
            int baseValue = GetUserTypeBaseValue(userType);
            int fromLoyalty = GetChurnRateLoyaltyPart(gameContext, companyId, userType);

            return baseValue + fromLoyalty;
        }

        internal static int GetUserTypeBaseValue(UserType userType)
        {
            int multiplier = 1;

            if (userType == UserType.Regular)
                multiplier = 4;
            if (userType == UserType.Newbie)
                multiplier = 9;

            return multiplier;
        }

        public static int GetPricingLoyaltyPenalty(GameContext gameContext, int companyId)
        {
            var c = CompanyUtils.GetCompanyById(gameContext, companyId);

            var pricing = c.finance.price;

            switch (pricing)
            {
                case Pricing.Free: return 0;
                case Pricing.Low: return 5;
                case Pricing.Medium: return 22;
                case Pricing.High: return 30;

                default: return 1000;
            }
        }

        public static string GetClientLoyaltyDescription(GameContext gameContext, int companyId, UserType userType)
        {
            BonusContainer bonusContainer = new BonusContainer("Client loyalty is", GetClientLoyalty(gameContext, companyId, userType));

            bonusContainer.Append("App level", GetAppLoyaltyBonus(gameContext, companyId));
            bonusContainer.Append("Market demand", GetMarketSituationLoyaltyBonus(gameContext, companyId));
            bonusContainer.Append("Bugs", GetClientLoyaltyBugPenalty(gameContext, companyId));
            bonusContainer.Append("Pricing", GetPricingLoyaltyPenalty(gameContext, companyId));

            return bonusContainer.ToString();
        }

        public static int GetMarketDiff(GameContext gameContext, int companyId)
        {
            var best = NicheUtils.GetLeaderApp(gameContext, companyId);

            var c = CompanyUtils.GetCompanyById(gameContext, companyId);

            return best.product.ProductLevel - c.product.ProductLevel;
        }

        public static long GetClients(GameEntity company)
        {
            long amount = 0;

            foreach (var p in company.marketing.Segments)
                amount += p.Value;

            return amount;
        }
    }
}
