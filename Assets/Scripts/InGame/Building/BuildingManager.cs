using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Gold,
    Energy,
}

public enum GoldBuildingType
{
    IceFishing,              // 얼음 낚시터
    GoldMine,                // 금 광산
    FirewoodChopping,        // 장작 패기
    PotatoFarming,           // 감자 농사
    BlastFurnace,            // 용광로
    WinterClothesWorkshop,   // 겨울옷 공방
    Cauldron,                // 가마솥
    PowerPlant,              // 발전소
    End
}

public enum EnergyBuildingType
{
    CatTower,         // 캣 타워
    Scratcher,        // 스크래처
    HotSpring,        // 온천
    bonfire,          // 모닥불
    Box,              // 박스
    End
}

public static class BuildingManager
{
    public static Dictionary<GoldBuildingType, int> s_GoldBuildingCount = new Dictionary<GoldBuildingType, int>();
    public static Dictionary<EnergyBuildingType, int> s_EnergyBuildingCount = new Dictionary<EnergyBuildingType, int>();

    public static List<GoldProductionBuilding> s_GoldProductionBuildings = new List<GoldProductionBuilding>();
    public static List<EnergyProductionBuilding> s_EnergyProductionBuildings = new List<EnergyProductionBuilding>();

    public static void Init()
    {
        for (int buildingtype = 0; buildingtype < (int)GoldBuildingType.End; buildingtype++)
        {
            s_GoldBuildingCount.Add((GoldBuildingType)buildingtype, 0);
        }

        for (int buildingtype = 0; buildingtype < (int)EnergyBuildingType.End; buildingtype++)
        {
            s_EnergyBuildingCount.Add((EnergyBuildingType)buildingtype, 0);
        }
    }

    public static bool CanGoRest(out Vector3 pos)
    {
        pos = Vector3.zero;

        if (s_EnergyProductionBuildings.Count == 0)
            return false;

        for (int i = 0; i < s_EnergyProductionBuildings.Count; i++)
            if (s_EnergyProductionBuildings[i].CanDeploy())
            {
                pos = s_EnergyProductionBuildings[i].transform.position;
                return true;
            }

        return false;
    }

    /// <summary>
    /// 현재 배치할 건물 정보 가져오기
    /// 
    /// 어차피 골드 생산하는 건물밖에 설치 못함
    /// </summary>
    public static GoldProductionBuilding GetGoldProductionBuilding(ProductionBuilding Productionbuilding) => Productionbuilding as GoldProductionBuilding;
}


