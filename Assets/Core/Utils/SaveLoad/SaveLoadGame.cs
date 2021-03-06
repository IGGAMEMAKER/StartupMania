﻿using System;
using System.IO;
using UnityEngine;

using System.Collections.Generic;
using Entitas;
using System.Linq;

namespace Assets.Core
{
    public partial class State
    {
        public static void SaveGame(GameContext Q)
        {
            var entities = Q.GetEntities()
                //.Where(e => !e.hasAnyCompanyGoalListener)
                //.Where(e => !e.hasAnyCompanyListener)
                //.Where(e => !e.hasAnyDateListener)
                //.Where(e => !e.hasAnyNotificationsListener)
                .ToArray()
                ;

            SaveEntities(entities, Q);
        }

        public static void SaveEntities(GameEntity[] entities, GameContext gameContext)
        {
            var fileName = "entities.dat";

            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

            var entityData = new Dictionary<int, IComponent[]>();



            foreach (var e in entities)
            {
                while (e.hasAnyDateListener)
                    e.RemoveAnyDateListener();

                while (e.hasDateListener)
                    e.RemoveDateListener();

                while (e.hasMenuListener)
                    e.RemoveMenuListener();

                while (e.hasAnyCompanyGoalListener)
                    e.RemoveAnyCompanyGoalListener();

                while (e.hasAnyCompanyListener)
                    e.RemoveAnyCompanyListener();

                while (e.hasAnyNotificationsListener)
                    e.RemoveAnyNotificationsListener();

                while (e.hasNavigationHistoryListener)
                    e.RemoveNavigationHistoryListener();

                // new wave of listeners
                while (e.hasMarketingListener)
                    e.RemoveMarketingListener();

                while (e.hasBrandingListener)
                    e.RemoveBrandingListener();

                while (e.hasProductListener)
                    e.RemoveProductListener();

                while (e.hasTeamListener)
                    e.RemoveTeamListener();

                while (e.hasAnyGameEventContainerListener)
                    e.RemoveAnyGameEventContainerListener();

                while (e.hasAnyGamePausedListener)
                    e.RemoveAnyGamePausedListener();

                while (e.hasAnyTimerRunningListener)
                    e.RemoveAnyTimerRunningListener();
                //

                RemoveUselessDataFromProduct(e);
                
                var comps = e.GetComponents()
                    //.Where(c => c.GetType() != typeof(INavigationHistoryListener))
                    .ToArray();

                entityData[e.creationIndex] = comps;
            }

            using (StreamWriter sw = new StreamWriter(fileName))
            using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                if (entityData.Count > 0)
                {
                    Debug.Log("Serializing data " + entityData.Count);
                    serializer.Serialize(writer, entityData);

                    Debug.Log("Serialized " + entityData.Count);
                }
            }
        }

        public static void RemoveUselessDataFromProduct(GameEntity e)
        {
            if (e.hasProduct)
            {
                e.RemoveNicheSegments();
                e.RemoveChannelInfos();
                e.RemoveNicheBaseProfile();
            }
        }

        public static void AddPerformanceImprovingDataToProduct(GameEntity e, GameContext gameContext)
        {
            if (e.hasProduct)
            {
                Companies.WrapProductWithAdditionalData(e, gameContext);
            }
        }

        public static void ClearEntities()
        {
            Contexts.sharedInstance.game.DestroyAllEntities();
        }

        public static void LoadGameData(GameContext gameContext)
        {
            ClearEntities();
            LoadEntities(gameContext);

            ScheduleUtils.PauseGame(gameContext);

            //var MyCompany = Companies.GetPlayerCompany(gameContext);
            //var playerFlagship = Companies.GetFlagship(gameContext, MyCompany) ?? null;

            //ScreenUtils.SetSelectedHuman(gameContext, ScreenUtils.GetPlayer(gameContext).human.Id);
            //ScreenUtils.SetSelectedCompany(gameContext, Companies.GetPlayerFlagshipID(gameContext));

            //ScreenUtils.SetSelectedNiche(gameContext, NicheType.Com_Blogs);
        }

        public static void LoadEntities(GameContext gameContext)
        {
            var fileName = "entities.dat";

            Dictionary<int, IComponent[]> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, IComponent[]>>(File.ReadAllText(fileName), new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            });


            var componentTypes = new Dictionary<Type, int>();

            for (var i = 0; i < GameComponentsLookup.componentTypes.Length; i++)
            {
                var t = GameComponentsLookup.componentTypes[i];

                componentTypes[t] = i;
            }


            foreach (var kvp in obj)
            {
                var e = gameContext.CreateEntity();

                var entityIndex = kvp.Key;
                var components = kvp.Value;

                foreach (var c in components)
                {
                    //Debug.Log("Read component: " + c);

                    var componentIndex = componentTypes[c.GetType()];

                    e.AddComponent(componentIndex, c);
                }

                AddPerformanceImprovingDataToProduct(e, gameContext);
            }
        }
    }
}
