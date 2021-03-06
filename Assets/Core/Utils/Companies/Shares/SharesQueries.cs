﻿using System.Collections.Generic;

namespace Assets.Core
{
    partial class Companies
    {
        public static int GetTotalShares(GameEntity company)
        {
            var shareholders = company.shareholders.Shareholders;

            int totalShares = 0;

            foreach (var e in shareholders)
                totalShares += e.Value.amount;

            return totalShares;
        }

        public static bool IsSharesCanBeSold(GameEntity company)
        {
            return company.isPublicCompany || company.hasAcceptsInvestments;
        }

        public static bool IsAreSharesSellable(GameContext context, int companyId)
        {
            return IsSharesCanBeSold(Get(context, companyId));
        }

        public static int[] GetShareholdersCopy(GameEntity company)
        {
            var shareholders = GetShareholders(company);
            int[] array = new int[company.shareholders.Shareholders.Keys.Count];
            shareholders.Keys.CopyTo(array, 0);

            return array;
        }

        public static Dictionary<int, BlockOfShares> GetShareholders(GameEntity company)
        {
            var shareholders = new Dictionary<int, BlockOfShares>();

            if (company.hasShareholders)
                shareholders = company.shareholders.Shareholders;

            return shareholders;
        }

        public static bool IsInvestsInCompany(GameEntity company, GameEntity investor) => Investments.IsInvestsInCompany(investor, company);

        public static int GetAmountOfShares(GameContext context, GameEntity c, GameEntity investor)
        {
            var shareholders = c.shareholders.Shareholders;

            return IsInvestsInCompany(c, investor) ? shareholders[investor.shareholder.Id].amount : 0;
        }


        //public static int GetShareSize(GameContext context, int companyId, int investorId) 
        public static int GetShareSize(GameContext context, GameEntity c, GameEntity investor)
        {
            int shares = GetAmountOfShares(context, c, investor);
            int total = GetTotalShares(c);

            if (total == 0)
                return 0;

            return shares * 100 / total;
        }
        public static float GetExactShareSize(GameContext context, GameEntity c, GameEntity investor)
        {
            int shares = GetAmountOfShares(context, c, investor);
            int total = GetTotalShares(c);

            if (total == 0)
                return 0;

            return shares * 100f / total;
        }

        public static long GetSharesCost(GameContext context, GameEntity c, GameEntity investor, int shares = -1)
        {
            if (shares == -1)
                shares = GetAmountOfShares(context, c, investor);

            int total = GetTotalShares(c);

            return Economy.CostOf(c, context) * shares / total;
        }

        public static string GetInvestorName(GameContext context, int investorId) => GetInvestorName(GetInvestorById(context, investorId));
        public static string GetInvestorName(GameEntity investor)
        {
            return investor.shareholder.Name;
        }

        public static string GetShareholderStatus(int sharesPercent)
        {
            if (sharesPercent < 1)
                return "Non voting";

            if (sharesPercent < 10)
                return "Voting";

            if (sharesPercent < 25)
                return "Majority";

            if (sharesPercent < 50)
                return "Blocking";

            if (sharesPercent < 100)
                return "Controling";

            return "Owner";
        }
    }
}
