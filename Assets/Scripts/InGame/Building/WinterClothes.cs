using UnityEngine;

public class WinterClothes : GoldProductionBuilding
{
    // 제봉
    [SerializeField] private GameObject Filature;

    protected override void OnCatMemberChange(int index)
    {
        if (index == 0)
        {
            Filature.SetActive(false);
        }
        else
        {
            Filature.SetActive(true);
        }
    }
}
