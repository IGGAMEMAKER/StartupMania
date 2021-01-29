﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using Assets.Core;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// Read
// Как-работает-editorwindow-ongui-в-unity-3d
// https://ru.stackoverflow.com/questions/515395/%D0%9A%D0%B0%D0%BA-%D1%80%D0%B0%D0%B1%D0%BE%D1%82%D0%B0%D0%B5%D1%82-editorwindow-ongui-%D0%B2-unity-3d

// https://answers.unity.com/questions/37180/how-to-highlight-or-select-an-asset-in-project-win.html
// https://gist.github.com/rutcreate/0af3c34abd497a2bceed506f953308d7
// https://stackoverflow.com/questions/36850296/get-a-prefabs-file-location-in-unity
// https://forum.unity.com/threads/dropdown-in-inspector.468739/
// https://forum.unity.com/threads/editorguilayout-scrollview-not-working.143502/

// optional
// https://answers.unity.com/questions/201848/how-to-create-a-drop-down-menu-in-editor.html
// https://gist.github.com/bzgeb/3800350
// GUID http://www.unity3d.ru/distribution/viewtopic.php?f=18&t=4640


//string myString = "Hello World";
//bool groupEnabled;
//bool myBool = true;
//float myFloat = 1.23f;
// void RenderExample()
// {
// // myString = EditorGUILayout.TextField ("Text Field", myString);
//         
// // groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
// // myBool = EditorGUILayout.Toggle ("Toggle", myBool);
// // myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
// // EditorGUILayout.EndToggleGroup ();
// }



public enum SceneBlahType
{
    Prefab,
    Scene
}

public struct UrlOpeningAttempt
{
    public string ScriptName;
    public string PreviousUrl;
}

public struct SimpleUISceneType
{
    public string Url;
    public string Name;
    public string AssetPath;
    public bool Exists;

    public long Usages;
    public long LastOpened;

    //public SceneBlahType AssetType => SimpleUI.isPrefabAsset(AssetPath) ? SceneBlahType.Prefab : SceneBlahType.Scene;

    public SimpleUISceneType(string url, string assetPath, string name = "")
    {
        Url = url;
        AssetPath = assetPath;
        Name = name.Length > 0 ? name : url;
        Exists = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath) != null;

        Usages = 0;
        LastOpened = 0;
    }
}

public partial class SimpleUI : EditorWindow
{
    private Vector2 recentPrefabsScrollPosition = Vector2.zero;

    static bool isDraggedPrefabMode = false;
    static bool isDraggedGameObjectMode = false;
    static bool isUrlEditingMode = false;
    static bool isPrefabChosenMode = false;
    static bool isUrlAddingMode = false;
    static bool isConcreteUrlChosen = false;

    static bool wasOpenedFromProject = false;

    public static List<SimpleUI.PrefabMatchInfo> allReferencesFromAssets;
    public static List<SimpleUI.UsageInfo> referencesFromCode;

    //static bool _isSceneMode = true;

    static bool isPrefabMode => PrefabStageUtility.GetCurrentPrefabStage() != null;

    static int ChosenIndex => prefabs.FindIndex(p => p.Url.Equals(GetCurrentUrl())); // GetCurrentUrl()
    static bool hasChosenPrefab => ChosenIndex >= 0;

    public static string GetCurrentUrl() => newUrl.StartsWith("/") ? newUrl : "/" + newUrl;
    public static string GetCurrentAssetPath() => GetOpenedAssetPath(); // newPath

    static string GetOpenedAssetPath()
    {
        if (isPrefabMode)
        {
            return PrefabStageUtility.GetCurrentPrefabStage().assetPath;
        }

        return SceneManager.GetActiveScene().path;
    }

    [MenuItem("Window/SIMPLE UI")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        // EditorWindow.GetWindow(typeof(SimpleUI), false, "Simple UI", true);
        var w = EditorWindow.GetWindow(typeof(SimpleUI));
        // w.minSize = new Vector2(200, 100);
    }

    void OnGUI()
    {
        recentPrefabsScrollPosition = GUILayout.BeginScrollView(recentPrefabsScrollPosition);
        GUILayout.Label("SIMPLE UI", EditorStyles.largeLabel);
        GUILayout.Label("newUrl " + newUrl);
        GUILayout.Label("newPath " + newPath);

        //RenderExistingTroubles();
        //Space();
        //if (Button("Print OpenUrl info"))
        //{
        //    PrintMatchInfo(WhatUsesComponent<OpenUrl>());
        //}

        if (!hasChosenPrefab)
            RenderPrefabs();

        if (isDraggedGameObjectMode)
            RenderMakingAPrefabFromGameObject();
        else if (isDraggedPrefabMode)
            RenderAddingNewRouteFromDraggedPrefab();
        else if (hasChosenPrefab)
            RenderChosenPrefab();
        else
            RenderAddingNewRoute();

        HandleDragAndDrop();

        GUILayout.EndScrollView();
    }

    static SimpleUI()
    {
        PrefabStage.prefabStageOpened += PrefabStage_prefabOpened;
        PrefabStage.prefabStageClosing += PrefabStage_prefabClosed;

        //EditorSceneManager.activeSceneChangedInEditMode += EditorSceneManager_activeSceneChangedInEditMode;
        //EditorSceneManager.sceneOpened += EditorSceneManager_sceneOpened;
        ////EditorSceneManager.sceneLoaded += EditorSceneManager_sceneLoaded;

        ////SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        ////SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        ////SceneManager.activeSceneChanged += SceneManager_sceneChanged;
    }

    //private static void EditorSceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    Debug.Log("Editor scene LOADED");
    //}

    private static void EditorSceneManager_sceneOpened(Scene scene, OpenSceneMode mode)
    {
        Debug.Log("Editor scene OPENED " + scene.name);
        //scene.GetRootGameObjects().First();
    }

    private static void EditorSceneManager_activeSceneChangedInEditMode(Scene arg0, Scene arg1)
    {
        Debug.Log($"Editor scene CHANGED from {arg0.name} to {arg1.name}");

        var obj = WrapSceneWithMenu();
        Selection.activeGameObject = obj;
    }

    //private static void SceneManager_sceneChanged(Scene arg0, Scene arg1)
    //{
    //    Debug.Log($"Scene changed from {arg0.name} to {arg1.name}");

    //    //SceneManager_sceneUnloaded(arg0);
    //    //SceneManager_sceneLoaded(arg0, LoadSceneMode.Additive);
    //}

    //private static void SceneManager_sceneUnloaded(Scene arg0)
    //{
    //    Debug.Log("Scene unloaded");

    //    DestroyImmediate(FindObjectOfType<DisplayConnectedUrls>());
    //}

    //private static void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    //{
    //    Debug.Log("Scene loaded");

    //    var go = WrapSceneWithMenu();
    //    Selection.activeGameObject = go;
    //}

    static GameObject WrapSceneWithMenu()
    {
        if (FindObjectOfType<DisplayConnectedUrls>() != null)
            return null;

        if (!IsAssetPathExists(GetCurrentAssetPath()))
        {
            // this scene was not attached to any url
            return null;
        }

        var go = new GameObject("SimpleUI Menu", typeof(DisplayConnectedUrls));
        go.transform.SetAsFirstSibling();

        return go;
    }

    private static void PrefabStage_prefabClosed(PrefabStage obj)
    {
        Debug.Log("prefab closed");
        DestroyImmediate(obj.prefabContentsRoot.GetComponent<DisplayConnectedUrls>());
    }

