using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class CatToPlace : MonoBehaviour
{
    [SerializeField] private Image CatImage;
    [SerializeField] private TextMeshProUGUI CatNameText;
    [SerializeField] private Image SkillImage;
    [SerializeField] private List<GameObject> AbilityRatingStars = new List<GameObject>();
    [SerializeField] private Button PlacementButton;

    /// <summary>
    /// 배치할 고양이 정보 설정
    /// </summary>
    /// <param name="onclick">배치 버튼에 들어갈 onclick 이벤트</param>
    public void SetData(Sprite catSprite, string catName, Sprite skillSprite, int abilityRating, Action onclick)
    {
        CatImage.sprite = catSprite;
        CatNameText.text = catName;
        SkillImage.sprite = skillSprite;

        for (int i = 0; i < abilityRating; i++)
        {
            AbilityRatingStars[i].SetActive(true);
        }

        PlacementButton.onClick.AddListener(() => onclick());
    }
}
