﻿namespace Assets.Utils
{
    public static partial class Markets
    {
        public static long GetStartCapital(NicheType nicheType, GameContext gameContext) => GetStartCapital(GetNiche(gameContext, nicheType), gameContext);
        public static long GetStartCapital(GameEntity niche, GameContext gameContext)
        {
            var timeToMarket = Products.GetTimeToMarketFromScratch(niche);

            var timeToProfitability = 4;

            var marketingBudget = 200000;

            return (timeToMarket + timeToProfitability) * GetBaseProductMaintenance(gameContext, niche) + marketingBudget;
        }

        internal static long GetBaseProductMaintenance(GameContext gameContext, GameEntity niche)
        {
            return GetBiggestMaintenanceOnMarket(gameContext, niche);
        }
    }
}