    private static void PrefabStage_prefabOpened(PrefabStage obj)
    {
        Debug.Log("Prefab opened: " + obj.prefabContentsRoot.name);

        // Wrap with SimpleUI menues
        obj.prefabContentsRoot.AddComponent<DisplayConnectedUrls>();
        Selection.activeGameObject = obj.prefabContentsRoot;


        newPath = obj.assetPath;
        newName = GetPrettyNameFromAssetPath(newPath); // x.Substring(0, ind);
        
        // choose URL
        ChooseUrlFromPickedPrefab();
        TryToIncreaseCurrentPrefabCounter();
    }

    void OnInspectorUpdate()
    {
        var url = newUrl;
        var path = GetOpenedAssetPath();
        ChooseUrlFromPickedPrefab();

        // no matching urls
        if (newUrl.Equals(""))
            SetAddingRouteMode();

        bool objectChanged = !newPath.Equals(path); // || !newUrl.Equals(url);

        if (objectChanged)
        {
            Debug.Log("Object changed");
            newPath = path;

            TryToIncreaseCurrentPrefabCounter();
        }

        if (!isPrefabMode)
        {
            WrapSceneWithMenu();
        }
    }

    static void ChooseUrlFromPickedPrefab()
    {
        var path = GetOpenedAssetPath();
        var urls = prefabs.Where(p => p.AssetPath.Equals(path));

        if (urls.Count() == 0)
        {
            newUrl = "";
        }

        if (urls.Count() == 1)
        {
            newUrl = urls.First().Url;
            isConcreteUrlChosen = true;
        }

        if (urls.Count() > 1)
        {
            // pick first automatically or do nothing?
            isConcreteUrlChosen = false;
        }
    }


