using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 골드 생산 건물 고양이 능력
/// </summary>

public enum GoldAbilityType
{
    Fishing,                // 낚시
    Mining,                 // 광질
    Axing,                  // 도끼질
    Farming,                // 농사
    Kiln,                   // 가마질
    Knitting,               // 뜨개질 
    Boiling,                // 끓이기
    GeneratorOperating,     // 발전기
    End
}

public class Cat : MonoBehaviour
{
    public string Name;
    public GoldAbilityType GoldAbilityType;
    public int AbilityRating;

    public int Status;
    public int Recovery;
    

    public int ReductionTimebyGrade => AbilityRating switch
    {
        1 => 10,
        2 => 15,
        3 => 20,
        _ => throw new System.Exception("Cat Ability Rating that does not exist")
    };



    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 고양이 휴식 
    /// 골드 10번 생산했을시 호출
    /// 에너지 생산 건물로 가서 에너지 생산
    /// </summary>
    public void GoToRest()
    {

    }
}
