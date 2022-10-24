using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 골드 생산 건물 고양이 능력
/// </summary>

public enum GoldAbilityType
{
    Fishing,               // 낚시
    Mining,                // 광질
    Axing,                 // 도끼질
    Farming,               // 농사
    Kiln,                  // 가마질
    Knitting,              // 뜨개질 
    Boiling,               // 끓이기
    GeneratorOperating     // 발전기
}

public class Cat : MonoBehaviour
{
    public string Name;
    public GoldAbilityType GoldAbilityType;
    int AbilityRating;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