    #region Open Asset
    public static void OpenAssetByPath(string path)
    {
        AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath(path));
    }

    public static void OpenPrefabByAssetPath(string path)
    {
        if (!SimpleUI.IsAssetPathExists(path))
        {
            //Debug.LogError("Failed to OpenPrefabByAssetPath() " + path);
            OpenAssetByPath(path);
            return;
        }

        var p1 = prefabs.First(p => p.AssetPath.Equals(path));

        OpenPrefab(p1);
    }
    public static void OpenPrefabByUrl(string url)
    {
        Debug.Log("Trying to open prefab by url: " + url);

        if (!url.StartsWith("/"))
            url = "/" + url;

        var p1 = prefabs.First(p => p.Url.Equals(url));

        OpenPrefab(p1);
    }
    static void OpenPrefab(SimpleUISceneType p)
    {
        newPath = p.AssetPath;
        newUrl = p.Url;

        PossiblePrefab = null;
        isDraggedPrefabMode = false;
        isUrlEditingMode = false;
        isConcreteUrlChosen = true;

        // calculate previous DisplayConnectuedUrlsEditor.OnEnable() here
        allReferencesFromAssets = WhatUsesComponent<OpenUrl>();
        referencesFromCode = WhichScriptReferencesConcreteUrl(newUrl);

        OpenAssetByPath(newPath);
    }
    #endregion

    #region UI shortcuts
    public static bool Button(string text)
    {
        GUIStyle style = GUI.skin.FindStyle("Button");
        style.richText = true;

        if (!text.Contains("\n"))
            text += "\n";
        
        return GUILayout.Button($"<b>{text}</b>", style);
    }

    public static void Label(string text)
    {
        Space();
        BoldLabel(text);
    }

    public static void BoldLabel(string text)
    {
        GUILayout.Label(text, EditorStyles.boldLabel);
    }

    public static void Space(int space = 15)
    {
        GUILayout.Space(space);
    }
    #endregion

    #region string utils
    public static IEnumerable<SimpleUISceneType> GetSubUrls(string url, bool recursive) => prefabs.Where(p => isSubRouteOf(p.Url, url, recursive));


    /// <summary>
    /// if recursive == false
    /// function will return true ONLY for DIRECT subroutes
    /// 
    /// otherwise it will return true for sub/sub routes too
    /// </summary>
    /// <param name="subUrl"></param>
    /// <param name="root"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public static bool isSubRouteOf(string subUrl, string root, bool recursive)
    {
        // searching for /ProjectScreen descendants

        // filter /Abracadabra
        if (!subUrl.StartsWith(root))
            return false;

        // filter self
        if (subUrl.Equals(root))
            return false;

        var subStr = subUrl.Substring(root.Length);

        if (root.Equals("/"))
            subStr = subUrl;

        bool isUrlItself = subStr.IndexOf('/') == 0;

        // filter /ProjectScreenBLAH
        if (!isUrlItself)
            return false;

        // remaining urls are
        // /ProjectScreen/Blah
        // /ProjectScreen/Blah/Blah

        // subStrs
        // /Blah
        // /Blah/Blah

        bool isDirectSubroute = subStr.LastIndexOf('/') == 0;
        bool isSubSubRoute = subStr.LastIndexOf('/') > 0;

        if (!recursive)
            return isDirectSubroute;
        else
            return true;
    }

    static string GetPrettyNameFromAssetPath(string path)
    {
        var x = path.Split('/').Last();
        var ind = x.LastIndexOf(".prefab");

        if (isSceneAsset(x))
        {
            ind = x.LastIndexOf(".unity");
        }

        return x.Substring(0, ind);
    }

    public static string GetPrettyNameForExistingUrl(string url)
    {
        var prefab = GetPrefabByUrl(url);

        return prefab.Name;
    }

    public static string GetUpperUrl(string url)
    {
        var index = url.LastIndexOf("/");

        if (index <= 0)
            return "/";

        return url.Substring(0, index);
    }

    bool Contains(string text1, string searching)
    {
        return text1.ToLower().Contains(searching.ToLower());
    }

    string GetValidatedUrl(string url)
    {
        if (!url.StartsWith("/"))
            return url.Insert(0, "/");

        return url;
    }
    #endregion


    #region Troubleshooting
    public static void FindMissingAssets()
    {
        var prefs = prefabs;

        for (var i = 0; i < prefs.Count; i++)
        {
            var p = prefs[i];

            p.Exists = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(p.AssetPath) != null; // Directory.Exists(p.AssetPath);

            UpdatePrefab(p, i);
        }
    }

    public static void AddMissingUrl(string url, string scriptName, string FunctionName)
    {
        if (!UrlOpeningAttempts.ContainsKey(url))
            UrlOpeningAttempts[url] = new List<UrlOpeningAttempt>();

        UrlOpeningAttempts[url].Add(new UrlOpeningAttempt { PreviousUrl = GetCurrentUrl(), ScriptName = scriptName });

        SaveData();
    }

    void RenderMissingUrls()
    {
        if (UrlOpeningAttempts.Any())
            Label("Failed to open these URLs from code");

        Space();
        foreach (var missing in UrlOpeningAttempts)
        {
            var scripts = string.Join("\n", missing.Value.Select(m => m.PreviousUrl));

            EditorGUILayout.HelpBox($"Tried to open url <b>{missing.Key}</b> from, but failed", MessageType.Error, true);
            EditorGUILayout.HelpBox($"Did that from {scripts}", MessageType.Warning);
        }

        if (UrlOpeningAttempts.Any())
        {
            Space();
            if (Button("Dismiss warnings"))
            {
                UrlOpeningAttempts.Clear();
                SaveData();
            }
        }
    }

    void RenderExistingTroubles()
    {
        if (prefabs.Count == 0)
            return;

        if (GUILayout.Button("Find missing assets"))
        {
            FindMissingAssets();
        }

        RenderMissingUrls();
        RenderMissingAssets();
    }

    void RenderMissingAssets()
    {
        Space();
        var missingAssets = prefabs.FindAll(p => !p.Exists);

        if (missingAssets.Any())
            Label("These urls are missing assets");

        foreach (var missing in missingAssets)
        {
            EditorGUILayout.HelpBox($"Url {missing.Url} is missing an asset {missing.AssetPath}", MessageType.Error, true);
            EditorGUILayout.HelpBox($"You can fix the link to the asset or move the asset to path {missing.AssetPath}", MessageType.Info, true);

            if (GUILayout.Button("Fix the issue"))
            {
                isUrlEditingMode = true;
                OpenPrefab(missing);
            }
        }
    }
    #endregion

    static void SetAddingRouteMode()
    {
        var path = GetOpenedAssetPath();

        // pick values from asset path
        newName = GetPrettyNameFromAssetPath(path);

        if (!newUrl.EndsWith("/"))
            newUrl += "/";

        newUrl += newName;

        isUrlAddingMode = true;
    }

    void RenderAddingNewRoute()
    {
        if (!isUrlAddingMode)
        {
            SetAddingRouteMode();
        }

        Space();

        var assetType = isPrefabMode ? "prefab" : "SCENE";

        if (!isPrefabMode)
            return;

        var assetPath = GetCurrentAssetPath();
        GUILayout.Label($"Add current asset ({assetType})", EditorStyles.boldLabel);

        newUrl = EditorGUILayout.TextField("Url", newUrl);

        var url = newUrl;

        bool urlOK = url.StartsWith("/");
        bool newNameOK = newName.Length > 0;

        if (urlOK)
        {
            newName = EditorGUILayout.TextField("Name", newName);
        }
        else
        {
            EditorGUILayout.HelpBox("Url needs to start with /", MessageType.Error);
        }

        if (urlOK && newNameOK)
        {
            Space();
            if (GUILayout.Button("Add asset!")) //  <" + newName + ">
            {
                Debug.Log("Added asset");

                AddAsset(url, assetPath, newName);

                SaveData();
            }
        }
    }



    #region Render prefabs
    void RenderRecentPrefabs()
    {
        var sortedByOpenings = prefabs.OrderByDescending(pp => pp.LastOpened);
        var recent = sortedByOpenings.Take(6);

        GUILayout.Label("Recent prefabs", EditorStyles.boldLabel);
        searchUrl = EditorGUILayout.TextField("Search", searchUrl);

        if (searchUrl.Length == 0)
        {
            RenderPrefabs(recent);
        }
        else
        {
            if (Button("Clear"))
            {
                searchUrl = "";
            }

            Space();
            RenderPrefabs(sortedByOpenings.Where(p => Contains(p.Url, searchUrl) || Contains(p.Name, searchUrl)));
        }
    }

    void RenderFavoritePrefabs()
    {
        var top = prefabs.OrderByDescending(pp => pp.Usages).Take(4);

        GUILayout.Label("Favorite prefabs", EditorStyles.boldLabel);
        RenderPrefabs(top);
    }

    void RenderAllPrefabs()
    {
        var top = prefabs.OrderByDescending(pp => pp.Url);

        GUILayout.Label("All prefabs", EditorStyles.boldLabel);
        RenderPrefabs(top);
    }

    void RenderRootPrefab()
    {
        var upperUrl = GetUpperUrl(newUrl);

        bool isTopRoute = newUrl.Equals("/");

        if (!isTopRoute)
        {
            var root = GetPrefabByUrl(upperUrl);
            
            Label($"Root");

            RenderPrefabs(new List<SimpleUISceneType> { root });
        }
    }

    void RenderSubroutes()
    {
        var subUrls = GetSubUrls(newUrl, false);

        if (subUrls.Count() > 0)
        {
            Label("SubRoutes");
            RenderPrefabs(subUrls, newUrl);
        }
    }

    void RenderPrefabs()
    {
        Space();

        RenderFavoritePrefabs();
        RenderRecentPrefabs();
    }
    #endregion


    // ----- utils -------------
    #region Utils
    static SimpleUISceneType GetPrefabByUrl(string url)
    {
        return prefabs.FirstOrDefault(p => p.Url.Equals(url));
    }

    void AddAsset(string url, string assetPath, string name)
    {
        prefabs.Add(new SimpleUISceneType(url, assetPath, name) { LastOpened = DateTime.Now.Ticks });
    }

    void RenderPrefabs(IEnumerable<SimpleUISceneType> list, string trimStart = "")
    {
        foreach (var p in list)
        {
            var c = GUI.color;

            // set color
            bool isChosen = hasChosenPrefab && prefabs[ChosenIndex].AssetPath.Equals(p.AssetPath);

            var color = isChosen ? Color.yellow : Color.white;

            //ColorUtility.TryParseHtmlString(isChosen ? "gold" : "white", out Color color);
            //var color = ColorUtility.TryParseHtmlString(isChosen ? "#FFAB04" Visuals.GetColorFromString(isChosen ? Colors.COLOR_YOU : Colors.COLOR_NEUTRAL);

            GUI.contentColor = color;
            GUI.color = color;
            GUI.backgroundColor = color;


            GUIStyle style = GUI.skin.FindStyle("Button");
            style.richText = true;

            string trimmedUrl = p.Url;

            if (trimStart.Length > 0)
            {
                var lastDashIndex = trimmedUrl.LastIndexOf('/');

                trimmedUrl = trimmedUrl.Substring(lastDashIndex);
                //trimmedUrl = trimmedUrl.Trim(trimStart.ToCharArray());
            }

            if (GUILayout.Button($"<b>{p.Name}</b>\n{trimmedUrl}", style))
            {
                OpenPrefab(p);
            }

            // restore colors
            GUI.contentColor = c;
            GUI.color = c;
            GUI.backgroundColor = c;
        }
    }

    static void TryToIncreaseCurrentPrefabCounter()
    {
        if (hasChosenPrefab)
        {
            var pref = prefabs[ChosenIndex];

            pref.Usages++;
            pref.LastOpened = DateTime.Now.Ticks;

            UpdatePrefab(pref);
        }
    }


    #endregion

    static void Print2(string text)
    {
        Print("PRT2: " + text);
    }
    static void Print(string text)
    {
        Debug.Log(text);
    }

    public static void OpenUrl(string url, string scriptName)
    {
        SimpleUIEventHandler eventHandler = FindObjectOfType<SimpleUIEventHandler>();

        if (eventHandler == null)
        {
            Debug.LogError("SimpleUIEventHandler NOT FOUND");
        }
        else
        {
            var queryIndex = url.IndexOf('?');
            var query = "";

            if (queryIndex >= 0)
            {
                query = url.Substring(queryIndex);
                url = url.Substring(0, queryIndex);
            }

            eventHandler.OpenUrl(url, scriptName);
        }
    }
}

// editing route mode
public partial class SimpleUI
{
    static string searchUrl = "";
    static Vector2 searchScrollPosition = Vector2.zero;

    public static bool renameSubroutes = true;
    public static string newEditingUrl = "";

    public static string newUrl = "";
    public static string newName = "";
    public static string newPath = "";

    void RenderChosenPrefab()
    {
        if (!isConcreteUrlChosen)
        {
            // pick concrete URL
            RenderUrlsWhichAreAttachedToSamePrefab();
        }
        else
        {
            if (isUrlEditingMode)
                RenderEditingPrefabMode();
            else
                RenderLinkToEditing();
        }
    }

    void RenderUrlsWhichAreAttachedToSamePrefab()
    {
        var chosenPrefab = prefabs[ChosenIndex];
        var samePrefabUrls = prefabs.Where(p => p.AssetPath.Equals(chosenPrefab.AssetPath));

        Label("Prefab " + chosenPrefab.Name + " is attached to these urls");
        Label("Choose proper one!");

        Space();
        RenderPrefabs(samePrefabUrls);
    }

