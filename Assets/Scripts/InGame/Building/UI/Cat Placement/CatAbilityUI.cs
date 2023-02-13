using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatAbilityUI : MonoBehaviour
{
    [SerializeField] private Image AbilityImage;
    [SerializeField] private List<GameObject> RatingStars = new List<GameObject>();

    public void SetAbility(CatAbilityUI catAbilityUI)
    {
        AbilityImage = catAbilityUI.AbilityImage;
        //this.RatingStars = catAbilityUI.RatingStars;
    }

    public void SetAbility(CatData catData)
    {
        AbilityImage.sprite = catData.AbilitySprite;

        foreach (var ratingStar in RatingStars)
            ratingStar.SetActive(false);

        for (int i = 0; i < catData.AbilityRating; i++)
        {
            RatingStars[i].SetActive(true);
        }
    }
}
