using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public enum GoldBuildingType
{
    IceFishing,              // ���� ������
    GoldMine,                // �� ����
    FirewoodChopping,        // ���� �б�
    PotatoFarming,           // ���� ���
    BlastFurnace,            // �뱤��
    WinterClothesWorkshop,   // �ܿ�� ����
    Cauldron,                // ������
    PowerPlant,              // ������
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


