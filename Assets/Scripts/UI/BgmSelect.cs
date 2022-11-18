using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSelect : MonoBehaviour
{
    public bool[] canTurnOnBgm = new bool[6];

    [SerializeField]
    private AudioClip[] bgms;
    [SerializeField]
    private Transform parentObj;
    private BgmUIInfo[] bgmUIInfos = new BgmUIInfo[6];
    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            bgmUIInfos[i] = parentObj.GetChild(i).GetComponent<BgmUIInfo>();
        }
    }

    public void BgmChange(int index)
    {
        if (canTurnOnBgm[index])
        {
            SoundManager.Instance.PlaySoundClip(bgms[index], SoundType.BGM);
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX, 2);
        }
    }
    public void purchaseBgm(int index)
    {
        if (GameManager.Instance._energy >= 40000)
        {
            GameManager.Instance._energy -= 40000;
            canTurnOnBgm[index] = true;
            bgmUIInfos[index].purchaseBtn.gameObject.SetActive(false);
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX, 2);
        }
    }
}
