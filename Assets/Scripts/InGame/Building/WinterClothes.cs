using UnityEngine;

public class WinterClothes : GoldProductionBuilding
{
    // 제봉
    [SerializeField] private GameObject Filature;

    public override void OnCatMemberChange(CatData catData, int index)
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
