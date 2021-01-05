﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SimpleUIEventHandler : MonoBehaviour
{
    // string - url
    // GameObject - prefab
    public Dictionary<string, GameObject> Objects = new Dictionary<string, GameObject>();

    public string CurrentUrl;
    static List<SimpleUISceneType> prefabs; // = new List<NewSceneTypeBlah>();

    private static int counter = 0;
    private bool canRenderStuff = true;

    void Start()
    {
        LoadData();

        // foreach (var prefab in prefabs)
        // {
        //     RenderPrefab(prefab.Url);
        //     // HidePrefab(prefab.Url);
        // }
    }

    public void OpenTab(string url)
    {
        var trimmedUrl = url.StartsWith("/") ? url.TrimStart('/') : url; 
        
        OpenUrl(CurrentUrl + "/" + trimmedUrl);
    }

    List<string> ParseUrlToSubRoutes(string url)
    {
        var urls = new List<string>();
        
        var cUrl = "/";
        
        // hide opened stuff
        foreach (var subPath in url.Split('/'))
        {
            if (subPath.StartsWith("/") || cUrl.EndsWith("/"))
                cUrl += subPath;
            else
                cUrl += "/" + subPath;
            
            urls.Add(cUrl);
        }

        return urls;
    }

    void PrintParsedRoute(List<string> urls, string label)
    {
        Debug.Log(label + ": " + string.Join("\n", urls));
    }
    
    public void OpenUrl(string url)
    {
        if (url.Equals(CurrentUrl))
            return;
        
        LoadData();
        
        Debug.Log($"SHIFTING FROM {CurrentUrl}=>{url}");

        var newUrls = ParseUrlToSubRoutes(url);
        var oldUrls = ParseUrlToSubRoutes(CurrentUrl);
        var commonUrls = new List<string>();
        
        PrintParsedRoute(oldUrls, "OLD routes");
        PrintParsedRoute(newUrls, "NEW routes");

        foreach (var removableUrl in oldUrls)
        {
            if (newUrls.Contains(removableUrl))
            {
                commonUrls.Add(removableUrl);
            }
        }

        foreach (var commonUrl in commonUrls)
        {
            newUrls.RemoveAll(u => u.Equals(commonUrl));
            oldUrls.RemoveAll(u => u.Equals(commonUrl));
        }
        
        foreach (var removableUrl in oldUrls)
        {
            // HidePrefab(removableUrl);
        }
        
        foreach (var newUrl in newUrls)
        {
            // RenderPrefab(newUrl);
        }
        
        // if attempt overflow, render only necessary stuff
        // if (!canRenderStuff)
            RenderPrefab(url);

        CurrentUrl = url;
    }

    void MeasureAttempts(string url)
    {
        counter++;
        
        if (counter > 100)
            canRenderStuff = false;
    }

    void RenderPrefab(string url)
    {
        Debug.Log("Render prefab by url: " + url);

        MeasureAttempts(url);
        
        var p = GetPrefab(url);
        
        if (p != null)
            p.SetActive(true);
    }

    void HidePrefab(string url)
    {
        Debug.Log("HIDE prefab by url: " + url);

        MeasureAttempts(url);

        var p = GetPrefab(url);
        
        if (p != null)
            p.SetActive(false);
    } 

    GameObject GetPrefab(string url)
    {
        try
        {
            if (!Objects.ContainsKey(url))
            {
                if (!prefabs.Any(p => p.Url.Equals(url)))
                {
                    Debug.LogError("URL " + url + " not found");
                    return null;
                }

                var pre = prefabs.First(p => p.Url.Equals(url));

                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(pre.AssetPath);
                if (obj == null)
                {
                    Debug.LogError("Prefab in route " + pre.AssetPath + " not found");
                    return null;
                }

                // Objects[url] = Instantiate(AssetDatabase.GetMainAssetTypeAtPath(pre.AssetPath));
                Objects[url] = Instantiate(obj, transform);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Objects[url];
    }

    static void LoadData()
    {
        var fileName = "SimpleUI/SimpleUI.txt";

        List<SimpleUISceneType> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SimpleUISceneType>>(
            File.ReadAllText(fileName), new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            });

        prefabs = obj ?? new List<SimpleUISceneType>();
    }
}