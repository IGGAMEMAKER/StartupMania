﻿using System.Linq;
using UnityEngine;

namespace Assets.Core
{
    public static partial class Products
    {
        public static void UpdgradeProduct(GameEntity product, GameContext gameContext, bool IgnoreCooldowns = false)
        {
            if (Cooldowns.HasConceptUpgradeCooldown(gameContext, product) && !IgnoreCooldowns)
                return;

            UpgradeProductLevel(product, gameContext);
            UpdateMarketRequirements(product, gameContext);

            if (!IgnoreCooldowns)
                Cooldowns.AddConceptUpgradeCooldown(gameContext, product);
        }

        private static void UpgradeProductLevel(GameEntity product, GameContext gameContext)
        {
            if (Random.Range(0, 100) < GetInnovationChance(product, gameContext))
                product.ReplaceProduct(product.product.Niche, GetProductLevel(product) + 1);
        }

        private static long GiveInnovatorBenefits(GameEntity product, GameContext gameContext)
        {
            MarketingUtils.AddBrandPower(product, Constants.INNOVATION_BRAND_POWER_GAIN);

            // get your competitor's clients
            var products = Markets.GetProductsOnMarket(gameContext, product)
                .Where(p => p.isRelease)
                .Where(p => p.company.Id != product.company.Id);

            long sum = 0;
            foreach (var p in products)
            {
                var disloyal = MarketingUtils.GetClients(p) / 6;

                MarketingUtils.LooseClients(p, disloyal);
                MarketingUtils.AddClients(product, disloyal);

                sum += disloyal;
            }

            return sum;
        }

        private static void UpdateMarketRequirements(GameEntity product, GameContext gameContext)
        {
            var niche = Markets.GetNiche(gameContext, product.product.Niche);

            var demand = GetMarketDemand(niche);
            var newLevel = GetProductLevel(product);

            if (newLevel > demand)
            {
                // innovation
                var clientChange = GiveInnovatorBenefits(product, gameContext);
                NotifyAboutInnovation(product, gameContext, clientChange);

                niche.ReplaceSegment(newLevel);

                // order matters
                RemoveTechLeaders(product, gameContext);
                product.isTechnologyLeader = true;
            }
            else if (newLevel == demand)
            {
                // if you are techonology leader and you fail to innovate, you will not lose tech leadership
                if (product.isTechnologyLeader)
                    return;

                RemoveTechLeaders(product, gameContext);
            }
        }





        public static void NotifyAboutInnovation(GameEntity product, GameContext gameContext, long clients)
        {
            if (Companies.IsInPlayerSphereOfInterest(product, gameContext) && Markets.GetCompetitorsAmount(product, gameContext) > 1)
                NotificationUtils.AddPopup(gameContext, new PopupMessageInnovation(product.company.Id, clients));
        }

        private static void RemoveTechLeaders(GameEntity product, GameContext gameContext)
        {
            var players = Markets.GetProductsOnMarket(gameContext, product).ToArray();

            foreach (var p in players)
                p.isTechnologyLeader = false;
        }




        public static bool HasFreeImprovements(GameEntity product)
        {
            var level = GetProductLevel(product);

            return product.productImprovements.Count < level * 2;
        }

        // TODO move to separate file/delete
        public static void UpgradeFeatures(ProductImprovement improvement, GameEntity product)
        {
            if (HasFreeImprovements(product))
            {
                product.productImprovements.Improvements[improvement]++;
                product.productImprovements.Count++;
            }
        }
    }
}
