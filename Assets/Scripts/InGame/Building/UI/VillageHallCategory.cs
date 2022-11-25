using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageHallCategory : Category
{

    private static VillageHallCategory s_curCategory;
    private static GameObject s_curCategoryObj;

    protected override void Start()
    {
        base.Start();

        if (isSelected)
        {
            s_curCategory = this;
            s_curCategoryObj = CurCategoryObject;
        }
    }

    protected override void Select()
    {
        base.Select();

        if (s_curCategory == this)
            return;

        s_curCategory.Unselect();
        s_curCategory = this;


        s_curCategoryObj.SetActive(false);
        CurCategoryObject.SetActive(true);
        s_curCategoryObj = CurCategoryObject;


        rect.DOAnchorPosY(-100f, 0.3f);
    }

    public override void Unselect()
    {
        base.Unselect();

        rect.DOAnchorPosY(20, 0.3f);
    }

}