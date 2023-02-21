using UnityEngine;

public class WinterClothes : GoldProductionBuilding
{
    // 제봉틀
    [SerializeField] private GameObject Filature;
    // 실뭉치
    [SerializeField] public GameObject BallOfString;

    // 바꾸자
    public override void ChangeCat(CatData catData, int index)
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
