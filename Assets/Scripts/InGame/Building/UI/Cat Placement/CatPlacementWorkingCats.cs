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

    public List<CatData> CatData = new List<CatData>();
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
    /// 고양이 배치에서 건물에서 일하는 고양이 정보 설정
    /// </summary>    
    /// <param name="call">고양이 눌렀을때 이벤트</param>
    public void SetData(int index, CatData CatData, Action<int> call)
    {
        print("dd");
        CatImages[index].sprite = CatData.CatSprite;

        if (CatAbilitys.Count == 0 || CatAbilitys.Count <= index)
        {
            print(nameof(CatPlacementWorkingCats));
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