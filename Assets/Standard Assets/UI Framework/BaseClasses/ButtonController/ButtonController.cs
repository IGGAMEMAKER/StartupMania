﻿using Assets.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Button))]
public abstract partial class ButtonController : BaseClass
{
    Button Button;
    [Tooltip("Sets the color of link")]
    public bool IsLink = false;

    public abstract void Execute();
    public virtual void ButtonStart() { }

    void ExecuteAndUpdateScreen()
    {
        Execute();

        UpdatePage();
    }

    // start
    public virtual void Initialize()
    {
        Button = GetComponentInChildren<Button>() ?? GetComponent<Button>();

        Button.onClick.AddListener(ExecuteAndUpdateScreen);

        ButtonStart();

        if (IsLink)
        {
            var text = GetComponentInChildren<TextMeshProUGUI>();

            var linkColor = Visuals.GetColorFromString(Colors.COLOR_LINK);

            if (text != null)
                text.color = linkColor;
            else
                GetComponentInChildren<Text>().color = linkColor;
        }
    }

    /// <summary>
    /// CANNOT OVVERRIDE IN INHERITED CLASSES
    /// </summary>
    void OnEnable()
    {
        Initialize();
    }

    // destroy
    void OnDisable()
    {
        RemoveListener();
    }

    void RemoveListener()
    {
        if (Button)
            Button.onClick.RemoveListener(ExecuteAndUpdateScreen);
        else
            Debug.LogWarning("This component is not assigned to Button. It is assigned to " + gameObject.name);
    }

    public void UpdatePage()
    {
        ScreenUtils.UpdateScreen(Q);
    }
}

// navigation
public abstract partial class ButtonController
{
    public void GoBack()
    {
        ScreenUtils.NavigateBack(Q);
    }

    // navigate
    public void Navigate(ScreenMode screenMode, string field, object data)
    {
        ScreenUtils.Navigate(Q, screenMode, field, data);
    }

    public void Navigate(ScreenMode screenMode)
    {
        ScreenUtils.Navigate(Q, screenMode);
    }

    public void NavigateToTeamScreen(int teamId)
    {
        Navigate(ScreenMode.TeamScreen, C.MENU_SELECTED_TEAM, teamId);
    }

    public void NavigateToMainScreen()
    {
        Navigate(ScreenMode.HoldingScreen);
    }

    public void NavigateToNiche(NicheType niche)
    {
        Navigate(ScreenMode.NicheScreen, C.MENU_SELECTED_NICHE, niche);
    }

    public void NavigateToIndustry(IndustryType industry)
    {
        Navigate(ScreenMode.IndustryScreen, C.MENU_SELECTED_INDUSTRY, industry);
    }

    public void NavigateToCompany(ScreenMode screenMode, int companyId)
    {
        Navigate(screenMode, C.MENU_SELECTED_COMPANY, companyId);
    }

    public void NavigateToHuman(int humanId)
    {
        Navigate(ScreenMode.CharacterScreen, C.MENU_SELECTED_HUMAN, humanId);
    }

    public void NavigateToProjectScreen(int companyId)
    {
        NavigateToCompany(ScreenMode.ProjectScreen, companyId);
    }

    //public void NavigateToMainScreen()
    //{
    //    Navigate(ScreenMode.HoldingScreen);
    //}
}
