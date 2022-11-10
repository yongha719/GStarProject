using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViliageInfoUI : MonoBehaviour
{
    [Header("주민")]
    [SerializeField] private RectTransform CatsContent;
    [SerializeField] private GameObject CatInfoPrefab;

    private CatManager CatManager;

    private void OnEnable()
    {
        if (CatManager.CatList != null)
        {
            for (int i = 0; i < CatsContent.childCount; i++)
                Destroy(CatsContent.GetChild(i).gameObject);

            var CatList = CatManager.CatList;
            var cnt = CatList.Count;

            for (int i = 0; i < cnt; i++)
            {
                var catInfo = Instantiate(CatInfoPrefab, CatsContent).GetComponent<CatInfoUI>();

                catInfo.SetData(CatList[i],
                    call: () =>
                    {
                        CatManager.RemoveCat(CatList[i]);
                    });
            }
        }
    }

    void Awake()
    {
        CatManager = CatManager.Instance;
    }

    void Update()
    {

    }
}
