using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : Singleton<CatManager>
{
    public List<CatData> CatDataList = new List<CatData>();
    public List<Cat> CatList = new List<Cat>();
    public CatInfo[] catInfos;
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
            CatDataList.Add(cat.catData);
            CatList.Add(cat);
        }
    }
    /// <summary>
    /// 마을 내보내기
    /// </summary>
    public void RemoveCat(CatData cat)
    {
        CatDataList.Remove(cat);

        // 쫓겨나는 이벤트
    }
    public Sprite GetCatAbiltySprite(GoldAbilityType type) => abiltySpritesList[(int)type];


}
