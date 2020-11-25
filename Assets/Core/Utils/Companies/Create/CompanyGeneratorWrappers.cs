﻿using Assets.Core;
using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core
{
    partial class Companies
    {
        // Create
        public static int GenerateCompanyId(GameContext context)
        {
            return context.GetEntities(GameMatcher.Company).Length;
        }

        private static GameEntity CreateCompany(GameContext context, string name, CompanyType companyType)
        {
            var CEO = Humans.GenerateHuman(context);


            var level = UnityEngine.Random.Range(70, 90);

            Humans.SetTrait(CEO, Trait.Ambitious, level);
            Humans.SetSkills(CEO, WorkerRole.CEO);

            return CreateCompany(context, name, companyType, new Dictionary<int, BlockOfShares>(), CEO);
        }

        public static GameEntity GenerateCompanyGroup(GameContext context, string name, int FormerProductCompany)
        {
            var c = GenerateCompanyGroup(context, name);

            CopyShareholders(context, FormerProductCompany, c.company.Id);

            return c;
        }

        public static void CopyShareholders(GameContext gameContext, int from, int to)
        {
            var From = Get(gameContext, from);
            var To = Get(gameContext, to);

            ReplaceShareholders(To, From.shareholders.Shareholders);
        }


        public static GameEntity GenerateCompanyGroup(GameContext context, string name)
        {
            var c = CreateCompany(context, name, CompanyType.Group);
            c.isManagingCompany = true;

            Investments.BecomeInvestor(context, c, 0);

            return c;
        }

        public static GameEntity GenerateInvestmentFund(GameContext context, string name, long money)
        {
            var c = CreateCompany(context, name, CompanyType.FinancialGroup);

            Investments.BecomeInvestor(context, c, money);

            return c;
        }

        public static GameEntity GenerateHoldingCompany(GameContext context, string name)
        {
            var c = GenerateCompanyGroup(context, name);

            return TurnToHolding(context, c.company.Id);
        }

        public static GameEntity GenerateProductCompany(GameContext context, string name, NicheType NicheType)
        {
            var c = CreateCompany(context, name, CompanyType.ProductCompany);

            return CreateProduct(context, c, NicheType);
        }

        public static GameEntity AutoGenerateProductCompany(NicheType NicheType, GameContext gameContext)
        {
            var playersOnMarket = Markets.GetCompetitorsAmount(NicheType, gameContext);

            var c = GenerateProductCompany(gameContext, Enums.GetFormattedNicheName(NicheType) + " " + playersOnMarket, NicheType);

            AutoFillShareholders(gameContext, c, true);
            //SetFounderAmbitionDueToMarketSize(c, gameContext);

            return c;
        }

        // TODO remove
        public static void SetFounderAmbitionDueToMarketSize(GameEntity company, GameContext gameContext)
        {
            var niche = Markets.Get(gameContext, company.product.Niche);
            var rating = Markets.GetMarketPotentialRating(niche);


            var rand = UnityEngine.Random.Range(1f, 2f) * 5;

            // 5...25
            var ambition = 65 + Mathf.Clamp(rating * rand, 0, 30);
            var CeoId = GetCEOId(company);

            var ceo = Humans.Get(gameContext, CeoId);

            Humans.SetTrait(ceo, Trait.Ambitious, (int)ambition);
        }


        public static void AutoFillShareholders(GameContext gameContext, GameEntity c, bool founderOnly)
        {
            var founder = c.cEO.HumanId;
            var shareholder = Humans.Get(gameContext, founder);

            Investments.BecomeInvestor(gameContext, shareholder, 100000);

            AddShares(c, shareholder, 500);

            if (founderOnly)
                return;

            for (var i = 0; i < UnityEngine.Random.Range(1, 5); i++)
            {
                var investor = Investments.GetRandomInvestmentFund(gameContext);

                AddShares(c, investor, 100);
            }
        }

        public static void AutoFillNonFilledShareholders(GameContext gameContext, bool founderOnly)
        {
            var nonFinancialCompaniesWithZeroShareholders = Array.FindAll(gameContext.GetEntities(GameMatcher
                .AllOf(GameMatcher.Company, GameMatcher.Shareholders)),
                e => IsNotFinancialStructure(e) && e.shareholders.Shareholders.Count == 0);

            foreach (var c in nonFinancialCompaniesWithZeroShareholders)
                AutoFillShareholders(gameContext, c, founderOnly);
        }
    }
}
