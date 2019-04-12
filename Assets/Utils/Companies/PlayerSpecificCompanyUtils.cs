﻿using Entitas;
using System;

namespace Assets.Utils
{
    partial class CompanyUtils
    {
        public static GameEntity GetAnyOfControlledCompanies(GameContext context)
        {
            return GetPlayerControlledProductCompany(context);
        }

        public static GameEntity[] GetProductsNotControlledByPlayer(GameContext context)
        {
            return context.GetEntities(GameMatcher.AllOf(GameMatcher.Product).NoneOf(GameMatcher.ControlledByPlayer));
        }

        public static GameEntity GetPlayerControlledProductCompany(GameContext context)
        {
            // TODO check all use cases! 
            // Most of them simply use MyControlledProductCompany, when we may need GetMyGroupCompany instead!

            var companies =
                context.GetEntities(GameMatcher.AllOf(GameMatcher.Product, GameMatcher.ControlledByPlayer));

            if (companies.Length == 1) return companies[0];

            return null;
        }

        public static GameEntity GetPlayerControlledGroupCompany(GameContext context)
        {
            var companies =
                context.GetEntities(GameMatcher.AllOf(GameMatcher.ControlledByPlayer).NoneOf(GameMatcher.Product));

            if (companies.Length == 1) return companies[0];

            return null;
        }

        public static GameEntity[] GetMyNeighbours(GameContext context)
        {
            GameEntity[] products = GetProductsNotControlledByPlayer(context);

            var myProduct = GetPlayerControlledProductCompany(context).product;

            return Array.FindAll(products, e => e.product.Niche != myProduct.Niche && e.product.Industry == myProduct.Industry);
        }

        public static GameEntity[] GetMyCompetitors(GameContext context)
        {
            //return NicheUtils.GetPlayersOnMarket(GetPlayerControlledProductCompany(context), context);
            GameEntity[] products = GetProductsNotControlledByPlayer(context);

            GameEntity myProductEntity = GetPlayerControlledProductCompany(context);

            return Array.FindAll(products, e => e.product.Niche == myProductEntity.product.Niche);
        }
    }
}
