using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FindObjectType을 사용하여 쓸 것
/// 
/// </summary>
public class CatManager : Singleton<CatManager>
{
    public List<CatData> CatList = new List<CatData>();
    public Sprite[] catSpritesList;


    /// <summary>
    /// 마을 내보내기
    /// </summary>
    public void RemoveCat(CatData cat) => CatList.Remove(cat);

    public Sprite ReturnCatSprite(CatSkinType catSkinType)
    {
        return catSpritesList[(int)catSkinType];
    }

}
