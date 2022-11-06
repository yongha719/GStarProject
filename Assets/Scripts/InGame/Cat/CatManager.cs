using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatInfo", menuName = "CatInfo", order = int.MinValue)]
[System.Serializable]
public class CatInfo : ScriptableObject
{
    public string CatName;
    public Sprite CatSprite;
    public CatSkinType CatType;
}
public class CatManager : Singleton<CatManager>
{
    public List<CatData> CatList = new List<CatData>();
    public CatInfo[] catInfos;
    private Sprite[] abiltySpritesList;

    private void Awake()
    {
        catInfos = Resources.LoadAll<CatInfo>("CatInfos");
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
    public Sprite GetCatAbiltySprite(GoldAbilityType type) => abiltySpritesList[(int)type];


}