    void RenderLinkToEditing()
    {
        var index = ChosenIndex;
        var prefab = prefabs[index];

        Label(prefab.Url);

        if (Button("Edit prefab"))
        {
            isUrlEditingMode = true;

            newUrl = prefab.Url;
            newEditingUrl = newUrl;
            newPath = prefab.AssetPath;
            newName = prefab.Name;
        }

        Space();
        RenderPrefabs();
    }

    void RenameUrl(string from, string to)
    {
        var matches = WhatUsesComponent(from, allReferencesFromAssets);

        try
        {
            //AssetDatabase.StartAssetEditing();

            foreach (var match in matches)
            {
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(match.PrefabAssetPath);
                //var asset = AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath(match.PrefabAssetPath));

                var component = asset.GetComponents<OpenUrl>().First(a => a.GetInstanceID() == match.InstanceID);

                if (component != null && component.Url.Contains(from))
                {
                    var newUrl2 = component.Url.Replace(from, to);

                    Debug.Log("Renaming " + component.Url + " to " + newUrl2);
                }
            }
        }
        finally
        {
            //AssetDatabase.StopAssetEditing();
        }
    }

    void RenderStatButtons(SimpleUISceneType pref)
    {
        Space();
        Space();
        if (pref.Usages > 0 && GUILayout.Button("Reset Counter"))
        {
            pref.Usages = 0;

            UpdatePrefab(pref);
        }

        var maxUsages = prefabs.Max(p => p.Usages);
        if (pref.Usages < maxUsages && GUILayout.Button("Prioritize"))
        {
            pref.Usages = maxUsages + 1;

            UpdatePrefab(pref);
        }
    }

    void RenderRenameUrlButton(SimpleUISceneType prefab)
    {
        Space();

        renameSubroutes = EditorGUILayout.ToggleLeft("Rename subroutes too", renameSubroutes);

        Space();
        if (renameSubroutes)
            EditorGUILayout.HelpBox("Renaming this url will lead to renaming these urls too...", MessageType.Warning);
        else
            EditorGUILayout.HelpBox("Will only rename THIS url", MessageType.Warning);

        List<string> RenamingUrls = new List<string>();
        if (renameSubroutes)
        {
            Space();
            var subroutes = GetSubUrls(prefab.Url, true);

            RenamingUrls.Add(prefab.Url);

            foreach (var route in subroutes)
            {
                RenamingUrls.Add(route.Url);
            }

            // render
            foreach (var route in RenamingUrls)
            {
                BoldLabel(route);
            }
        }

        var phrase = renameSubroutes ? "Rename url & subUrls" : "Rename THIS url";

        var matches = WhatUsesComponent(newUrl, allReferencesFromAssets);


        // references from prefabs & scenes
        var names = matches.Select(m => $"<b>{SimpleUI.GetPrettyAssetType(m.PrefabAssetPath)}</b> " + SimpleUI.GetTrimmedPath(m.PrefabAssetPath)).ToList();
        var routes = matches.Select(m => m.PrefabAssetPath).ToList();

        // references from code
        foreach (var occurence in referencesFromCode)
        {
            names.Add($"<b>Code</b> {SimpleUI.GetTrimmedPath(occurence.ScriptName)}");
            routes.Add(occurence.ScriptName);
        }

        Space();
        if (Button(phrase))
        {
            if (EditorUtility.DisplayDialog("Do you want to rename url " + prefab.Url, "This action will rename url and subUrls in X prefabs, Y scenes and Z script files.\n\nPRESS CANCEL IF YOU HAVE UNSAVED PREFAB OR SCENE OR CODE CHANGES", "Rename", "Cancel"))
            {
                Debug.Log("Rename starts now!");

                foreach (var url in RenamingUrls)
                {
                    RenameUrl(url, url);
                }
            }
            //prefab.Url = newEditingUrl;
            //prefab.Name = newName;
            //prefab.AssetPath = newPath;

            //UpdatePrefab(prefab);
        }

        //EditorUtility.DisplayProgressBar("Renaming url", "Info", UnityEngine.Random.Range(0, 1f));
    }

    void RenderEditingPrefabMode()
    {
        var index = ChosenIndex;
        var prefab = prefabs[index];

        Label(prefab.Url);

        if (Button("Go back"))
        {
            isUrlEditingMode = false;
        }

        var prevUrl = newUrl;
        var prevName = newName;
        var prevPath = newPath;

        Space();



        Label("Edit url");
        newEditingUrl = EditorGUILayout.TextField("Url", newEditingUrl);

        if (newEditingUrl.Length > 0)
        {
            newName = EditorGUILayout.TextField("Name", newName);

            if (newName.Length > 0)
            {
                newPath = EditorGUILayout.TextField("Asset Path", newPath);
            }
        }



        // if data changed, save it
        if (!prevPath.Equals(newPath) || !prevName.Equals(newName))
        {
            //prefab.Url = newEditingUrl;
            prefab.Name = newName;
            prefab.AssetPath = newPath;

            UpdatePrefab(prefab);
        }

        // if Url changed, rename everything
        if (!newUrl.Equals(newEditingUrl))
        {
            RenderRenameUrlButton(prefab);
        }

        Space();
        RenderRootPrefab();
        RenderSubroutes();

        // TODO url or path?
        // opened another url
        if (!newPath.Equals(prevPath))
            return;

        RenderStatButtons(prefab);

        Space(450);
        if (GUILayout.Button("Remove URL"))
        {
            prefabs.RemoveAt(index);
            SaveData();
        }
    }
}

// adding new routes
// dragging prefabs
public partial class SimpleUI
{
    private static GameObject PossiblePrefab;
    private static string possiblePrefabName = "";

    private static string draggedUrl = "";
    private static string draggedName = "";
    private static string draggedPath = "";

    public static bool isSceneAsset(string path) => path.EndsWith(".unity");
    public static bool isPrefabAsset(string path) => path.EndsWith(".prefab");
    public static string GetPrettyAssetType(string path) => isSceneAsset(path) ? "Scene" : "Prefab";

    /// <summary>
    /// cuts directory name / url begginings: 
    /// /blah/test.jpeg => test.jpeg
    /// /blah/test => test
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>

    //var trimmedScriptName = SimpleUI.GetTrimmedPathName(occurence.ScriptName.Substring(occurence.ScriptName.LastIndexOf('/'));
    //var names = matches.Select(m => $"<b>{SimpleUI.GetPrettyAssetType(m.PrefabAssetPath)} </b>" + SimpleUI.GetLastPathName(m.PrefabAssetPath.Substring(m.PrefabAssetPath.LastIndexOf("/"))).ToList();
    public static string GetTrimmedPath(string path) => path.Substring(path.LastIndexOf("/"));

