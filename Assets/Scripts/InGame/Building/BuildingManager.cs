using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

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

    End
}

public static class BuildingManager
{
    public static Dictionary<GoldBuildingType, int> s_GoldBuildings = new Dictionary<GoldBuildingType, int>();
    public static Dictionary<EnergyBuildingType, int> s_EnergyBuildings = new Dictionary<EnergyBuildingType, int>();

    public static void Init()
    {
        for (int buildingtype = 0; buildingtype < (int)GoldBuildingType.End; buildingtype++)
        {
            s_GoldBuildings.Add((GoldBuildingType)buildingtype, 1);
        }
    }

    public static void BuildingLevelUp(GoldBuildingType buildingType)
    {
        s_GoldBuildings[buildingType]++;
    }

    public static void BuildingLevelUp(EnergyBuildingType buildingType)
    {
        s_EnergyBuildings[buildingType]++;
    }
}


