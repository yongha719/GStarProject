using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatAbilityUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> RatingStars = new List<GameObject>();

    private Image AbilityImage;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;

        AbilityImage = GetComponent<Image>();
    }

    public void SetAbility(CatAbilityUI catAbilityUI)
    {
        this.AbilityImage = catAbilityUI.AbilityImage;
        this.RatingStars = catAbilityUI.RatingStars;
        this.rectTransform = catAbilityUI.rectTransform;
    }

    public void SetAbility(Sprite abilitySprite, int Rating)
    {
        AbilityImage.sprite = abilitySprite;
        
        foreach (var ratingStar in RatingStars)
            ratingStar.SetActive(false);

        for (int i = 0; i < Rating; i++)
        {
            RatingStars[i].SetActive(true);
        }
    }
}
