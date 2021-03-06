﻿using Assets.Core;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class MoraleManagementSystem : OnPeriodChange
{
    public MoraleManagementSystem(Contexts contexts) : base(contexts) {}

    protected override void Execute(List<GameEntity> entities)
    {
        //var companies = contexts.game.GetEntities(GameMatcher.AllOf(GameMatcher.Alive, GameMatcher.Company));
        var companies = Companies.Get(gameContext);

        // maslov pyramid
        //
        // salary
        // interesting tasks
        // career ladder
        // feedback (i am doing useful stuff)
        // influence (become company shareholder)

        var date = ScheduleUtils.GetCurrentDate(gameContext);

        var playerFlagshipId = Companies.GetPlayerFlagshipID(gameContext);

        var humans = Humans.Get(gameContext);

        foreach (var c in companies)
        {
            var culture = Companies.GetActualCorporateCulture(c);

            List<int> defectedManagers = new List<int>();
            var recruitedManagers = new List<ExpiringJobOffer>();

            // gain expertise and recalculate loyalty
            foreach (var team in c.team.Teams)
            {
                var managers = team.Managers.Select(m => Humans.Get(humans, m)); //  humans.First(h => h.human.Id == m) //  Humans.Get(humans, m)
                bool tooManyLeaders = managers.Count(m => m.humanSkills.Traits.Contains(Trait.Leader)) >= 2;

                team.TooManyLeaders = tooManyLeaders;

                foreach (var human in managers)
                {
                    // var human = managers.First(m => m.human.Id == humanId);

                    var relationship = human.humanCompanyRelationship;

                    var loyaltyChange = Teams.GetLoyaltyChangeForManager(human, team, culture, c);

                    var newLoyalty = Mathf.Clamp(relationship.Morale + loyaltyChange, 0, 100);
                    var newAdaptation = Mathf.Clamp(relationship.Adapted + 5, 0, 100);

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
                        defectedManagers.Add(human.human.Id);
                    else
                    {
                        // if has offers
                        // choose best one
                        var offers = human.workerOffers.Offers;

                        // has competing offers
                        if (Humans.HasCompetingOffers(human))
                        {
                            var desires = offers.Select(offer => Teams.GetOpinionAboutOffer(human, offer).Sum());

                            var maxDesire = desires.Max();

                            bool hasOneBestOffer = desires.Count(o => o >= maxDesire) == 1;

                            if (hasOneBestOffer)
                            {
                                // can choose best one
                                var bestOffer = offers.Find(e => Teams.GetOpinionAboutOffer(human, e).Sum() >= maxDesire);

                                // if best offer was made by another company
                                if (bestOffer.CompanyId != c.company.Id)
                                {
                                    // and company did nothing during one month
                                    if (bestOffer.DecisionDate < date)
                                    {
                                        recruitedManagers.Add(bestOffer);
                                        bestOffer.Accepted = true;

                                        human.workerOffers.Offers.Clear();
                                        human.workerOffers.Offers.Add(bestOffer);
                                    }
                                }
                            }
                            else
                            {
                                // otherwise, companies need to resend their offers
                                // or they will be expired
                            }
                        }
                    }
                }
            }

            // fire managers
            foreach (var humanId in defectedManagers)
            {
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

                    bool wantsToWorkInYourCompany = false; // UnityEngine.Random.Range(0, 100) < 50;

                    // // NotifyPlayer
                    if (worksInPlayerCompetitorCompany && wantsToWorkInYourCompany)
                        NotificationUtils.AddPopup(gameContext, new PopupMessageWorkerWantsToWorkInYourCompany(c.company.Id, humanId));

                    // or this worker will start his own bussiness in same/adjacent sphere

                    // or will be destroyed
                }
            }

            foreach (var offer in recruitedManagers)
            {
                var human = humans.First(h => h.human.Id == offer.HumanId);

                var company = companies.First(c1 => c1.company.Id == offer.CompanyId);
                var previousCompany = c; // companies.First(c1 => c1.company.Id == human.worker.companyId);

                Debug.Log($"Recruiting manager {Humans.GetFullName(human)} from {previousCompany.company.Name} to {company.company.Name}");

                Teams.HuntManager(human, company, gameContext, 0);
                Teams.SetJobOffer(human, company, offer.JobOffer, 0, gameContext);

                if (Companies.IsPlayerFlagship(previousCompany))
                {
                    NotificationUtils.AddNotification(gameContext, new NotificationMessageManagerRecruiting(offer.HumanId, company.company.Id, true));
                }
            }

            // clean expired offers
            foreach (var t in c.team.Teams)
            {
                foreach (var humanId in t.Managers)
                {
                    var human = humans.First(h => h.human.Id == humanId); // Humans.Get(humans, humanId);

                    human.workerOffers.Offers.RemoveAll(o => o.DecisionDate < date && !o.Accepted);
                }
            }
        }
    }
}