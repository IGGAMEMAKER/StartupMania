﻿using Assets.Core;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RenderFullAudiencesListView : ListView
{
    int segmentId = 0;
    public Text AudienceDescription;
    public Text PositionongDescription;
    public Text CompaniesInterestedInUsers;

    public override void SetItem<T>(Transform t, T entity)
    {
        t.GetComponent<AudiencePreview>().SetEntity((AudienceInfo)(object)entity, Flagship);
    }

    public override void ViewRender()
    {
        base.ViewRender();

        var infos = Marketing.GetAudienceInfos();
        //.OrderBy(a => a.Size);

        // TODO DUPLICATED FROM AudiencesOnMainScreenListView.ViewRender()
        var audiences = Marketing.GetAudienceInfos();

        //bool showAudiences = true;
        var company = Flagship;
        bool showAudiences = company.isRelease;

        if (showAudiences)
        {
            SetItems(audiences);
        }
        else
        {
            // take primary audience only
            SetItems(audiences.Where(a => a.ID == Marketing.GetCoreAudienceId(company)));
        }

        //SetItems(infos);

        var audience = Marketing.GetAudienceInfos()[segmentId];
        var segmentName = audience.Name;

        var potentialPhrase = $"{Format.Minify(audience.Size)} users";
        var incomePerUser = (double)Economy.GetBaseIncomeByMonetizationType(company); // 1L * (segmentId + 1);
        var worth = (long)(incomePerUser * audience.Size);

        var worthPhrase = Format.Money(worth, true);

        AudienceDescription.text = segmentName + $"\n\n<size=30>Potential\n{Visuals.Positive(potentialPhrase)}\n\nIncome\n{Visuals.Positive(worthPhrase)}</size>";
        PositionongDescription.text = $"We are making {Marketing.GetPositioningName(company)}";

        if (CompaniesInterestedInUsers != null)
        {
            //CompaniesInterestedInUsers.text = $"which are interested in {segmentName}";
            CompaniesInterestedInUsers.text = $"which are interested in {Marketing.GetPositioningName(company)}";
        }

        //FindObjectOfType<CompaniesFocusingSpecificSegmentListView>().SetSegment(segmentId);
    }

    private void OnEnable()
    {
        //segmentId = Flagship.productTargetAudience.SegmentId;
        segmentId = Marketing.GetCoreAudienceId(Flagship); // Mathf.Clamp(Flagship.productPositioning.Positioning, 0, Marketing.GetAudienceInfos().Count); // Marketing.GetAudienceInfos().Where(a => a.ID == ).First().ID;

        ViewRender();
    }

    public override void OnItemSelected(int ind)
    {
        base.OnItemSelected(ind);

        segmentId = Item.GetComponent<AudiencePreview>().segmentId;

        ViewRender();
    }
}
