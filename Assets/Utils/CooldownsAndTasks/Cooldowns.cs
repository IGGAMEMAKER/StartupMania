﻿using Entitas;
using System.Collections.Generic;

namespace Assets.Core
{
    partial class Cooldowns
    {
        //// new cooldown system
        //private static GameEntity GetCooldownContainer(GameContext gameContext)
        //{
        //    return gameContext.GetEntities(GameMatcher.CooldownContainer)[0];
        //}

        //public static Dictionary<string, Cooldown> GetCooldowns(GameContext gameContext)
        //{
        //    var container = GetCooldownContainer(gameContext);

        //    return container.cooldownContainer.Cooldowns;
        //}


        //public static bool HasCooldown(GameContext gameContext, Cooldown cooldown) => HasCooldown(gameContext, cooldown.GetKey());
        //public static bool HasCooldown(GameContext gameContext, string cooldownName)
        //{
        //    return GetCooldowns(gameContext).ContainsKey(cooldownName);
        //}

        //public static void AddCooldown(GameContext gameContext, CompanyTask cooldown, int duration)
        //{
        //    AddTask(gameContext, cooldown, duration);
        //    var cooldowns = GetCooldowns(gameContext);

        //    cooldown.EndDate = ScheduleUtils.GetCurrentDate(gameContext) + duration;
        //}

        //public static bool TryGetCooldown(GameContext gameContext, Cooldown req, out Cooldown cooldown) => TryGetCooldown(gameContext, req.GetKey(), out cooldown);
        //public static bool TryGetCooldown(GameContext gameContext, string cooldownName, out Cooldown cooldown)
        //{
        //    return GetCooldowns(gameContext).TryGetValue(cooldownName, out cooldown);
        //}
    }
}
