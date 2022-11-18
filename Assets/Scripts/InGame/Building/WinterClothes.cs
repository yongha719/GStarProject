using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterClothes : GoldProductionBuilding
{
    [SerializeField] private GameObject Filature;

    protected override void OnCatMemberChange(int index)
    {
        if (index == 1)
        {
            Filature.SetActive(true);
        }
        else
        {
            Filature.SetActive(false);

        }
    }
}
