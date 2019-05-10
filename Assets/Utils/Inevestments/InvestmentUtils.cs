﻿using Assets.Utils.Humans;
using Entitas;
using System;

namespace Assets.Utils
{
    public static class InvestmentUtils
    {
        public static int GenerateInvestorId(GameContext context)
        {
            return context.GetEntities(GameMatcher.Shareholder).Length;
        }

        public static GameEntity GenerateAngel(GameContext gameContext)
        {
            var human = HumanUtils.GenerateHuman(gameContext);

            var investorId = GenerateInvestorId(gameContext);

            BecomeInvestor(gameContext, human, 1000000);

            TurnToAngel(gameContext, investorId);

            return human;
        }

        public static int BecomeInvestor(GameContext context, GameEntity e, long money)
        {
            int investorId = GenerateInvestorId(context);

            string name = "Investor?";

            InvestorType investorType = InvestorType.VentureInvestor;

            // company
            if (e.hasCompany)
            {
                name = e.company.Name;

                if (e.company.CompanyType == CompanyType.FinancialGroup)
                    investorType = InvestorType.VentureInvestor;
                else
                    investorType = InvestorType.Strategic;
            }
            else if (e.hasHuman)
            {
                // or human
                // TODO turn human to investor

                name = e.human.Name + " " + e.human.Surname;
                investorType = InvestorType.Founder;

                if (!e.hasCompanyResource)
                    e.AddCompanyResource(new Classes.TeamResource(money));
            }

            e.AddShareholder(investorId, name, investorType);
            AddMoneyToInvestor(context, investorId, money);

            return investorId;
        }

        public static void TurnToAngel(GameContext gameContext, int investorId)
        {
            var investor = GetInvestorById(gameContext, investorId);

            investor.ReplaceShareholder(investor.shareholder.Id, investor.shareholder.Name, InvestorType.Angel);
        }

        public static void AddMoneyToInvestor(GameContext context, int investorId, long sum)
        {
            var investor = GetInvestorById(context, investorId);

            var companyResource = investor.companyResource;
            companyResource.Resources.AddMoney(sum);

            investor.ReplaceCompanyResource(companyResource.Resources);
        }

        public static GameEntity GetInvestorById(GameContext context, int investorId)
        {
            return Array.Find(context.GetEntities(GameMatcher.Shareholder), s => s.shareholder.Id == investorId);
        }

        public static int GetCompanyIdByInvestorId(GameContext context, int investorId)
        {
            return GetInvestorById(context, investorId).company.Id;
        }

        public static string GetFormattedInvestorType(InvestorType investorType)
        {
            switch (investorType)
            {
                case InvestorType.Angel: return "Angel";
                case InvestorType.FFF: return "Family friends fools";
                case InvestorType.Founder: return "Founder";
                case InvestorType.StockExchange: return "Stock Exchange";
                case InvestorType.Strategic: return "Strategic";
                case InvestorType.VentureInvestor: return "Venture investor";
            }

            return investorType.ToString();
        }

        public static string GetInvestorGoalDescription(BlockOfShares shares)
        {
            return GetInvestorGoal(shares.InvestorGoal);

            switch (shares.InvestorGoal)
            {
                case InvestorGoal.BecomeBestByTech:
                    return "Become technology leader";

                case InvestorGoal.BecomeMarketFit:
                    return "Become market fit";

                case InvestorGoal.BecomeProfitable:
                    return "Become profitable";

                case InvestorGoal.GrowClientBase:
                    return "Grow client base";

                case InvestorGoal.GrowCompanyCost:
                    return "Grow company cost";

                case InvestorGoal.GrowProfit:
                    return "Grow profit";

                case InvestorGoal.ProceedToNextRound:
                    return "Proceed to next investment round";

                default:
                    return shares.InvestorGoal.ToString();
            }
        }

        public static string GetInvestorGoal(InvestorGoal investorGoal)
        {
            switch (investorGoal)
            {
                case InvestorGoal.BecomeBestByTech:
                    return "Become technology leader";

                case InvestorGoal.BecomeMarketFit:
                    return "Become market fit";

                case InvestorGoal.BecomeProfitable:
                    return "Become profitable";

                case InvestorGoal.GrowClientBase:
                    return "Grow client base";

                case InvestorGoal.GrowCompanyCost:
                    return "Grow company cost";

                case InvestorGoal.GrowProfit:
                    return "Grow profit";

                case InvestorGoal.ProceedToNextRound:
                    return "Proceed to next investment round";

                default:
                    return investorGoal.ToString();
            }
        }

        public static string GetInvestorGoal(BlockOfShares shares)
        {
            return GetInvestorGoal(shares.InvestorGoal);
        }
    }
}
