using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public enum GoldBuildingType
{
    IceFishing, GoldMine, FirewoodChopping, PotatoFarming
}

public enum EnergyBuildingType
{

}

public static class BuildingManager
{
    public static Dictionary<GoldBuildingType, int> s_GoldBuildings = new Dictionary<GoldBuildingType, int>();
    public static Dictionary<EnergyBuildingType, int> s_EnergyBuildings = new Dictionary<EnergyBuildingType, int>();


    public static void BuildingLevelUp(GoldBuildingType buildingType)
    {
        s_GoldBuildings[buildingType]++;
    }

    public static void BuildingLevelUp(EnergyBuildingType buildingType)
    {
        s_EnergyBuildings[buildingType]++;
    }
}


