using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatManager : Singleton<CatManager>
{
    public List<CatData> CatList = new List<CatData>();
    private Sprite[] catSpritesList;
    private Sprite[] abiltySpritesList;


    private void Awake()
    {
        catSpritesList = Resources.LoadAll<Sprite>("CatSprites");
        abiltySpritesList = Resources.LoadAll<Sprite>("AbiltySprites");
    }

    /// <summary>
    /// 마을 내보내기
    /// </summary>
    public void RemoveCat(CatData cat)
    {
        CatList.Remove(cat);

        // 쫓겨나는 이벤트
    }
    public Sprite ReturnCatAbiltySprite(GoldAbilityType type)
    {
        return abiltySpritesList[(int)type];
    }

    public Sprite GetCatSprite(CatSkinType catSkinType) => catSpritesList[(int)catSkinType];

}
