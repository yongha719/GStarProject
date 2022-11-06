using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlacementWarning : Warning
{
    CatPlacement CatPlacement;

    private const string WARNING_TEXT = "을(를)\n배치하시겠습니까?";

    void Start()
    {
        CatPlacement = FindObjectOfType<CatPlacement>();

        YesButton.onClick.AddListener(() =>
        {

        });

        NoButton.onClick.AddListener(() =>
        {
            WarningUI.SetActive(false);
        });
    }

    public void OnClickYesButton(Action call) => YesButton.onClick.AddListener(() => call());

    public void SetWaringData(string CatName)
    {
        WarningText.text = CatName + WARNING_TEXT;
    }
}