    void HandleDragAndDrop()
    {
        if (Event.current.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            Event.current.Use();
        }
        else if (Event.current.type == EventType.DragPerform)
        {
            // To consume drag data.
            DragAndDrop.AcceptDrag();

            // GameObjects from hierarchy.
            if (DragAndDrop.paths.Length == 0 && DragAndDrop.objectReferences.Length > 0)
            {
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    var go = obj as GameObject;
                    bool isPrefab = PrefabUtility.GetPrefabAssetType(obj) != PrefabAssetType.NotAPrefab;

                    if (isPrefab)
                    {
                        Debug.Log("prefab - " + obj);

                        HandleDraggedPrefab(go);
                    }
                    else
                    {
                        Debug.Log("GameObject - " + obj);

                        HandleDraggedGameObject(go);
                    }
                }
            }
            // Object outside project. It mays from File Explorer (Finder in OSX).
            else if (DragAndDrop.paths.Length > 0 && DragAndDrop.objectReferences.Length == 0)
            {
                Debug.Log("File");
                foreach (string path in DragAndDrop.paths)
                {
                    if (isSceneAsset(path))
                    {
                        Debug.Log("- Dragging Scene! " + path);

                        HandleDraggedScene(path);
                    }
                    else
                    {
                        Debug.Log("- " + path);
                    }
                }
            }
            // Unity Assets including folder.
            else if (DragAndDrop.paths.Length == DragAndDrop.objectReferences.Length)
            {
                Debug.Log("UnityAsset");
                for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
                {
                    var obj = DragAndDrop.objectReferences[i];
                    string path = DragAndDrop.paths[i];
                    Debug.Log(obj.GetType().Name);

                    // Folder.
                    if (obj is DefaultAsset)
                    {
                        Debug.Log(path);
                    }
                    // C# or JavaScript.
                    else if (obj is MonoScript)
                    {
                        Debug.Log(path + "\n" + obj);
                    }
                    else if (obj is Texture2D)
                    {
                    }
                }
            }
            // Log to make sure we cover all cases.
            else
            {
                Debug.Log("Out of reach");
                Debug.Log("Paths:");
                foreach (string path in DragAndDrop.paths)
                {
                    Debug.Log("- " + path);
                }

                Debug.Log("ObjectReferences:");
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    Debug.Log("- " + obj);
                }
            }
        }
    }

    private void RenderMakingAPrefabFromGameObject()
    {
        const string defaultPrefabName = "Bad prefab name: You cannot name new prefab GameObject, cause it's easy to confuse name";

        Space();
        possiblePrefabName = EditorGUILayout.TextField("Name", possiblePrefabName);

        var path = $"Assets/Prefabs/{possiblePrefabName}.prefab";

        bool hasSameNamePrefabAlready = AssetDatabase.LoadAssetAtPath<GameObject>(path) != null;
        bool isEmpty = possiblePrefabName.Length == 0;
        bool isDefaultName = possiblePrefabName.ToLower().Equals("gameobject");

        bool isNameOK = !isEmpty && !isDefaultName && !hasSameNamePrefabAlready;

        if (!isNameOK)
        {
            if (isDefaultName)
                EditorGUILayout.HelpBox(defaultPrefabName, MessageType.Error);
            //EditorGUILayout.LabelField(defaultPrefabName);

            if (hasSameNamePrefabAlready)
                EditorGUILayout.HelpBox($"Bad prefab name: prefab ({path}) already exists)", MessageType.Error);
            // EditorGUILayout.LabelField($"!!!Bad prefab name: empty: {isEmpty}, name=gameobject: {isDefaultName}, prefab already exists: {hasSameNamePrefabAlready}");
        }

        if (isNameOK && Button("CREATE prefab!"))
        {
            PrefabUtility.SaveAsPrefabAssetAndConnect(PossiblePrefab, path, InteractionMode.UserAction, out var success);

            Debug.Log("Prefab saving " + success);

            if (success)
            {
                isDraggedGameObjectMode = false;
                HandleDraggedPrefab(PossiblePrefab);
            }
        }

        Space();
        if (Button("Go Back"))
        {
            isDraggedGameObjectMode = false;
            isDraggedPrefabMode = false;
        }
    }

    void RenderAddingNewRouteFromDraggedPrefab()
    {
        Space();
        GUILayout.Label("Add DRAGGED prefab", EditorStyles.boldLabel);

        draggedName = EditorGUILayout.TextField("Name", draggedName);
        draggedUrl = EditorGUILayout.TextField("Url", draggedUrl);

        var dataCorrect = draggedUrl.Length > 0 && draggedName.Length > 0;

        if (dataCorrect && GUILayout.Button("Add DRAGGED prefab!"))
        {
            Space();

            draggedUrl = GetValidatedUrl(draggedUrl);

            AddAsset(draggedUrl, draggedPath, draggedName);

            isDraggedPrefabMode = false;

            SaveData();

            Debug.Log("Added DRAGGED prefab");

            DestroyImmediate(PossiblePrefab);

            PossiblePrefab = null;

            Debug.Log("Removed object too");
        }
    }

    void HandleDraggedPrefab(GameObject go)
    {
        isDraggedPrefabMode = true;
        PossiblePrefab = go;

        var parent = PrefabUtility.GetCorrespondingObjectFromSource(go);
        string prefabPath = AssetDatabase.GetAssetPath(parent);

        Debug.Log("route = " + prefabPath);

        // try to attach this prefab
        // to current prefab

        draggedName = GetPrettyNameFromAssetPath(prefabPath);
        draggedPath = prefabPath;
        draggedUrl = newUrl.TrimEnd('/') + "/" + draggedName.TrimStart('/');
    }

    void HandleDraggedScene(string path)
    {
        draggedName = GetPrettyNameFromAssetPath(path);
        draggedPath = path;
        draggedUrl = newUrl.TrimEnd('/') + "/" + draggedName.TrimStart('/');
    }

    void HandleDraggedGameObject(GameObject go)
    {
        isDraggedGameObjectMode = true;

        possiblePrefabName = go.name;

        draggedName = go.name;
        draggedPath = "";
        draggedUrl = newUrl.TrimEnd('/') + "/" + draggedName.TrimStart('/'); // draggedUrl = newUrl + "/" + draggedName;

        PossiblePrefab = go;
    }
}


// Save/Load info
public partial class SimpleUI
{
    static List<SimpleUISceneType> _prefabs;
    static Dictionary<string, List<UrlOpeningAttempt>> UrlOpeningAttempts;

    public static List<SimpleUISceneType> prefabs
    {
        get
        {
            if (_prefabs == null || _prefabs.Count == 0)
            {
                LoadData();
            }

            return _prefabs;
        }
    }

    public static bool IsAssetPathExists(string path)
    {
        return prefabs.Any(p => p.AssetPath.Equals(path));
    }

    public static bool IsUrlExist(string url)
    {
        return prefabs.Any(p => p.Url.Equals(url));
    }
    // = new List<NewSceneTypeBlah>();

    static void SaveData()
    {
        var fileName = "SimpleUI/SimpleUI.txt";
        var fileName2 = "SimpleUI/SimpleUI-MissingUrls.txt";

        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
        serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
        serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
        serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

        var entityData = _prefabs;
        //var entityData = prefabs; // new Dictionary<int, IComponent[]>();

        using (StreamWriter sw = new StreamWriter(fileName))
        using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
        {
            if (entityData.Count > 0)
            {
                serializer.Serialize(writer, entityData);
            }
        }

        var data = UrlOpeningAttempts;

        using (StreamWriter sw = new StreamWriter(fileName2))
        using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
        {
            if (data.Count() > 0)
            {
                serializer.Serialize(writer, data);
            }
        }
    }

    static void LoadData()
    {
        //if (prefabs != null && prefabs.Count == 0)
        //    return;

        var fileName = "SimpleUI/SimpleUI.txt";
        var missingUrls = "SimpleUI/SimpleUI-MissingUrls.txt";

        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
        };

        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SimpleUISceneType>>(File.ReadAllText(fileName), settings);
        var obj2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<UrlOpeningAttempt>>>(File.ReadAllText(missingUrls), settings);

        _prefabs = obj ?? new List<SimpleUISceneType>();
        UrlOpeningAttempts = obj2 ?? new Dictionary<string, List<UrlOpeningAttempt>>();
    }

    static void UpdatePrefab(SimpleUISceneType prefab) => UpdatePrefab(prefab, ChosenIndex);
    public static void UpdatePrefab(SimpleUISceneType prefab, int index)
    {
        if (!hasChosenPrefab)
            return;

        prefabs[index] = prefab;
        SaveData();
    }
}

