using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCategory : MonoBehaviour
{
    public Sprite On;
    public Sprite Off;

    [SerializeField] private Button button;
    [SerializeField] private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();
    }


}
