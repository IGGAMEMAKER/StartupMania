﻿

// public class ScheduleInitializeSystem : IInitializeSystem
// {
//     readonly GameContext _context;
//
//     public ScheduleInitializeSystem(Contexts contexts)
//     {
//         _context = contexts.game;
//     }
//
//     void IInitializeSystem.Initialize()
//     {
//         var DateEntity = _context.CreateEntity();
//         DateEntity.AddDate(0);
//         DateEntity.AddSpeed(3);
//         DateEntity.AddProfiling(0, new StringBuilder());
//
//         ScheduleUtils.PauseGame(_context);
//     }
// }
