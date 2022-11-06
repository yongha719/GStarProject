using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using DG.Tweening.Core;

public class CatInvite : MonoBehaviour
{
    public double needGoldValue;

    [SerializeField]
    private TextMeshProUGUI needGoldText;
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
    private void OnEnable()
    {
        needGoldValue = CatManager.Instance.CatList.Count * 500;
        needGoldText.text = CalculatorManager.returnStr(needGoldValue);
    }
    public void CatInviteBtnFunc()
    {
        if (needGoldValue <= GameManager.Instance._coin)
        {
            GameManager.Instance._coin -= needGoldValue;
            inviteEffect.gameObject.SetActive(true);
            inviteEffect.PressBtn();
            GachaEffect();
        }
        else
        {
            Debug.Log("소지 코인 부족");
        }
    }
    void GachaEffect()
    {
        curCatData = RandomCatEarn();

        for (int i = 0; i < Stars.Length; i++)
            Stars[i].SetActive(i < curCatData.AbilityRating);
        catSprite.sprite = curCatData.CatSprite;
        catName.text = curCatData.Name;
        skillIcon.sprite = curCatData.AbilitySprite;


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
        catData.AbilitySprite = CatManager.Instance.GetCatAbiltySprite(catData.GoldAbilityType); ;
        catData.CatSprite = CatManager.Instance.catInfos[(int)catData.CatSkinType].CatSprite;
        catData.Name = CatManager.Instance.catInfos[(int)catData.CatSkinType].CatName;

        int value = Random.Range(0, 20);
        if (value < 3)
            catData.AbilityRating = 3;
        else if (value < 7)
            catData.AbilityRating = 2;
        else
            catData.AbilityRating = 1;

        return catData;
    }
    public void StartEffect()
    {
        StartCoroutine(PlayEffect());
    }
    public void PressBtn()
    {
        fastSnow.Play();
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
        CatShadow.SetActive(true);
    }
}
