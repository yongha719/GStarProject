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

        CatDataList[0].Cat = new Cat();
    }
    private void Start()
    {
        if (CatList.Count == 0) CatList.Add(CatInvite.RandomCatEarn());
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