// Scripts, attached to prefab
public partial class SimpleUI
{
    void RenderScriptsAttachedToThisPrefab(SimpleUISceneType p)
    {
        var GO = Selection.activeObject as GameObject;
        var scripts = new Dictionary<string, int>();

        RenderScriptsAttachedToThisGameObject(GO.transform, ref scripts);

        Debug.Log("Scripts, attached to PREFAB");

        foreach (var s in scripts)
            Debug.Log(s.Key);
    }

    void RenderScriptsAttachedToThisGameObject(Transform GO, ref Dictionary<string, int> scripts)
    {
        foreach (Transform go in GO.transform)
        {
            foreach (Component c in go.GetComponents<Component>())
            {
                string name = ObjectNames.GetInspectorTitle(c);
                if (name.EndsWith("(Script)"))
                {
                    string formated = name.Replace("(Script)", String.Empty).Replace(" ", String.Empty) + ".cs";
                    scripts[formated] = 1;
                }
            }

            RenderScriptsAttachedToThisGameObject(go, ref scripts);
        }
    }
}


// what uses component OpenUrl
public partial class SimpleUI : EditorWindow
{
    public static bool HasNoPrefabsBetweenObjects(MonoBehaviour component, GameObject root)
    {
        // is directly attached to root prefab object with no in between prefabs

        // root GO
        // -> NonPrefab1 GO
        // -> NonPrefab2 GO
        // -> -> NonPrefab3 GO with our component
        // returns true

        // -> NonPrefab1 GO
        // -> Prefab2
        // -> -> NonPrefab3 Go with our component
        // returns false

        // PrefabUtility.IsAnyPrefabInstanceRoot(component.gameObject);
        // PrefabUtility.IsOutermostPrefabInstanceRoot(component.gameObject);
        // PrefabUtility.IsPartOfAnyPrefab(component.gameObject);
        // PrefabUtility.IsPartOfPrefabAsset(component.gameObject);
        // PrefabUtility.IsPartOfPrefabInstance(component.gameObject);
        // PrefabUtility.IsPartOfRegularPrefab(component.gameObject);
        // PrefabUtility.IsPartOfNonAssetPrefabInstance(component.gameObject);

        //var nearestRoot = PrefabUtility.GetNearestPrefabInstanceRoot(component);
        //var outerRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(component);

        //var rootId = root.GetInstanceID();

        //var nearestId = nearestRoot.GetInstanceID();
        //var outerId = outerRoot.GetInstanceID();

        //var result = nearestId == outerId;

        //// Debug.Log($"isDirectly attached to root prefab? <b>{result}</b> c={component.gameObject.name}, root={root.gameObject.name}\n" 
        ////           + $"\n<b>nearest prefab ({nearestId}): </b>" + nearestRoot.name 
        ////           + $"\n<b>outer prefab ({outerId}): </b>" + outerRoot.name 
        ////           + $"\n\nroot instance ID={rootId}");

        //return result;

        var rootOf = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(root);
        var pathOfComponent = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(component);

        return rootOf.Equals(pathOfComponent);
    }

    public static bool IsAddedAsOverridenComponent(MonoBehaviour component)
    {
        var outermost = PrefabUtility.GetOutermostPrefabInstanceRoot(component);

        //// return !PrefabUtility.GetAddedComponents(outermost).Any(ac => ac.GetType() == component.GetType() && component.GetInstanceID() == ac.instanceComponent.GetInstanceID());

        //return PrefabUtility.IsAddedComponentOverride(component);

        return PrefabUtility.GetAddedComponents(outermost).Any(ac => component.GetInstanceID() == ac.instanceComponent.GetInstanceID());
    }


    public static bool IsRootOverridenProperties2(MonoBehaviour component, GameObject root, string[] properties)
    {
        var outermost = PrefabUtility.GetOutermostPrefabInstanceRoot(component);

        // var propertyOverrides = PrefabUtility.GetPropertyModifications(outermost)
        var objectOverrides = PrefabUtility.GetObjectOverrides(outermost)
            // .Where(modification => modification.instanceObject.GetType() == typeof(OpenUrl))
            .Where(modification => modification.instanceObject.GetInstanceID() == component.GetInstanceID());
        // .Where(modification => modification.target.GetType() == typeof(OpenUrl));
        // modification.target.GetInstanceID() == component.GetInstanceID() &&
        // properties.Contains(modification.propertyPath)
        // );

        var propsFormatted = string.Join("\n", objectOverrides.Select(modification => modification.instanceObject.GetType()));
        // Print("IsRoot overriding properties: " + propsFormatted);

        return objectOverrides.Any();
    }

    public static bool HasOverridenProperties(MonoBehaviour component, string[] properties)
    {
        var result = PrefabUtility.HasPrefabInstanceAnyOverrides(component.gameObject, false);


        var overrides = PrefabUtility.GetObjectOverrides(component.gameObject);

        // var wat = overrides.First().coupledOverride.GetAssetObject();
        // Debug.Log("first override " + wat);

        var nearestPrefab = PrefabUtility.GetCorrespondingObjectFromSource(component);

        // Debug.Log($"Check overrides for component {component.gameObject.name}");

        foreach (var modification in PrefabUtility.GetPropertyModifications(component))
        {
            if (modification.propertyPath.Contains("m_"))
                continue;

            if (properties.Contains(modification.propertyPath))
                return true;
            // Debug.Log($"Property: {modification.propertyPath}");
            // Debug.Log($"Value: {modification.value}");
            // Debug.Log(modification.target);
        }

        //Debug.Log("Corresponding object for " + component.gameObject.name + " is " + nearestPrefab.name);

        //var str = result ? "HAS" : "Has NO";

        //// PrintArbitraryInfo(null, $"{component.gameObject.name} {str} overrides"); // ({root.gameObject.name})

        //return result;

        return false;
    }

    static string ParseAddedComponents(GameObject parent)
    {
        var addedComponents = PrefabUtility.GetAddedComponents(parent);

        var formattedAddedComponents = addedComponents.Where(FilterNecessaryComponents)
            .Select(ac => ac.instanceComponent.GetType() + " " + ac.instanceComponent.GetInstanceID() + " " + ac.instanceComponent.gameObject.GetInstanceID());

        return string.Join(", ", formattedAddedComponents);
    }

    private static bool FilterNecessaryComponents(AddedComponent arg)
    {
        return arg.instanceComponent.GetType() == typeof(OpenUrl);
    }

