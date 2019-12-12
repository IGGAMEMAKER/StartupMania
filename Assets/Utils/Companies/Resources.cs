﻿using Assets.Classes;
using UnityEngine;

namespace Assets.Utils
{
    public static partial class Companies
    {
        public static void SpendResources(GameEntity company, long money) => SpendResources(company, new TeamResource(money));
        public static void SpendResources(GameEntity company, TeamResource resource)
        {
            company.companyResource.Resources.Spend(resource);

            company.ReplaceCompanyResource(company.companyResource.Resources);
        }

        public static void SetResources(GameEntity company, TeamResource resource)
        {
            company.ReplaceCompanyResource(resource);
        }

        public static void AddResources(GameEntity company, TeamResource resource)
        {
            company.companyResource.Resources.Add(resource);

            company.ReplaceCompanyResource(company.companyResource.Resources);
        }

        public static void SetStartCapital(GameEntity product, GameContext gameContext) => SetStartCapital(product, Markets.GetNiche(gameContext, product), gameContext);
        public static void SetStartCapital(GameEntity product, GameEntity niche, GameContext gameContext)
        {
            var startCapital = Markets.GetStartCapital(niche, gameContext) * Random.Range(50, 150) / 100;

            AddResources(product, new TeamResource(startCapital));
        }


        public static bool IsEnoughResources(GameEntity company, long money) => IsEnoughResources(company, new TeamResource(money));
        public static bool IsEnoughResources(GameEntity company, TeamResource resource)
        {
            return company.companyResource.Resources.IsEnoughResources(resource);
        }
    }
}
