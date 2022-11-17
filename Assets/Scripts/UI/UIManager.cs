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

    private GameManager gameManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        ResourcesApply();
        StartCoroutine(TitleEffect());
    }

    public void ResourcesApply()
    {
        catText.text = $"{CatManager.Instance.CatList.Count}마리";
        coinText.text = gameManager.resource.coin.returnStr(); CalculatorManager.returnStr(gameManager.resource.coin);
        iceText.text = CalculatorManager.returnStr(gameManager.resource.ice);
        energyText.text = CalculatorManager.returnStr(gameManager.resource.energy);

    }
    private IEnumerator TitleEffect()
    {
        Image[] images = transform.GetChild(0).GetComponentsInChildren<Image>();
        TextMeshProUGUI[] texts = transform.GetChild(0).GetComponentsInChildren<TextMeshProUGUI>();

        foreach (Image image in images) image.color = new Color(1, 1, 1, 0);
        foreach (TextMeshProUGUI text in texts) text.color = new Color(1, 1, 1, 0);
        while (true)
        {
            title.transform.position += Vector3.up * Time.deltaTime * Mathf.Cos(Time.time) / 5;
            pressToStartText.DOFade(Mathf.Abs(Mathf.Cos(Time.time)), 0);

            if (Input.GetMouseButtonDown(0)) break;
            yield return null;
        }

        SoundManager.Instance.PlaySoundClip("SFX_Button_Touch", SoundType.SFX);
        title.DOFade(0, 1).SetEase(Ease.InBack);
        pressToStartText.DOFade(0, 1).SetEase(Ease.InBack);
        yield return new WaitForSeconds(1);

        title.gameObject.SetActive(false);
        pressToStartText.gameObject.SetActive(false);

        foreach (Image image in images)
            image.DOFade(1, 0.5f);
        foreach (TextMeshProUGUI text in texts)
            text.DOFade(1, 0.5f);

    }
}
