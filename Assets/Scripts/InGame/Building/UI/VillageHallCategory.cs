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
        if (s_curCategory == this)
            return;

        base.Select();

        s_curCategory.Unselect();
        s_curCategory = this;

        s_curCategoryObj.SetActive(false);
        s_curCategoryObj = CurCategoryObject;
        s_curCategoryObj.SetActive(true);
    }
}