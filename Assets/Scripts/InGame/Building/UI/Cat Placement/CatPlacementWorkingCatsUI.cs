using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 고양이 배치에서 건물에서 일하는 고양이
/// </summary>
public class CatPlacementWorkingCatsUI : UIPopup
{
    // 그냥 개체 하나 더 만들까 
    [SerializeField] List<Image> CatImages;
    [SerializeField] List<Button> CatClickButtons;
    [SerializeField] List<Animator> CatAnimators;
    public List<CatAbilityUI> CatAbilitys = new List<CatAbilityUI>();

    public Transform AbilityParent;
    public GameObject AbilityPrefab;
    public List<CatData> CatDatas = new List<CatData>();

    public int MaxDeployableCat;

    private Sprite TransparentImage;
    int curCatNum;

    private void Start()
    {
        TransparentImage = CatImages[0].sprite;
    }

    public void RemoveCat(CatData catData)
    {
        int index = CatDatas.IndexOf(catData);

        CatImages[index].sprite = TransparentImage;
        //CatClickButtons[index].onClick.RemoveAllListeners();
        CatAbilitys.RemoveAt(index);
        Destroy(AbilityParent.GetChild(index).gameObject);
        CatDatas.Remove(catData);
    }

    public void SetData(int index, Animator animator)
    {
        CatAnimators[index] = animator;
    }

    /// <summary>
    /// 고양이 배치에서 건물에서 일하는 고양이 정보 설정
    /// </summary>    
    /// <param name="call">고양이 눌렀을때 이벤트</param>
    public void SetData(int index, CatData CatData, Action<int> call = null)
    {
        curCatNum = CatClickButtons.IndexOf(CatClickButtons[index]);

        CatImages[index].sprite = CatData.CatSprite;
        CatAbilitys[index].SetAbility(CatData);
        CatAnimators[index].runtimeAnimatorController = CatData.Cat.AnimatorController;

        if (CatDatas.Count < index + 1)
        {
            CatDatas.Add(CatData);
            print("CatData Add");
            print(CatDatas.Count);
            print(gameObject);
        }
        else
            CatDatas[index] = CatData;

        CatClickButtons[index].onClick.AddListener(() =>
        {
            call?.Invoke(curCatNum);
        });
    }
}