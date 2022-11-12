using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 고양이 배치에서 건물에서 일하는 고양이
/// </summary>
public class CatPlacementWorkingCats : MonoBehaviour
{
    // 그냥 객체 하나 더 만들까 
    [SerializeField] List<Image> CatImages;
    [SerializeField] List<Button> CatClickButtons;
    public List<CatAbilityUI> CatAbilitys = new List<CatAbilityUI>();

    public GameObject AbilityPrefab;

    public List<CatData> CatDatas = new List<CatData>();

    int curCatNum;

    /// <param name="call">고양이 눌렀을때 이벤트</param>
    public void SetData(int index, Sprite catSprite, Action call)
    {
        CatImages[index].sprite = catSprite;
        CatClickButtons[index].onClick.AddListener(() =>
        {
            call();
        });
    }

    /// <summary>
    /// 일하고 있는 고양이와 바꾸기
    /// </summary>
    public void SetData(int index, CatData CatData)
    {
        print(index);
        CatDatas[index] = CatData;
        CatImages[index].sprite = CatData.CatSprite;
        CatAbilitys[index].SetAbility(CatData.AbilitySprite, CatData.AbilityRating);
    }

    /// <summary>
    /// 고양이 배치에서 건물에서 일하는 고양이 정보 설정
    /// </summary>    
    /// <param name="call">고양이 눌렀을때 이벤트</param>
    public void SetData(int index, CatData CatData, Action<int> call)
    {
        curCatNum = CatClickButtons.IndexOf(CatClickButtons[index]);

        CatImages[index].sprite = CatData.CatSprite;

        CatDatas.Add(CatData);

        if (CatAbilitys.Count < index + 1)
        {
            var catAbilityUI = Instantiate(AbilityPrefab).GetComponent<CatAbilityUI>();
            catAbilityUI.SetAbility(CatData.AbilitySprite, CatData.AbilityRating);
            CatAbilitys.Add(catAbilityUI);
        }
        else
        {
            CatAbilitys[index].SetAbility(CatData.AbilitySprite, CatData.AbilityRating);
        }


        CatClickButtons[index].onClick.AddListener(() =>
        {
            call(curCatNum);
        });
    }
}