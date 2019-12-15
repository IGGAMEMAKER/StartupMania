﻿using Assets.Utils;
using UnityEngine;


public class ClosePopupExitGame : SimplePopupButtonController
{
    public override void Execute()
    {
        NotificationUtils.ClosePopup(GameContext);

        // copied from
        // Exit.cs
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public override string GetButtonName() => "Exit Game";
}
