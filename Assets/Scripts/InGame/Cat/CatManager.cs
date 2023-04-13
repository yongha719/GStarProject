using System;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Singleton<CatManager>
{
    public List<Cat> CatList = new List<Cat>();
    public CatInfo[] catInfos;


    private void Awake()
    {
        catInfos = Resources.LoadAll<CatInfo>("CatInfos");
    }

    private void Start()
    {

    }

    /// <summary>
    /// 마을 내보내기
    /// </summary>
    public void RemoveCat(Cat cat)
    {
        if (CatList.Contains(cat))
        {
            Destroy(cat.gameObject);
            CatList.Remove(cat);
            cat.goldBuilding.WorkingCatsUI.RemoveCat(cat.catData);
        }

        // 쫓겨나는 이벤트
    }

    /// <summary>
    /// 고양이가 움직일 수 있는 범위 바꾸기 
    /// 마을 회관 레벨업시 호출
    /// </summary>
    public void ChangeRangeCatMovement(int area)
    {
        foreach (var cat in CatList)
        {
            cat.canMoveArea = area;
        }
    }

    public CatInfo GetCatInfo(int index)
    {
        return catInfos[index];
    }

}
