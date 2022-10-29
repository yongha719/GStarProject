using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingCategory : Category
{
    private static Category s_Category;
    private static GameObject s_CategoryBuildings;

    protected override void Start()
    {
        base.Start();

        if (isSelected)
        {
            s_Category = this;
            s_CategoryBuildings = CurCategoryBuildings;
        }
    }

    protected override void Select()
    {
        if (s_Category == this)
            return;

        s_Category.Unselect();
        s_Category = this;


        s_CategoryBuildings.SetActive(false);
        CurCategoryBuildings.SetActive(true);
        s_CategoryBuildings = CurCategoryBuildings;


        rect.DOAnchorPosY(-70f, 0.3f);
    }

    public override void Unselect()
    {
        base.Unselect();         

        rect.DOAnchorPosY(60f, 0.3f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
