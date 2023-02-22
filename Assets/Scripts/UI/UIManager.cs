using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using DG.Tweening;


public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI catText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI iceText;

    //private bool onTitle = true;

    [Header("타이틀UI")]
    [SerializeField]
    private Image title;
    [SerializeField]
    private TextMeshProUGUI pressToStartText;

    [Header("Building LevelUp")]
    [SerializeField] Button BuildingLevelUpButton;
    [SerializeField] Image BuildingImage;
    [SerializeField] TextMeshProUGUI BuildingLevel;
    private ProductionBuilding CurCanLevelUpBuilding;

    CanvasGroup CanvasGroup;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        CanvasGroup = transform.GetComponentInChildren<CanvasGroup>();

        ResourcesApply();
        StartCoroutine(TitleEffect());

        // 레벨업할 때마다 초기화
        BuildingLevelUpButton.onClick.AddListener(() =>
        {

        });
    }

    public void ResourcesApply()
    {
        catText.text = $"{CatManager.Instance.CatList.Count}마리";
        coinText.text = gameManager.resource.coin.returnStr();
        iceText.text = gameManager.resource.ice.returnStr();
        energyText.text = gameManager.resource.energy.returnStr();
    }

    private IEnumerator TitleEffect()
    {
        CanvasGroup.alpha = 0f;

        while (true)
        {
            title.transform.position += Vector3.up * Time.deltaTime * Mathf.Cos(Time.time) / 5;
            pressToStartText.DOFade(Mathf.Abs(Mathf.Cos(Time.time)), 0);

            if (Input.GetMouseButtonDown(0)) break;
            yield return null;
        }

        SoundManager.Instance.PlaySoundClip("SFX_Button_Touch", SoundType.SFX);
        title.DOFade(0f, 1f).SetEase(Ease.InBack);
        pressToStartText.DOFade(0f, 1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(1);

        title.gameObject.SetActive(false);
        pressToStartText.gameObject.SetActive(false);

        CanvasGroup.DOFade(1f, 0.5f);
    }

    /// <summary>
    /// 건물 빠르게 레벨업할 수 있는 UI 버튼
    /// </summary>
    public void SetBuildingLevelUp()
    {
        var building = BuildingManager.GetCanLevelUpBuilding();

        if (building == null || CurCanLevelUpBuilding == building)
            return;

        CurCanLevelUpBuilding = building;

        
    }


}