    // path - RootAssetPath
    public static PrefabMatchInfo GetPrefabMatchInfo2(MonoBehaviour component, GameObject asset, string path, string[] properties)
    {
        var matchingComponent = new PrefabMatchInfo { PrefabAssetPath = path, ComponentName = component.gameObject.name };
        matchingComponent.URL = (component as OpenUrl).Url;
        matchingComponent.InstanceID = component.gameObject.GetInstanceID();


        //component.gameObject.transform.GetSiblingIndex();

        bool isAttachedToRootPrefab = HasNoPrefabsBetweenObjects(component, asset);
        bool isAttachedToNestedPrefab = !isAttachedToRootPrefab;

        if (isAttachedToRootPrefab)
        {
            // directly appears in prefab
            // so you can upgrade it and safely save ur prefab

            matchingComponent.IsDirectMatch = true;
            Print2($"Directly occurs as {matchingComponent.ComponentName} in {matchingComponent.PrefabAssetPath} {matchingComponent.URL}");
        }

        if (isAttachedToNestedPrefab)
        {
            bool isAddedByRoot = IsAddedAsOverridenComponent(component);
            bool isPartOfNestedPrefab = !isAddedByRoot;

            if (isAddedByRoot)
            {
                // added, but not saved in that prefab
                // so prefab will not see this component in itself

                // you need to update URL of this component here, but don't accidentally apply changes to prefab, which this component sits on
                // you can safely save changes in root prefab as well
                var outer = PrefabUtility.GetOutermostPrefabInstanceRoot(component);

                matchingComponent.IsOverridenAsAddedComponent = true;
                Print2(
                    $"{matchingComponent.ComponentName} Is <b>ATTACHED</b> to some nested prefab\n\nouter={outer.name} {ParseAddedComponents(outer)}, {component.GetInstanceID()}");
            }

            if (isPartOfNestedPrefab)
            {
                // so you might need to check Overriden Properties

                if (IsRootOverridenProperties2(component, asset, properties))
                {
                    // update property and just save root prefab

                    matchingComponent.IsOverridenAsComponentProperty = true;
                    Print2($"Root <b>OVERRIDES VALUES</b> on {matchingComponent.ComponentName}");
                }
                else
                {
                    // you will upgrade value in it's own prefab
                    // no actions are necessary for root prefab

                    matchingComponent.IsNormalPartOfNestedPrefab = true;
                    Print2($"{matchingComponent.ComponentName} is <b>part of a nested prefab</b>");
                }
            }
        }

        return matchingComponent;
    }

    public static void PrintMatchInfo(IEnumerable<PrefabMatchInfo> matches)
    {
        foreach (var matchingComponent in matches)
        {
            if (matchingComponent.IsDirectMatch)
            {
                // directly appears in prefab
                // so you can upgrade it and safely save ur prefab

                Print($"Directly occurs as {matchingComponent.ComponentName} in {matchingComponent.PrefabAssetPath}");
            }
            else
            {
                // appears somewhere in nested prefabs

                if (matchingComponent.IsOverridenAsAddedComponent)
                {
                    // is added by root component

                    Print($"{matchingComponent.ComponentName} Is <b>ATTACHED BY ROOT</b> to some nested prefab\n");
                }
                else
                {
                    // is part of nested prefab
                    if (matchingComponent.IsOverridenAsComponentProperty)
                    {
                        Print($"Root <b>OVERRIDES VALUES</b> on {matchingComponent.ComponentName}");
                    }

                    if (matchingComponent.IsNormalPartOfNestedPrefab)
                    {
                        Print($"{matchingComponent.ComponentName} is <b>part of a nested prefab</b>");
                    }
                }
            }

            //Print("linkTo=" + matchingComponent.URL);
        }
    }

    public class PrefabMatchInfo
    {
        public string PrefabAssetPath;
        public string ComponentName;
        public string URL;
        public int InstanceID;

        public bool IsDirectMatch; // with no nested prefabs, can apply changes directly. (Both on root and it's childs)
        public bool IsNormalPartOfNestedPrefab; // absolutely normal prefab part with NO overrides. No actions required

        //public bool IsOverridenPartOfNestedPrefab =>
        //    IsOverridenAsAddedComponent ||
        //    IsOverridenAsComponentProperty; // is overriden somehow: maybe there is not saved Added component or Overriden Parameters in component itself

        public bool IsOverridenAsComponentProperty;
        public bool IsOverridenAsAddedComponent;
    }

    public static List<PrefabMatchInfo> WhatUsesComponent(string url, List<PrefabMatchInfo> matchInfos)
    {
        return matchInfos.Where(m => m.URL.Equals(url.TrimStart('/'))).ToList();
    }
    //public static List<PrefabMatchInfo> WhatUsesComponent(string url)
    //{
    //    var matches = WhatUsesComponent<OpenUrl>();

    //    return WhatUsesComponent(url, matches);
    //        //.Where(a => a.URL.Equals(url.TrimStart('/')))
    //        //.ToList();
    //}

    /// <summary>
    /// Only works if T is OpenUrl
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<PrefabMatchInfo> WhatUsesComponent<T>()
    {
        var typeToSearch = typeof(T);

        Debug.Log("Finding all Prefabs and scenes that have the component" + typeToSearch + "…");

        var excludeFolders = new[] { "Assets/Standard Assets" };
        //var guids = AssetDatabase.FindAssets("t:prefab", new[] { "Assets" });
        var guids = AssetDatabase.FindAssets("t:scene t:prefab", new[] { "Assets" });

        var paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToList();
        paths.RemoveAll(guid => excludeFolders.Any(guid.Contains));

        List<PrefabMatchInfo> matchingComponents = new List<PrefabMatchInfo>();

        var properties = new[] { "Url" };

        // save state
        bool isPrefabWasOpened = isPrefabMode;

        var activeScene = EditorSceneManager.GetActiveScene();

        var sceneCount = EditorSceneManager.sceneCount;

        List<Scene> scenes = new List<Scene>();

        //for (var i = 0; i < sceneCount; i++)
        //{
        //    scenes.Add(EditorSceneManager.GetSceneAt(i));
        //    Debug.Log("Loaded scene: " + scenes[i].name);
        //}

        foreach (var path in paths)
        {
            if (isPrefabAsset(path))
            {
                GetMatchingComponentsFromPrefab<T>(matchingComponents, path, typeToSearch, properties);
            }
            else
            {
                GetMatchingComponentsFromScene<T>(matchingComponents, path, typeToSearch, properties, scenes);
            }
        }

        // restore state if scenes were opened

        // restore scene
        //EditorSceneManager.SetActiveScene(activeScene);

        // restore prefab if was open

        return matchingComponents;
    }

    static void GetMatchingComponentsFromScene<T>(List<PrefabMatchInfo> matchingComponents, string path, Type typeToSearch, string[] properties, List<Scene> openedScenes)
    {
        // https://stackoverflow.com/questions/54452347/can-i-programatically-load-scenes-in-the-unity-editor

        var asset1 = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);

        if (asset1 == null)
        {
            Debug.LogError("Cannot load SCENE at path: " + path);
            return;
        }

        //var scene = EditorSceneManager.GetSceneByPath(path);

