using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Singleton<CatManager>
{
    public List<Cat> CatList = new List<Cat>();
    public CatInfo[] catInfos;
    public List<RuntimeAnimatorController> CatAnimators = new List<RuntimeAnimatorController>();

    private Sprite[] abiltySpritesList;

    private void Awake()
    {
        catInfos = Resources.LoadAll<CatInfo>("CatInfos");
        abiltySpritesList = Resources.LoadAll<Sprite>("AbiltySprites");
    }

    private void Start()
    {
        //ifNoCatDebuging();
    }

    //만약 고양이가 없을경우를 방지한 디버깅 함수 (튜토리얼이 생긴다면 삭제한다.)
    private void ifNoCatDebuging()
    {
        if (CatList.Count == 0)
        {
            Cat cat = new Cat();
            cat.catData = CatInvite.RandomCatEarn();
            cat.catData.Name = "고양이";
            CatList.Add(cat);
        }
    }

    /// <summary>
    /// 마을 내보내기
    /// </summary>
    public void RemoveCat(Cat cat)
    {
        if (CatList.Contains(cat))
        {
            CatList.Remove(cat);
            Destroy(cat.gameObject);
        }
        else
        {
            print("뭘 지우라는거야\n 다시 확인해");
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

    public Sprite GetCatAbiltySprite(GoldAbilityType type) => abiltySpritesList[(int)type];

}
