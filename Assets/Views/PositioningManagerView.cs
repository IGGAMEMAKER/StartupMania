﻿using Assets.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PositioningManagerView : View
{
    public ProductPositioning Positioning;

    public Text SegmentDescription;

    public CompaniesFocusingSpecificSegmentListView CompaniesFocusingSpecificSegmentListView;
    public RenderAudienceChoiceListView RenderAudienceChoiceListView;

    public GameObject PivotText;
    public GameObject PivotButton;

    public GameObject ExpandText;
    public GameObject ExpandButton;

    public GameObject ORText;

    List<GameObject> PivotButtons => new List<GameObject> { PivotButton, PivotText, ORText, ExpandButton, ExpandText };
    List<GameObject> PivotDescriptions => new List<GameObject> { PivotText, ExpandText };

    public GameObject PositioningsList;
    public GameObject PositioningDescriptionTab;

    public AudienceListView HatedAudiences;
    public Text HatedLabel;
    public AudienceListView LovingAudiences;
    public Text LovingLabel;

    List<int> TargetAudiences;

    bool flag = false;

    private void OnEnable()
    {
        var audiences = Marketing.GetAudienceInfos();

        Positioning = Marketing.GetPositioning(Flagship);
        TargetAudiences = audiences.Where(a => Marketing.IsAimingForSpecificAudience(Flagship, a.ID)).Select(a => a.ID).ToList();
        flag = true;

        ShowAll(PivotButtons);
        Hide(PositioningsList);
        Hide(PositioningDescriptionTab);

        ViewRender();
    }

    //public override void ViewRender()
    public void Rndr()
    {
        if (!flag)
            return;

        var company = Flagship;

        RenderAudienceChoiceListView.ViewRender();
        CompaniesFocusingSpecificSegmentListView.SetSegment(Positioning);

        RenderSegmentDescription(company);
    }

    string DescribeAudience(long cost, int i) => Visuals.Negative($"{Marketing.GetAudienceInfos()[i].Name} ({Format.MinifyMoney(cost)})");

    void RenderSegmentDescription(GameEntity company)
    {
        var audiences = Marketing.GetAudienceInfos();
        var worth = Marketing.GetPositioningWorth(company, Positioning);

        var audiencesSelected = Positioning.Loyalties
            .Select((l, i) => new { i, cost = Marketing.GetAudienceWorth(audiences[i]), isLoyal = l >= 0 });

        var lovingAudiences     = audiencesSelected.Where(f => f.isLoyal && !Marketing.IsAimingForSpecificAudience(company, f.i));
        var hatedAudiences      = audiencesSelected.Where(f => !f.isLoyal && Marketing.IsAimingForSpecificAudience(company, f.i));


        HatedAudiences  .SetAudiences(hatedAudiences.Select(a => audiences[a.i]).ToList(), -1);
        LovingAudiences .SetAudiences(lovingAudiences.Select(a => audiences[a.i]).ToList(), 1);

        bool hasNewLovingUsers = lovingAudiences.Count() > 0;
        bool hasNewHatingUsers = hatedAudiences.Count() > 0;

        Draw(HatedLabel, hasNewHatingUsers);
        Draw(HatedAudiences, hasNewHatingUsers);

        Draw(LovingLabel, hasNewLovingUsers);
        Draw(LovingAudiences, hasNewLovingUsers);

        HatedLabel.text = $"<size=50><color=red><b>!!! You will LOSE these users</b></color></size>";

        if (hasNewHatingUsers)
            LovingLabel.text = $"Instead, you will <b><color=green>GET</color></b>";
        else
            LovingLabel.text = $"You will <b><color=green>GET</color></b>";
    }

    void ShowTabs()
    {
        Show(PositioningsList);
        //Show(PositioningDescriptionTab);
    }

    void HideButtonDescriptions()
    {
        HideAll(PivotDescriptions);
    }

    public void OnExpandButton()
    {
        HideButtonDescriptions();
        ShowTabs();

        FindObjectOfType<RenderAudienceChoiceListView>().SetPositionings(new List<int> { 0, 1, 2, 3 });
        Show(PositioningDescriptionTab);
    }

    public void OnPivotButton()
    {
        HideButtonDescriptions();
        ShowTabs();

        FindObjectOfType<RenderAudienceChoiceListView>().SetPositionings(new List<int> { 0, 1 });
        Show(PositioningDescriptionTab);
    }

    public void SetAnotherPositioning(ProductPositioning positioning)
    {
        Positioning = positioning;
        flag = true;

        //ViewRender();
        CompaniesFocusingSpecificSegmentListView.SetSegment(Positioning);

        RenderSegmentDescription(Flagship);
    }

    public void AddAudience(int segmentId)
    {
        if (!TargetAudiences.Contains(segmentId))
        {
            TargetAudiences.Add(segmentId);
        }
    }

    public void RemoveAudience(int segmentId)
    {
        if (TargetAudiences.Contains(segmentId))
        {
            TargetAudiences.RemoveAll(id => id == segmentId);
        }
    }


    void RenderSegmentDescription2(GameEntity company)
    {
        var audiences = Marketing.GetAudienceInfos();
        var worth = Marketing.GetPositioningWorth(company, Positioning);

        var a = Positioning.Loyalties
            .Select((l, i) => new { i, cost = Marketing.GetAudienceWorth(audiences[i]), isLoyal = l >= 0 });

        var favoriteAudiences = a.Where(f => f.isLoyal);
        var hatedAudiences = a.Where(f => !f.isLoyal);

        var favoriteAudiencesDescription = string.Join("\n", favoriteAudiences.Select(f => DescribeAudience(f.cost, f.i)));
        var hatedAudiencesDescription = string.Join("\n", hatedAudiences.Select(f => DescribeAudience(f.cost, f.i)));

        SegmentDescription.text = $"{Positioning.name}\n\n<b>Potential Income</b>\n{Visuals.Positive(Format.MinifyMoney(worth))}\n\n<b>Suits</b>\n{favoriteAudiencesDescription}\n\n<b>Hate</b>\n{hatedAudiencesDescription}\n\n";
    }
}
