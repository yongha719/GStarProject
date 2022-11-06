using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatPlacementWarning : Warning
{
    CatPlacement CatPlacement;

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
}
