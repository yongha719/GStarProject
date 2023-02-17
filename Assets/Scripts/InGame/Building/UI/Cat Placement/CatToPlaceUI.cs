using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// 건물에 고양이 배치할 수 있는 고양이 UI
/// </summary>
public class CatToPlaceUI : MonoBehaviour
{
    [SerializeField] private Image CatImage;
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private CatAbilityUI Ability;
    [SerializeField] private Button PlacementButton;

    /// <summary>
    /// 배치할 고양이 정보 설정
    /// </summary>
    /// <param name="onclick">배치 버튼에 들어갈 onclick 이벤트</param>
    public void SetData(CatData catData, Action<CatToPlaceUI, CatData> onclick)
    {
        CatImage.sprite = catData.CatSprite;
        CatNameText.text = catData.Name;

        Ability.SetAbility(catData);

        PlacementButton.onClick.AddListener(() =>
        {
            onclick?.Invoke(this, catData);
        });
    }

    public void SetData(CatData catData)
    {
        CatImage.sprite = catData.CatSprite;
        CatNameText.text = catData.Name;

        Ability.SetAbility(catData);
    }
}
