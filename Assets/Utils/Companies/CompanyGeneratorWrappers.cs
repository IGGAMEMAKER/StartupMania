﻿using Assets.Classes;
using Assets.Utils.Humans;
using Entitas;
using System.Collections.Generic;

namespace Assets.Utils
{
    partial class CompanyUtils
    {
        // Create
        public static int GenerateCompanyId(GameContext context)
        {
            return context.GetEntities(GameMatcher.Company).Length;
        }

        public static int GenerateInvestorId(GameContext context)
        {
            return context.GetEntities(GameMatcher.Shareholder).Length;
        }

        private static GameEntity CreateCompany(GameContext context, string name, CompanyType companyType)
        {
            int humanId = HumanUtils.GenerateHuman(context);

            return CreateCompany(context, name, companyType, new Dictionary<int, BlockOfShares>(), humanId);
        }

        public static GameEntity GenerateCompanyGroup(GameContext context, string name, int FormerProductCompany)
        {
            var c = GenerateCompanyGroup(context, name);

            CopyShareholders(context, FormerProductCompany, c.company.Id);

            return c;
        }

        public static GameEntity GenerateCompanyGroup(GameContext context, string name)
        {
            var c = CreateCompany(context, name, CompanyType.Group);

            BecomeInvestor(context, c, 0);

            return c;
        }

        public static GameEntity GenerateInvestmentFund(GameContext context, string name, long money)
        {
            var c = CreateCompany(context, name, CompanyType.FinancialGroup);

            BecomeInvestor(context, c, money);

            return c;
        }

        public static GameEntity GenerateHoldingCompany(GameContext context, string name)
        {
            var c = GenerateCompanyGroup(context, name);

            return TurnToHolding(context, c.company.Id);
        }

        public static GameEntity GenerateProductCompany(GameContext context, string name, NicheType niche)
        {
            var c = CreateCompany(context, name, CompanyType.ProductCompany);

            return GenerateProduct(context, c, name, niche);
        }
    }
}
