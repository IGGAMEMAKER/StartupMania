﻿using Entitas;
using System.Collections.Generic;

namespace Assets.Core
{
    public static partial class ScreenUtils
    {
        public static GameEntity GetMenu(GameContext gameContext)
        {
            var entities = gameContext.GetEntities(GameMatcher.Menu);
            
            if (entities.Length == 0)
                return CreateMenu(gameContext);
            else
                return entities[0];
        }

        public static GameEntity GetPlayer(GameContext gameContext)
        {
            return gameContext.GetEntities(GameMatcher.Player)[0];
        }

        public static GameEntity CreateMenu(GameContext gameContext) => CreateMenu(gameContext.CreateEntity());
        public static GameEntity CreateMenu(GameEntity menu)
        {
            menu.AddNavigationHistory(new List<MenuComponent>());

            var dictionary = new Dictionary<string, object>
            {
                [C.MENU_SELECTED_COMPANY] = 1,
                [C.MENU_SELECTED_INDUSTRY] = IndustryType.Technology,
                [C.MENU_SELECTED_NICHE] = NicheType.Tech_SearchEngine,
                [C.MENU_SELECTED_HUMAN] = 0,
                [C.MENU_SELECTED_INVESTOR] = -1,
                [C.MENU_SELECTED_TEAM] = 0,
                [C.MENU_SELECTED_MAIN_SCREEN_PANEL_ID] = 0
            };

            menu.AddMenu(ScreenMode.HoldingScreen, dictionary);

            return menu;
        }

        // update menus
        public static void UpdateScreen(GameContext context, ScreenMode screenMode, Dictionary<string, object> data)
        {
            var menu = GetMenu(context);
                
            menu.ReplaceMenu(screenMode, data);
        }

        public static void UpdateScreen(GameContext context)
        {
            var menu = GetMenu(context);

            UpdateScreen(context, menu.menu.ScreenMode, menu.menu.Data);
        }
    }
}
