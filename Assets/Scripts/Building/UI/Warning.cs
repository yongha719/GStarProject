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

    public void SetWarningData(GameObject building, string text)
    {
        CurBuilding = building;
        WarningText.text = text;
    }

    private void Awake()
    {
        print("aa");

    }
    private void Start()
    {
        print("ss");


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
}
