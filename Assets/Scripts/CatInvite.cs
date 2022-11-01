using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CatInvite : MonoBehaviour
{
    [SerializeField]
    private CatInviteEffect inviteEffect;

    [SerializeField]
    private InputField catNameTextArea;
    public void CatInviteBtnFunc(double needGoldValue)
    {
        if (needGoldValue <= GameManager.Instance._coin)
        {
            GameManager.Instance._coin -= needGoldValue;
            inviteEffect.gameObject.SetActive(true);
            inviteEffect.PressBtn();
            StartCoroutine(GachaEffect());
        }
        else
        {
            Debug.Log("소지 코인 부족");
        }
    }
    IEnumerator GachaEffect()
    {
        yield return null;
    }

    public CatData RandomCatEarn()
    {
        CatData catData = new CatData();

        catData.GoldAbilityType = (GoldAbilityType)Random.Range(0, (int)GoldAbilityType.End);
        catData.CatSkinType = (CatSkinType)Random.Range(0, (int)CatSkinType.End);

        int value = Random.Range(0, 20);
        if (value < 3)
            catData.AbilityRating = 3;
        else if (value < 7)
            catData.AbilityRating = 2;
        else
            catData.AbilityRating = 1;

        catData.Name = catNameTextArea.text;

        return catData;
    }
}
