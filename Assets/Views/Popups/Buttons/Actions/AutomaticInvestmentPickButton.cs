﻿using Assets.Core;

public class AutomaticInvestmentPickButton : PopupButtonController<PopupMessageBankruptcyThreat>
{
    public override void Execute()
    {
        MyCompany.isAutomaticInvestments = true;

        //while (Economy.IsWillBecomeBankruptOnNextPeriod(Q, MyCompany) && !Economy.IsHasCashOverflow(Q, MyCompany))
        //{
        //    SoundManager.PlayFastCashSound();
        //}

        NotificationUtils.ClosePopup(Q);
    }

    public override string GetButtonName()
    {
        return "Raise investments automatically";
    }
}

