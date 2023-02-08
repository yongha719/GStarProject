using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VillageHallWarning : Warning
{
    private VillageHall VillageHall;

    private void Start()
    {
        VillageHall = FindObjectOfType<VillageHall>();

        YesButton.onClick.AddListener(() =>
        {
            print("clicked yes button");
            VillageHall.LevelUp();
        });

        NoButton.onClick.AddListener(() =>
        {
            WarningUISetActive(false);
        });
    }

    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
