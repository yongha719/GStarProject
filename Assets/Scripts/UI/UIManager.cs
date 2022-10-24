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

    private bool onTitle = true;

    [SerializeField]
    private Image title;
    [SerializeField]
    private TextMeshProUGUI pressToStartText;

    private GameManager gameManager;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        ResourcesApply();
        StartCoroutine(TitleEffect());
    }

    public void ResourcesApply()
    {
        catText.text = $"{gameManager.resource.catCount}¸¶¸®";
        coinText.text = CalculatorManager.returnStr(gameManager.resource.coin);
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
            pressToStartText.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Cos(Time.time)));

            if (Input.GetMouseButtonDown(0)) break;
            yield return null;
        }

        title.DOFade(0, 1).SetEase(Ease.InBack);
        pressToStartText.DOFade(0, 1).SetEase(Ease.InBack);
        pressToStartText.DOColor(new Color(1, 1, 1, 0), 1);
        yield return new WaitForSeconds(1);

        foreach (Image image in images)
            image.DOFade(1, 0.5f);
        foreach (TextMeshProUGUI text in texts)
            text.DOFade(1, 0.5f);

    }
}
