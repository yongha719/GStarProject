using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;

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
    private AbiltyScriptable[] abiltyInfos;

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
    [SerializeField]
    private Image skillInfoBallon;
    [SerializeField]
    private CatInviteDescription catInviteDescriptionScripts;

    public static AudioClip nowBgm;
    [Header("Animation Effect")]
    public GameObject CatShadow;
    public Image whiteScreen;
    public GameObject resultUI;
    public ParticleSystem slowSnow;
    public ParticleSystem fastSnow;

    [Header("Naming Tap")]
    [SerializeField]
    private GameObject spriteMask;
    [SerializeField]
    private GameObject setCatNameObj;

    private CatManager mCatManager;

    private void OnEnable()
    {
        slowSnow.Play();
        CostTextAccept();
    }

    private void Start()
    {
        abiltyInfos = Resources.LoadAll<AbiltyScriptable>("AbiltyInfo/");
        mySelfImage = transform.parent.GetComponent<Image>();

        mCatManager = CatManager.Instance;
    }

    private void CostTextAccept()
    {
        if (mCatManager.CatList != null) needGoldValue = mCatManager.CatList.Count * 500;
        else needGoldValue = 0;
        needGoldText.text = needGoldValue.returnStr();
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

    public void SetCatInfo(CatData catData, in CatInfo catInfo)
    {
        catData.CatSprite = catInfo.CatSprite;
        catData.Name = catInfo.CatName;
        catData.CatAnimator = catInfo.CatAnimator;
    }

    public CatData RandomCatEarn()
    {
        CatData catData = new CatData();
        AbiltyScriptable abilty = GetAbiltyScriptable((GoldAbilityType)Random.Range(0, (int)GoldAbilityType.End));


        catData.GoldAbilityType = abilty.type;
        catData.AbilitySprite = abilty.icon;
        catData.CatSkinType = (CatSkinType)Random.Range(0, mCatManager.catInfos.Length);

        SetCatInfo(catData, mCatManager.GetCatInfo((int)catData.CatSkinType));

        int value = Random.Range(0, 20);
        if (value < 3)
            catData.AbilityRating = 3;
        else if (value < 7)
            catData.AbilityRating = 2;
        else
            catData.AbilityRating = 1;

        string[] abiltyInfoTexts = abilty.abiltyDesc.Split('@');

        catInviteDescriptionScripts.SkillInfoApply(abiltyInfoTexts, catData.AbilityRating);

        return catData;
    }

    public void CatNaming()
    {
        if (catNameTextArea.text != null && catNameTextArea.text != "")
        {
            curCatData.Name = catNameTextArea.text;

            var cat = Instantiate(CatObj, Vector3.zero, Quaternion.identity).GetComponent<Cat>();
            cat.catData = curCatData;

            CatManager.Instance.CatList.Add(cat);

            CostTextAccept();
            catNameTextArea.text = null;
            //curCatData = null;
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



    public void ScreenOn(bool onOff)
    {
        if (nowCatInviting) return;
        if (onOff)
        {
            if (FindObjectOfType<VillageHall>())
            {
                Vector3 pos = FindObjectOfType<VillageHall>().transform.position;
                Camera.main.transform.DOMove(new Vector3(pos.x, pos.y + 1.25f, -10), 0.3f);
            }
            mySelfImage.DOFade(0.5f, 0f);
            transform.DOScale(1f, 0.3f);
        }
        else
        {
            transform.DOScale(0, 0.3f).OnComplete(() => mySelfImage.DOFade(0f, 0));
        }
    }

    private AbiltyScriptable GetAbiltyScriptable(GoldAbilityType type) => abiltyInfos.ToList().Find((x) => x.type == type);

}
