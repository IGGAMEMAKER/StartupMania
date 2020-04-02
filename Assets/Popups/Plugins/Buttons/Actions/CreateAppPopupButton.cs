﻿using Assets.Core;

public class CreateAppPopupButton : PopupButtonController<PopupMessageDoYouWantToCreateApp>
{
    public override void Execute()
    {
        NicheType nicheType = Popup.NicheType;

        var id = Companies.CreateProductAndAttachItToGroup(Q, nicheType, MyCompany);

        NotificationUtils.ClosePopup(Q);
        NotificationUtils.AddPopup(Q, new PopupMessageCreateApp(id));

        // had no products before
        if (Companies.GetDaughterCompaniesAmount(MyCompany, Q) == 1)
        {
            var company = Companies.Get(Q, id);
            company.isFlagship = true;

            //NavigateToNiche(company.product.Niche);
            Navigate(ScreenMode.HoldingScreen, Balance.MENU_SELECTED_NICHE, company.product.Niche);
        }
    }

    public override string GetButtonName() => "YES";
}