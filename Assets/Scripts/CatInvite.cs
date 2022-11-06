using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class CatInvite : MonoBehaviour
{
    [SerializeField]
    private CatInviteEffect inviteEffect;
    [SerializeField]
    private TMP_InputField catNameTextArea;
    private CatData curCatData;


    [Header("Result UI")]

    [SerializeField]
    private GameObject[] Stars;
    [SerializeField]
    private Image catSprite;
    [SerializeField]
    private Image skillIcon;
    [SerializeField]
    private TextMeshProUGUI catName;

    [Header("Animation Effect")]
    public GameObject CatShadow;
    public Image whiteScreen;
    public GameObject resultUI;
    public ParticleSystem slowSnow;
    public ParticleSystem fastSnow;
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

        curCatData = RandomCatEarn();

        for (int i = 0; i < Stars.Length; i++)
            Stars[i].SetActive(i < curCatData.AbilityRating);
        catSprite.sprite = curCatData.SkinImage;
        catName.text = curCatData.Name;
        skillIcon.sprite = curCatData.AbilityImage;

    }
    public void CatNaming()
    {
        if (catNameTextArea.text != null)
        {
            curCatData.Name = catNameTextArea.text;
            CatManager.Instance.CatList.Add(curCatData);
            curCatData = null;
        }
        else
        {
            Debug.Log("이름 짓기 오류 문구 뛰워야함");
        }
    }

    public CatData RandomCatEarn()
    {
        CatData catData = new CatData();

        catData.GoldAbilityType = (GoldAbilityType)Random.Range(0, (int)GoldAbilityType.End);
        catData.CatSkinType = (CatSkinType)Random.Range(0, (int)CatSkinType.End);
        catData.AbilityImage = CatManager.Instance.ReturnCatAbiltySprite(catData.GoldAbilityType); ;
        catData.SkinImage = CatManager.Instance.ReturnCatSprite(catData.CatSkinType);

        int value = Random.Range(0, 20);
        if (value < 3)
            catData.AbilityRating = 3;
        else if (value < 7)
            catData.AbilityRating = 2;
        else
            catData.AbilityRating = 1;

        catData.Name = null;

        return catData;
    }
    public void StartEffect()
    {
        StartCoroutine(PlayEffect());
    }
    public void PressBtn()
    {
        fastSnow.gameObject.SetActive(true);
        CatShadow.SetActive(true);
    }
    IEnumerator PlayEffect()
    {
        whiteScreen.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        fastSnow.Stop();
        slowSnow.Play();
        whiteScreen.color = new Color(1, 1, 1, 0);
        resultUI.SetActive(true);
    }
}
