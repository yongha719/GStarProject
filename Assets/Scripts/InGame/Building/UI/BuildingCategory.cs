using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingCategory : Category
{
    private static Category s_CurCategory;
    private static GameObject s_CurCategoryBuildings;

    protected override void Start()
    {
        base.Start();

        if (isSelected)
        {
            s_CurCategory = this;
            s_CurCategoryBuildings = CurCategoryObject;
        }
    }

    protected override void Select()
    {
        base.Select();

        if (s_CurCategory == this)
            return;

        s_CurCategory.Unselect();
        s_CurCategory = this;

        s_CurCategoryBuildings.SetActive(false);
        CurCategoryObject.SetActive(true);
        s_CurCategoryBuildings = CurCategoryObject;
    }

    public override void Unselect()
    {
        base.Unselect();         
    }

}
