using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatInvite : MonoBehaviour
{
    public void CatInviteBtnFunc(double needGoldValue)
    {
        if (needGoldValue <= GameManager.Instance._coin)
        {
            GameManager.Instance._coin -= needGoldValue;
            StartCoroutine(GachaEffect());
        }
        else
        {
            Debug.Log("소지 코인 부족");
        }
    }
    IEnumerator GachaEffect()
    {
        yield return null;
    }
}
