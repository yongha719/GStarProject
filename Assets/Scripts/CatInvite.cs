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
        catData.AbilityRating = Random.Range(1, 4);

        catData.Name = catNameTextArea.text;

        return catData;
    }
}
