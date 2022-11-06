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
    public void SetData(int index, Sprite catSprite, Sprite abilitySprite, int abilityRating, Action call)
    {
        CatImages[index].sprite = catSprite;

        if (CatAbilitys[index] == null)
        {
            CatAbilitys[index] = new CatAbilityUI();
            CatAbilitys[index].SetAbility(abilitySprite, abilityRating);
        }
        else
        {
            CatAbilitys[index].SetAbility(abilitySprite, abilityRating);
        }

        CatClickButtons[index].onClick.AddListener(() =>
        {
            call();


        });
    }
}