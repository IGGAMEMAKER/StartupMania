﻿using System.Collections.Generic;

namespace Assets.Core
{
    public static partial class Investments
    {
        public static List<GoalRequirements> GetGoalRequirements(GameEntity company, GameContext gameContext)
        {
            var goal = company.companyGoal;

            switch (goal.InvestorGoal)
            {
                // product company goals
                case InvestorGoalType.Prototype:
                    return Wrap(GoalPrototype(company, gameContext));

                case InvestorGoalType.FirstUsers:
                    return Wrap(GoalFirstUsers(company, gameContext));

                case InvestorGoalType.Release:
                    return Wrap(GoalRelease(company, gameContext));

                case InvestorGoalType.BecomeMarketFit:
                    return Wrap(GoalMarketFit(company, gameContext));

                // company group goals
                case InvestorGoalType.BecomeProfitable:
                    return Wrap(GoalProfitable(company, gameContext));

                case InvestorGoalType.GrowCompanyCost:
                    return Wrap(GoalCompanyCost(company, gameContext));
                
                //case InvestorGoal.GrowProfit: return GoalGrowProfit(company, gameContext);
                case InvestorGoalType.IPO:
                    return Wrap(GoalIPO(company, gameContext));

                default: return Wrap(new GoalRequirements { need = 888888, have = 191919 });
            }
        }

        static List<GoalRequirements> Wrap(GoalRequirements requirements)
        {
            return new List<GoalRequirements> { requirements };
        }

        private static bool IsCanTakeIPOGoal(GameEntity company, GameContext gameContext, InvestorGoalType nextGoal)
        {
            return !company.hasProduct && nextGoal == InvestorGoalType.GrowCompanyCost && Economy.CostOf(company, gameContext) > C.IPO_REQUIREMENTS_COMPANY_COST;
        }

        public static GoalRequirements GoalPrototype(GameEntity company, GameContext gameContext)
        {
            var targetAudience = Marketing.GetCoreAudienceId(company); // company.productPositioning.Positioning; //.SegmentId;
            return new GoalRequirements {
                have = (long)Marketing.GetSegmentLoyalty(company, targetAudience),
                need = 1
            };
        }

        public static GoalRequirements GoalFirstUsers(GameEntity company, GameContext gameContext)
        {
            return new GoalRequirements {
                have = Marketing.GetUsers(company),
                need = 50000, // company.companyGoal.MeasurableGoal
            };
        }

        public static GoalRequirements GoalRelease(GameEntity company, GameContext gameContext)
        {
            return new GoalRequirements {
                have = company.isRelease ? 1 : 0,
                need = 1
            };
        }

        public static GoalRequirements GoalGrowProfit(GameEntity company, GameContext gameContext)
        {
            return new GoalRequirements
            {
                have = Economy.GetProfit(gameContext, company),
                need = company.companyGoal.MeasurableGoal
            };
        }

        private static GoalRequirements GoalCompanyCost(GameEntity company, GameContext gameContext)
        {
            return new GoalRequirements
            {
                have = Economy.CostOf(company, gameContext),
                need = company.companyGoal.MeasurableGoal
            };
        }

        private static GoalRequirements GoalIPO(GameEntity company, GameContext gameContext)
        {
            return new GoalRequirements
            {
                have = company.isPublicCompany ? 1 : 0,
                need = 1
            };
        }

        private static GoalRequirements GoalProfitable(GameEntity company, GameContext gameContext)
        {
            return new GoalRequirements
            {
                have = Economy.GetProfit(gameContext, company),
                need = 0
            };
        }

        private static GoalRequirements GoalMarketFit(GameEntity company, GameContext gameContext)
        {
            var targetAudience = Marketing.GetCoreAudienceId(company); // company.productPositioning.Positioning; //.SegmentId;

            return new GoalRequirements
            {
                have = (long)Marketing.GetSegmentLoyalty(company, targetAudience),
                need = 5,
            };
            //return new GoalRequirements
            //{
            //    have = Products.GetProductLevel(company),
            //    need = Products.GetMarketRequirements(company, gameContext)
            //};
        }
    }
}
