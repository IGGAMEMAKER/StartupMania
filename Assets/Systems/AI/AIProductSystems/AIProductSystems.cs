﻿using Entitas;
using System.Collections.Generic;

public enum ProductCompanyGoals
{
    Survive,
    FixClientLoyalty,
    CompleteCompanyGoal,
    Develop,
    TakeTechLeadership
}

public partial class AIProductSystems : OnDateChange
{
    public AIProductSystems(Contexts contexts) : base(contexts) {}

    GameEntity[] GetAIProducts()
    {
        return gameContext.GetEntities(
            GameMatcher
            .AllOf(GameMatcher.Product)
            //.NoneOf(GameMatcher.ControlledByPlayer)
        );
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in GetAIProducts())
            ChooseGoal(e);
    }

    void ChooseGoal(GameEntity product)
    {
        var goals = new Dictionary<ProductCompanyGoals, long>
        {
            // threats
            [ProductCompanyGoals.Survive] = GetBankruptcyUrgency(product),
            [ProductCompanyGoals.FixClientLoyalty] = GetLoyaltyUrgency(product),

            // company goal
            [ProductCompanyGoals.CompleteCompanyGoal] = GetCompanyGoalUrgency(product),
        };
        
        var goal = PickMostImportantValue(goals);

        ExecuteGoal(goal, product);
    }

    static T PickMostImportantValue<T> (Dictionary<T, long> values)
    {
        long value = 0;
        T goal = default;

        foreach (var pair in values)
        {
            if (pair.Value > value)
            {
                value = pair.Value;
                goal = pair.Key;
            }
        }

        return goal;
    }

    void ExecuteGoal(ProductCompanyGoals goal, GameEntity product)
    {
        switch (goal)
        {
            case ProductCompanyGoals.Survive: Survive(product); break;
            case ProductCompanyGoals.FixClientLoyalty: FixLoyalty(product); break;

            default: CompleteCompanyGoal(product); break;
        }
    }
}
