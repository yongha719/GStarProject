using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VillageHallLevelUpWarning : WarningUI
{
    private VillageHall VillageHall;

    private void Start()
    {
        VillageHall = FindObjectOfType<VillageHall>();

        YesButton.onClick.AddListener(() =>
        {
            VillageHall.LevelUp();
        });

        NoButton.onClick.AddListener(() =>
        {
            CloseUIPopup();
        });
    }
}
