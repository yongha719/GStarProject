using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatInvite : MonoBehaviour
{
    public double needGoldValue;

    [SerializeField]
    private GameObject CatObj;

    [SerializeField]
    private TextMeshProUGUI needGoldText;
    [SerializeField]
    private TMP_InputField catNameTextArea;
    private CatData curCatData;
    private bool nowCatInviting;

    private Image mySelfImage;

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

    public static AudioClip nowBgm;
    [Header("Naming Tap")]
    [SerializeField]
    private GameObject spriteMask;
    [SerializeField]
    private GameObject setCatNameObj;


    private void OnEnable()
    {
        slowSnow.Play();
        CostTextAccept();
    }

    private void Start()
    {
        mySelfImage = transform.parent.GetComponent<Image>();
    }

    private void CostTextAccept()
    {
        needGoldValue = CatManager.Instance.CatDataList.Count * 500;
        needGoldText.text = CalculatorManager.returnStr(needGoldValue); 
    }

    public void CatInviteBtnFunc()
    {
        if (!nowCatInviting && needGoldValue <= GameManager.Instance._coin)
        {
            GameManager.Instance._coin -= needGoldValue;
            CatShadow.gameObject.SetActive(true);
            fastSnow.Play();
            nowCatInviting = true;
            nowBgm = SoundManager.Instance.audioSources[SoundType.BGM].clip;
            SoundManager.Instance.PlaySoundClip("BGM_Cat_Invite", SoundType.BGM);
            GachaEffect();
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX);
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
        if (catNameTextArea.text != null && catNameTextArea.text != "")
        {
            curCatData.Name = catNameTextArea.text;

            GameObject catObj = Instantiate(CatObj, Vector3.zero, Quaternion.identity);
            catObj.GetComponent<Cat>().catData = curCatData;

            curCatData.Cat = CatObj.GetComponent<Cat>();
            CatManager.Instance.CatList.Add(curCatData.Cat);
            CatManager.Instance.CatDataList.Add(curCatData);

            CostTextAccept();
            catNameTextArea.text = null;
            curCatData = null;
            UIManager.Instance.ResourcesApply();

            nowCatInviting = false;

            spriteMask.SetActive(true);
            setCatNameObj.SetActive(false);
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX, 2);
        }
    }

    static public CatData RandomCatEarn()
    {
        CatData catData = new CatData();

        catData.GoldAbilityType = (GoldAbilityType)Random.Range(0, (int)GoldAbilityType.End);
        catData.CatSkinType = (CatSkinType)Random.Range(0, (int)CatSkinType.End);
        catData.AbilitySprite = CatManager.Instance.GetCatAbiltySprite(catData.GoldAbilityType);
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

    public void ScreenOn(bool onOff)
    {
        if (nowCatInviting) return;
        if (onOff)
        {
            Vector3 pos = FindObjectOfType<VillageHall>().transform.position;
            Camera.main.transform.DOMove(new Vector3(pos.x, pos.y + 1.25f, -10), 0.3f);
            mySelfImage.DOFade(0.5f, 0);
            transform.DOScale(1, 0.3f);
        }
        else
        {
            transform.DOScale(0, 0.3f).OnComplete(() => mySelfImage.DOFade(0f, 0));
        }
    }
}
