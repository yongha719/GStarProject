using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Warning : MonoBehaviour
{
    private GameObject CurBuilding;
    [SerializeField] private TextMeshProUGUI WarningText;
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton;

    private const string WARNING_PHRASE = "(를)을\n설치하시겠습니까?";

    private void Awake()
    {

    }
    private void Start()
    {
        print("st");
        YesButton.onClick.AddListener(() =>
        {
            GridBuildingSystem.Instance.InitializeWithBuilding(CurBuilding);
            gameObject.SetActive(false);
        });

        NoButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
    public void SetWarningData(GameObject building, string text, System.Action action)
    {
        print("me");
        CurBuilding = building;
        WarningText.text = $"{text}{WARNING_PHRASE}";

        action();
    }
}
