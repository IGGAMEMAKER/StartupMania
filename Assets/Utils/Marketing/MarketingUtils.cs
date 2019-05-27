﻿using Assets.Utils.Formatting;
using System.Text;

namespace Assets.Utils
{
    public static partial class MarketingUtils
    {
        internal static void SetFinancing(GameContext gameContext, int companyId, MarketingFinancing marketingFinancing)
        {
            var c = CompanyUtils.GetCompanyById(gameContext, companyId);

            var f = c.finance;

            c.ReplaceFinance(f.price, marketingFinancing, f.salaries, f.basePrice);
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

        public static string GetAudienceHint(GameContext gameContext, UserType userType, GameEntity company)
        {
            StringBuilder hint = new StringBuilder();

            var id = company.company.Id;

            var churn = GetChurnRate(gameContext, id, userType);
            var churnClients = GetChurnClients(gameContext, id, userType);
            var promoted = GetPromotionClients(gameContext, id, userType);

            hint.AppendFormat("Due to our churn rate ({0}%)", churn);
            hint.AppendFormat(" we lose {0} clients each month\n", Visuals.Negative(churnClients.ToString()));

            if (userType != UserType.Core)
            {
                UserType next = userType == UserType.Newbie ? UserType.Regular : UserType.Core;

                hint.AppendFormat("<color={0}>Also, {2} clients will be promoted to {1}</color>",
                    VisualConstants.COLOR_POSITIVE,
                    EnumUtils.GetFormattedUserType(next),
                    promoted
                    );
            }

            hint.AppendLine();

            return hint.ToString();
        }
    }
}
