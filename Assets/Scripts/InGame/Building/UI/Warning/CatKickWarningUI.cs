using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatKickWarningUI : WarningUI
{
    const string CAT_KICK = "을(를) 내보내시겠습니까?";

    public void SetYesButton(Action call)
    {
        YesButton.onClick.AddListener(() => call());
    }

    public void SetCatName(string CatName)
    {
        WarningText.text = CatName + CAT_KICK;
    }
}
