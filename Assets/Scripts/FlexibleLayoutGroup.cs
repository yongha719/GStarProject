using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class FlexibleLayoutGroup : MonoBehaviour
{
    private GridLayoutGroup layoutGroup;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        layoutGroup = GetComponent<GridLayoutGroup>();
    }
    private void Update()
    {
        DoFleixble();
    }
    void DoFleixble()
    {
        //rectTransform.localScale.y = transform.childCount * (layoutGroup.cellSize.y + layoutGroup.spacing.y);
    }
}
