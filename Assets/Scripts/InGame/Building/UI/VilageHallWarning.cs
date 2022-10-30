using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilageHallWarning : Warning
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
            WarningUI.SetActive(false);
        });
    }
}
