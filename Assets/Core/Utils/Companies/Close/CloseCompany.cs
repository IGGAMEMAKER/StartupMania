﻿using Assets.Core;
using System.Collections.Generic;

namespace Assets.Core
{
    partial class Companies
    {
        public static GameEntity CloseCompany(GameContext context, int CompanyId) => CloseCompany(context, Get(context, CompanyId));
        public static GameEntity CloseCompany(GameContext context, GameEntity e)
        {
            // pay to everyone
            PayDividends(context, e, e.companyResource.Resources.money);

            // fire everyone

            NotifyAboutProductSupportEnd(e, context);

            if (e.hasProduct)
            {
                Teams.DismissTeam(e, context);
                Markets.ReturnUsersWhenCompanyIsClosed(e, context);
            }

            RemoveAllPartnerships(e, context);

            foreach (var holding in GetDaughterCompanies(context, e))
                DestroyBlockOfShares(context, holding, e.shareholder.Id);

            e.ReplaceShareholders(new Dictionary<int, BlockOfShares>());


            e.isAlive = false;

            return e;
        }



        public static void NotifyAboutProductSupportEnd(GameEntity company, GameContext gameContext)
        {
            if (Companies.IsInPlayerSphereOfInterest(company, gameContext))
                NotificationUtils.AddNotification(gameContext, new NotificationMessageBankruptcy(company.company.Id));

            NotificationUtils.SendBankruptcyPopup(gameContext, company);
        }
    }
}
