using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingCategory : Category
{
    private static Category s_CurCategory;
    private static GameObject s_CurCategoryBuildingList;

    protected override void Start()
    {
        base.Start();

        if (isSelected)
        {
            s_CurCategory = this;
            s_CurCategoryBuildingList = CurCategoryObject;
        }
    }

    protected override void Select()
    {
        if (s_CurCategory == this)
            return;

        base.Select();

        s_CurCategory.Unselect();
        s_CurCategory = this;

        s_CurCategoryBuildingList.SetActive(false);
        CurCategoryObject.SetActive(true);
        s_CurCategoryBuildingList = CurCategoryObject;
    }
}
