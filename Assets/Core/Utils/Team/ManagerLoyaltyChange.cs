﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Core
{
    public static partial class Teams
    {
        // loyalty drop
        public static List<CorporatePolicy> GetImportantCorporatePolicies()
        {
            return new List<CorporatePolicy>
            {
                CorporatePolicy.Make,
                CorporatePolicy.CompetitionOrSupport,
            };
        }

        public static TeamInfo GetTeamOf(GameEntity human, GameContext gameContext)
        {
            return GetTeamOf(human, Companies.Get(gameContext, human.worker.companyId));
        }

        public static TeamInfo GetTeamOf(GameEntity human, GameEntity company)
        {
            return company.team.Teams.Find(t => t.Managers.Contains(human.human.Id));
        }

        public static Bonus<float> GetOpinionAboutOffer(GameEntity worker, ExpiringJobOffer newOffer)
        {
            bool willNeedToLeaveCompany = worker.worker.companyId != newOffer.CompanyId;

            var bonus = new Bonus<float>("Opinion about offer");

            // scenarios
            // 1 - unemployed
            // 2 - employed, same company
            // 3 - employed, recruiting
            // 4 - !founder

            if (!Humans.IsEmployed(worker))
                return bonus.Append("Salary", newOffer.JobOffer.Salary > GetSalaryPerRating(worker) ? 1 : -1);

            int loyaltyBonus = (worker.humanCompanyRelationship.Morale - 50) / 10;
            int desireToLeaveCompany = 0;

            if (willNeedToLeaveCompany)
            {
                // it's not easy to recruit worker from other company
                desireToLeaveCompany -= 5;

                // and if your worker loves stability...
                if (worker.humanSkills.Traits.Contains(Trait.Loyal))
                    desireToLeaveCompany -= 5;

                // but if your worker loves new challenges...
                if (worker.humanSkills.Traits.Contains(Trait.NewChallenges))
                    desireToLeaveCompany += 10;

                if (desireToLeaveCompany > 0)
                    bonus.AppendAndHideIfZero("Wants to leave company", desireToLeaveCompany);
                else
                    bonus.AppendAndHideIfZero("Wants to stay in company", desireToLeaveCompany);

                bonus.Append("Loyalty to company", -loyaltyBonus);
            }
            else
            {
                // prolongation of contract
                bonus.Append("Loyalty to company", loyaltyBonus);
            }

            long newSalary = newOffer.JobOffer.Salary;

            long salary;
            salary = (long) Mathf.Max(Humans.GetCurrentOffer(worker).JobOffer.Salary, 1);

            float salaryRatio;
            salaryRatio = (newSalary - salary) * 1f / salary;
            salaryRatio = Mathf.Clamp(salaryRatio, -5, 5);

            bonus.Append("Salary", salaryRatio);


            return bonus;
        }

        public static int GetLoyaltyChangeForManager(GameEntity worker, TeamInfo team, GameEntity company)
        {
            //var company = Companies.Get(gameContext, worker.worker.companyId);

            var culture = Companies.GetActualCorporateCulture(company);

            return GetLoyaltyChangeForManager(worker, team, culture, company);
        }

        public static int GetLoyaltyChangeForManager(GameEntity worker, TeamInfo team,
            Dictionary<CorporatePolicy, int> culture, GameEntity company)
        {
            return GetLoyaltyChangeBonus(worker, team, culture, company).Sum();
        }

        public static Bonus<int> GetLoyaltyChangeBonus(GameEntity worker, TeamInfo team,
            Dictionary<CorporatePolicy, int> culture, GameEntity company)
        {
            var bonus = new Bonus<int>("Loyalty");

            bonus.Append("Base value", 1);

            var loyaltyBuff = team.ManagerTasks.Count(t => t == ManagerTask.ImproveAtmosphere);

            bonus.AppendAndHideIfZero("Manager focus on atmosphere", loyaltyBuff);

            var role = worker.worker.WorkerRole;

            // TODO: if is CEO in own project, morale loss is zero or very low
            bonus.AppendAndHideIfZero("IS FOUNDER", worker.hasCEO ? 5 : 0);

            // same role workers
            //ApplyDuplicateWorkersLoyalty(company, team, gameContext, ref bonus, worker, role);

            // salary
            ApplyLowSalaryLoyalty(company, ref bonus, worker);

            // incompetent leader
            //ApplyCEOLoyalty(company, team, gameContext, ref bonus, worker, role);

            // no possibilities to grow
            if (role != WorkerRole.CEO)
                bonus.AppendAndHideIfZero("Reached limits", Humans.GetRating(worker) >= 70 ? -3 : 0);

            bonus.AppendAndHideIfZero("Too many leaders",
                worker.humanSkills.Traits.Contains(Trait.Leader) && team.TooManyLeaders ? -2 : 0);
            // bonus.AppendAndHideIfZero(hu)
            return bonus;
        }

        private static void ApplyLowSalaryLoyalty(GameEntity company, ref Bonus<int> bonus, GameEntity worker)
        {
            bool isFounder = worker.hasShareholder; // &&
                             // company.shareholders.Shareholders.ContainsKey(worker.shareholder.Id);

            if (isFounder)
                return;

            var salary = Humans.GetSalary(worker);

            var expectedSalary = (double) GetSalaryPerRating(worker);

            bool isGreedy = worker.humanSkills.Traits.Contains(Trait.Greedy);
            bool isShy = worker.humanSkills.Traits.Contains(Trait.Shy);

            float multiplier = 0.8f;

            if (isGreedy)
            {
                multiplier = 0.9f;
            }
            else if (isShy)
            {
                multiplier = 0.5f;
            }

            // multiply on 4 cause period = week
            if (salary * 4 < expectedSalary * multiplier)
                bonus.Append("Low salary", -5);
        }

        private static void ApplyDuplicateWorkersLoyalty(GameEntity company, TeamInfo team, GameContext gameContext,
            ref Bonus<int> bonus, GameEntity worker, WorkerRole role)
        {
            var roles = team.Managers.Select(humanId => Humans.Get(gameContext, humanId).worker.WorkerRole);
            bool hasDuplicateWorkers = roles.Count(r => r == role) > 1;

            if (hasDuplicateWorkers)
                bonus.AppendAndHideIfZero("Too many " + Humans.GetFormattedRole(role) + "'s", -10);
        }

        private static void ApplyCEOLoyalty(GameEntity company, TeamInfo team, GameContext gameContext,
            ref Bonus<int> bonus, GameEntity worker, WorkerRole role)
        {
            bool hasCeo = HasMainManagerInTeam(company.team.Teams[0]);

            bonus.AppendAndHideIfZero("No CEO", hasCeo ? 0 : -4);

            var manager = GetMainManagerRole(team);
            var lead = GetWorkerByRole(manager, team, gameContext);

            if (lead == null)
            {
                bonus.Append($"No {Humans.GetFormattedRole(manager)} in team", -4);
            }
            else
            {
                var CEORating = Humans.GetRating(lead);
                var workerRating = Humans.GetRating(worker);

                if (CEORating < workerRating)
                    bonus.Append($"Incompetent leader (leader rating less than {workerRating})", -1);
            }
        }
    }
}