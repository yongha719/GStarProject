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
    [SerializeField] List<Image> CatImages;
    [SerializeField] List<Button> CatClickButtons;
    public List<CatAbilityUI> CatAbilitys;

    /// <param name="call">고양이 눌렀을때 이벤트</param>
    public void SetData(int index, Sprite catSprite, Action call)
    {
        CatImages[index].sprite = catSprite;
        CatClickButtons[index].onClick.AddListener(() =>
        {
            call();


        });
    }
}