        bool isOpenedAlready = openedScenes.Any(s => s.path.Equals(path));
        var scene = isOpenedAlready ? openedScenes.First(s => s.path.Equals(path)) : EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);

        //if (!scene.isLoaded)
        //{
        //    Debug.LogError("Scene not loaded: " + path);

        //    return;
        //}

        //if (!scene.IsValid())
        //{
        //    Debug.LogError(asset1);
        //    Debug.LogError(asset1.name);
        //    Debug.LogError("Scene " + path + " is invalid");

        //    return;
        //}

        foreach (var asset in scene.GetRootGameObjects())
        {
            List<T> components = asset.GetComponentsInChildren<T>(true).ToList();

            if (components.Any())
            {
                Print2("<b>----------------------------------------</b>");
                Print2("SCENE: Found component(s) " + typeToSearch + $" ({components.Count}) in file <b>" + path + "</b>");
            }

            foreach (var component1 in components)
            {
                var component = component1 as MonoBehaviour;

                var matchingComponent = GetPrefabMatchInfo2(component, asset, path, properties);

                matchingComponents.Add(matchingComponent);
            }
        }

        if (!isOpenedAlready)
            EditorSceneManager.CloseScene(scene, true);
    }

    static void GetMatchingComponentsFromPrefab<T>(List<PrefabMatchInfo> matchingComponents, string path, Type typeToSearch, string[] properties)
    {
        var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

        if (asset == null)
        {
            Debug.LogError("Cannot load prefab at path: " + path);
            return;
        }

        List<T> components = asset.GetComponentsInChildren<T>(true).ToList();
        var self = asset.GetComponent<T>();

        if (self != null)
        {
            components.Add(self);
        }

        if (components.Any())
        {
            Print2("<b>----------------------------------------</b>");
            Print2("PREFAB: Found component(s) " + typeToSearch + $" ({components.Count}) in file <b>" + path + "</b>");
        }

        foreach (var component1 in components)
        {
            var component = component1 as MonoBehaviour;

            var matchingComponent = GetPrefabMatchInfo2(component, asset, path, properties);

            matchingComponents.Add(matchingComponent);
        }
    }

    public struct UsageInfo
    {
        public string ScriptName;
        public int Line;
    }

    public static List<UsageInfo> WhichScriptReferencesConcreteUrl(string url)
    {
        var directory = "Assets/";
        var list = new List<UsageInfo>();

        Debug.Log("Finding all scrips, that call " + url);


        var excludeFolders = new[] { "Assets/Standard Assets/Frost UI", "Assets/Standard Assets/SimpleUI", "Assets/Standard Assets/Libraries", "Assets/Systems", "Assets/Core" };
        var guids = AssetDatabase.FindAssets("t:Script", new[] { "Assets" });

        Debug.Log($"Found {guids.Count()} scripts");
        var paths = guids.Select(AssetDatabase.GUIDToAssetPath).ToList();

        paths.RemoveAll(guid => excludeFolders.Any(guid.Contains));

        bool directMatch = true;
        var searchString = '"' + url;

        if (directMatch)
            searchString += '"';

        foreach (var path in paths)
        {
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

            var txt = asset != null ? ("\n" + asset.text) : "";

            //Debug.Log("found script: " + path + txt);

            if (asset == null)
            {
                Debug.LogError("Cannot load prefab at path: " + path);

                continue;
            }


            if (txt.Contains(searchString))
            {
                Debug.Log($"Found url {url} in text " + path);

                list.Add(new UsageInfo { Line = 1, ScriptName = path });
            }
        }

        return list;
    }


    public static bool IsRootOverridenProperties(MonoBehaviour component, GameObject root, string[] properties)
    {
        var fastFilter = new Func<PropertyModification, bool>(p => properties.Contains(p.propertyPath));
        var print = new Func<IEnumerable<PropertyModification>, string>(p => string.Join(", ", p.Select(pp => pp.propertyPath).ToList()));

        var outermost = PrefabUtility.GetOutermostPrefabInstanceRoot(component);
        var outermostPropertyChanges = PrefabUtility.GetPropertyModifications(outermost).Where(fastFilter);

        var outermostPath = AssetDatabase.GetAssetPath(outermost);
        var outermostAsset = AssetDatabase.LoadMainAssetAtPath(outermostPath);

        // var objectOverrides = PrefabUtility.GetObjectOverrides(component.gameObject).Where(change => change.instanceObject.GetType() == typeof(OpenUrl));
        // var propertyChanges = PrefabUtility.GetPropertyModifications(component.gameObject).Where(p => !p.propertyPath.Contains("m_") && properties.Contains(p.propertyPath));
        // PrefabUtility.HasPrefabInstanceAnyOverrides()
        var propertyChanges = PrefabUtility.GetPropertyModifications(component.gameObject).Where(fastFilter);

        var text = $"Outermost changes {outermost.name} hasAnyChanges={PrefabUtility.HasPrefabInstanceAnyOverrides(outermost, false)}, {outermostPath}\n";
        text += print(outermostPropertyChanges);
        text += "\nComponent changes\n";
        text += print(propertyChanges);

        Debug.Log(text);

        return propertyChanges.Any();
    }

    public static PrefabMatchInfo GetPrefabMatchInfo(MonoBehaviour component, GameObject root, string path, string[] matchingProperties)
    {
        var matchInfo = new PrefabMatchInfo { PrefabAssetPath = path, ComponentName = component.gameObject.name };
        return matchInfo;
        string text;


        var objectOverrides = PrefabUtility.GetObjectOverrides(component.gameObject)
            .Where(change => change.instanceObject.GetType() == typeof(OpenUrl));
        var propertyChanges = PrefabUtility.GetPropertyModifications(component.gameObject)
            .Where(p => !p.propertyPath.Contains("m_"));

        var parent = PrefabUtility.GetCorrespondingObjectFromSource(component.gameObject);
        var nearest = PrefabUtility.GetNearestPrefabInstanceRoot(component);
        var outermost = PrefabUtility.GetOutermostPrefabInstanceRoot(component);

        var selfAddedComponents = ParseAddedComponents(component.gameObject);
        var parentAddedComponents = ParseAddedComponents(parent);
        var nearestAddedComponents = ParseAddedComponents(nearest);
        var outermostAddedComponents = ParseAddedComponents(outermost);

        var urlChanges = propertyChanges
                // .Where(p => p.target.GetInstanceID() == c.GetInstanceID())
                .Where(p => matchingProperties.Contains(p.propertyPath))
            // .Where(p => p.target.GetType() == typeof(OpenUrl))
            ;

        if (urlChanges.Any())
            text = $"<b>HAS</b> {urlChanges.Count()} URL overrides of {component.gameObject.name} in {root.name}";
        else
            text =
                $"<b>NO</b> url overrides of {component.gameObject.name} in {root.name}, while propertyChanges={propertyChanges.Count()}";

        var concatObjectOverrides = string.Join(", \n",
            objectOverrides.Select(change =>
                (change.instanceObject.name + " (" + change.instanceObject.name + ")")));

        text += "\n\n" + $"({objectOverrides.Count()}) Object Overrides on: {component.gameObject.name}" + "\n\n" +
                concatObjectOverrides;

        text += $"\n\nAdded Components self={component.gameObject}\n({selfAddedComponents})";
        text += $"\n\nAdded Components parent={parent}\n({parentAddedComponents})";
        text += $"\n\nAdded Components nearest={nearest}\n({nearestAddedComponents})";
        text += $"\n\nAdded Components outermost={outermost}\n({outermostAddedComponents})";

        Debug.Log(text);

        // PrintBlah(null, $"<b>NO</b> url overrides of {component.gameObject.name} in {root.name}. propertyChanges={urlChanges.Count()} hasOverrides=<b>{hasOverrides}</b>");


        // var c = component.gameObject;
        // var routeToRoot = new List<GameObject>();
        //
        // routeToRoot.Add(c);
        //
        // int counter = 0;
        // while (c.transform.parent != null && counter < 10)
        // {
        //     c = c.transform.parent.gameObject; 
        //     
        //     routeToRoot.Add(c);
        //
        //     counter++;
        // }
        //
        // if (counter == 10)
        // {
        //     PrintBlah(null, "<B>OVERFLOW</B>");
        // }
        // else
        // {
        //     routeToRoot.Reverse();
        //     foreach (var o in routeToRoot)
        //     {
        //         bool isRoot = root.GetInstanceID() == o.GetInstanceID();
        //         bool isPrefabSelf = PrefabUtility.IsAnyPrefabInstanceRoot(o);
        //         bool isPrefabVariantSelf = PrefabUtility.IsPartOfVariantPrefab(o);
        //
        //         var propertyChanges = PrefabUtility.GetPropertyModifications(o).ToList()
        //             .Where(p => !p.propertyPath.Contains("m_"))
        //             .Where(p => properties.Contains(p.propertyPath))
        //             .Where(p => p.target.GetType() == typeof(OpenUrl));
        //
        //         bool hasOverrides = false;
        //         
        //         PrintBlah(null, $"{o.name} - {o.GetInstanceID()}. isRoot={isRoot}, isPrefab={isPrefabSelf}, hasOverrides={hasOverrides}");
        //     }
        // }

        return matchInfo;
    }
}