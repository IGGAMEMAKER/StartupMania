﻿using System.Linq;

namespace Assets.Core
{
    public static partial class Marketing
    {
        public static long GetChannelCost(GameEntity product, int channelId)
        {
            return (long)product.channelInfos.ChannelInfos[channelId].costPerAd;
            // return (long)channel.marketingChannel.ChannelInfo.costPerAd;
        }

        public static float GetChannelCostPerUser(GameEntity product, int channelId)
        {
            return GetChannelCost(product, channelId) * 1f / GetChannelClientGain(product, channelId);
        }

        public static bool IsActiveInChannel(GameEntity product, int channelId)
        {
            return product.companyMarketingActivities.Channels.ContainsKey(channelId);
            
            // // GameEntity channel
            // return channel.channelMarketingActivities.Companies.ContainsKey(product.company.Id);
        }

        public static long GetGrowthLoyaltyBonus(GameEntity company, int segmentId)
        {
            var loyalty = (int)GetSegmentLoyalty(company, segmentId);

            var loyaltyBonus = 0;
            if (loyalty >= 0)
            {
                loyaltyBonus = loyalty * 10;

                if (loyalty > 10)
                    loyaltyBonus = 10 * 10 + (loyalty - 10) * 5;

                if (loyalty > 20)
                    loyaltyBonus = 10 * 10 + 10 * 5 + (loyalty - 20) * 1;
            }

            return loyaltyBonus;
        }

        public static bool IsWillSufferOnAudienceLoss(GameEntity company, int segmentId)
        {
            return IsAimingForSpecificAudience(company, segmentId) && GetUsers(company, segmentId) > 0;
        }

        public static bool IsAimingForSpecificAudience(GameEntity company, int segmentId)
        {
            var positioning = GetPositioning(company);
            
            return positioning.Loyalties[segmentId]>= 0;
        }

        public static int GetAmountOfTargetAudiences(GameEntity company) => GetAmountOfTargetAudiences(GetPositioning(company));
        public static int GetAmountOfTargetAudiences(ProductPositioning positioning)
        {
            return positioning.Loyalties.Count(l => l > 0);
        }

        public static bool IsHasDisloyalAudiences(GameEntity company)
        {
            return GetAudienceInfos().Any(a => IsAudienceDisloyal(company, a.ID) && IsTargetAudience(company, a.ID));
        }

        public static bool IsAudienceDisloyal(GameEntity company, int segmentId)
        {
            // cannot get clients if existing ones are outraged

            return GetGrowthLoyaltyBonus(company, segmentId) < 0;
        }

        public static bool IsAttractsSpecificAudience(GameEntity company, int segmentId)
        {
            return IsAimingForSpecificAudience(company, segmentId) && !IsAudienceDisloyal(company, segmentId);
        }

        public static double GetSegmentFocusingMultiplier(GameEntity product)
        {
            var favouriteAudiences = GetAmountOfTargetAudiences(product);

            switch (favouriteAudiences)
            {
                case 1: return 1;
                case 2: return 0.7d;
                case 3: return 0.5d;

                default: return 0.1d;
            }

            // return  Companies.GetHashedRandom2(product.company.Id, channel.marketingChannel.ChannelInfo.ID + segmentId);
        }

        // fraction will be recalculated
        // take into account
        // * Base channel width (f.e. 100K users per week)

        // * proportions (teens: 90%, olds: 10%)
        // * random anomalies (there are more people of specific segment (especially in small channels)) teens: 80%, olds: 20%)
        // * Base user activity (desire to click on ads: 5% => we can get 5K users)
        // * segment bonuses (audience may be small, but it is way more active (desire to click X2) and you can get more)
        // * positioning bonuses
        public static long GetChannelClientGain(GameEntity company, int channelId) =>
            GetAudienceInfos().Select(i => GetChannelClientGain(company, company.channelInfos.ChannelInfos[channelId], i.ID)).Sum();

        public static long GetChannelClientGain(GameEntity company, int channelId, int segmentId) =>
            GetChannelClientGain(company, company.channelInfos.ChannelInfos[channelId], segmentId);
        
        public static long GetChannelClientGain(GameEntity company, ChannelInfo channelInfo, int segmentId)
        {
            if (!IsAttractsSpecificAudience(company, segmentId))
                return 0;

            var baseChannelBatch = channelInfo.Batch;
            var fraction = GetSegmentFocusingMultiplier(company);

            var batch = (long)(baseChannelBatch * fraction);

            var marketingEfficiency = Teams.GetMarketingEfficiency(company);
            var loyaltyBonus = GetGrowthLoyaltyBonus(company, segmentId);

            return batch * (marketingEfficiency + loyaltyBonus) / 100;
        }

        // in months
        public static void EnableChannelActivity(GameEntity product, GameEntity channel)
        {
            var companyId = product.company.Id;
            var channelId = channel.marketingChannel.ChannelInfo.ID;

            product.companyMarketingActivities.Channels[channelId] = 1;
            channel.channelMarketingActivities.Companies[companyId] = 1;
        }

        public static void DisableChannelActivity(GameEntity product, GameEntity channel)
        {
            var companyId = product.company.Id;
            var channelId = channel.marketingChannel.ChannelInfo.ID;

            product.companyMarketingActivities.Channels.Remove(channelId);
            channel.channelMarketingActivities.Companies.Remove(companyId);
        }
    }
}
