using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

/// <summary>
/// 건물에서 배치할 수 고양이 리스트에 있는 UI
/// </summary>
public class CatToPlace : MonoBehaviour
{
    [SerializeField] private Image CatImage;
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private CatAbilityUI Ability;
    [SerializeField] private Button PlacementButton;

    public CatData CatData;

    /// <summary>
    /// 배치할 고양이 정보 설정
    /// </summary>
    /// <param name="onclick">배치 버튼에 들어갈 onclick 이벤트</param>
    public void SetData(CatData catData, Action<CatToPlace, CatData> onclick)
    {
        CatData = catData;

        CatImage.sprite = CatData.CatSprite;
        CatNameText.text = CatData.Name;

        Ability.SetAbility(CatData.AbilitySprite, CatData.AbilityRating);

        PlacementButton.onClick.AddListener(() =>
        {
            onclick?.Invoke(this, CatData);
        });
    }

    public void SetData(CatData catData)
    {
        CatData = catData;

        CatImage.sprite = CatData.CatSprite;
        CatNameText.text = CatData.Name;

        Ability.SetAbility(CatData.AbilitySprite, CatData.AbilityRating);
    }
}
