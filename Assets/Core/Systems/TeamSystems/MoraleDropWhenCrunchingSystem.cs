﻿using Assets.Core;
using Entitas;
using System;
using System.Collections.Generic;
using UnityEngine;

class MoraleManagementSystem : OnPeriodChange
{
    public MoraleManagementSystem(Contexts contexts) : base(contexts) {}

    protected override void Execute(List<GameEntity> entities)
    {
        var companies = contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.Alive, GameMatcher.Company));

        // pyramid
        //
        // salary
        // interesting tasks
        // career ladder
        // feedback (i am doing useful stuff)
        // influence (become company shareholder)

        var playerFlagshipId = Companies.GetPlayerFlagshipID(gameContext);

        foreach (var c in companies)
        {
            var culture = Companies.GetActualCorporateCulture(c, gameContext);

            List<int> defectedManagers = new List<int>();

            // gain expertise and recalculate loyalty
            foreach (var m in c.team.Managers)
            {
                var humanId = m.Key;

                var human = Humans.GetHuman(gameContext, humanId);

                var relationship = human.humanCompanyRelationship;

                var loyaltyChange   = Teams.GetLoyaltyChangeForManager(human, culture);

                var newLoyalty      = Mathf.Clamp(relationship.Morale  + loyaltyChange, 0, 100);
                var newAdaptation   = Mathf.Clamp(relationship.Adapted + 5, 0, 100);


                // TODO: if is CEO in own project, morale loss is way lower or zero
                bool isOwner = human.hasCEO;
                if (isOwner)
                    newLoyalty = 100;


                human.ReplaceHumanCompanyRelationship(newAdaptation, newLoyalty);

                // gain expertise
                if (c.hasProduct)
                {
                    var niche = c.product.Niche;

                    var newExpertise = 1;
                    if (human.humanSkills.Expertise.ContainsKey(niche))
                        newExpertise = Mathf.Clamp(human.humanSkills.Expertise[niche] + 1, 0, 100);

                    human.humanSkills.Expertise[niche] = newExpertise;
                }


                // leave company on low morale
                if (newLoyalty <= 0)
                    defectedManagers.Add(humanId);
            }

            // fire managers
            foreach (var humanId in defectedManagers)
            {
                var human = Humans.GetHuman(gameContext, humanId);

                bool isInPlayerFlagship = c.company.Id == playerFlagshipId;
                if (isInPlayerFlagship)
                {
                    NotificationUtils.AddPopup(gameContext, new PopupMessageWorkerLeavesYourCompany(c.company.Id, humanId));
                }
                else
                {
                    Teams.FireManager(c, gameContext, humanId);

                    // competitors need to have chances to hire this worker

                    bool worksInPlayerCompetitorCompany = Companies.IsInPlayerSphereOfInterest(c, gameContext);

                    bool wantsToWorkInYourCompany = UnityEngine.Random.Range(0, 100) < 50;

                    // // NotifyPlayer
                    if (worksInPlayerCompetitorCompany && wantsToWorkInYourCompany)
                        NotificationUtils.AddPopup(gameContext, new PopupMessageWorkerWantsToWorkInYourCompany(c.company.Id, humanId));

                    // or this worker will start his own bussiness in same/adjacent sphere

                    // or will be destroyed
                }
            }
        }
    }
}