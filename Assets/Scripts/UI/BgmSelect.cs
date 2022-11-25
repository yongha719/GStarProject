using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmSelect : MonoBehaviour
{
    public bool[] canTurnOnBgm = new bool[7];

    [SerializeField]
    private AudioClip[] bgms;
    [SerializeField]
    private Transform parentObj;
    [SerializeField]
    private Sprite[] sprites = new Sprite[2];
    private BgmUIInfo[] bgmUIInfos = new BgmUIInfo[7];
    private int nowPlayBgmIndex = 0;
    private void Start()
    {
        for (int i = 0; i < canTurnOnBgm.Length; i++)
        {
            bgmUIInfos[i] = parentObj.GetChild(i).GetComponent<BgmUIInfo>();
        }
    }

    public void BgmChange(int index)
    {
        if (canTurnOnBgm[index])
        {
            SoundManager.Instance.PlaySoundClip(bgms[index], SoundType.BGM);
            bgmUIInfos[nowPlayBgmIndex].playBtn.image.sprite = sprites[1];
            nowPlayBgmIndex = index;
            bgmUIInfos[index].playBtn.image.sprite = sprites[0];
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX, 2);
        }
    }
    public void PurchaseBgm(int index)
    {
        if (GameManager.Instance._energy >= 4000000)
        {
            GameManager.Instance._energy -= 4000000;
            canTurnOnBgm[index] = true;
            bgmUIInfos[index].purchaseBtn.gameObject.SetActive(false);
        }
        else
        {
            SoundManager.Instance.PlaySoundClip("SFX_Error", SoundType.SFX, 2);
        }
    }
    public void ObjectSorting()
    {
        for (int i = 0; i < canTurnOnBgm.Length; i++)
        {
            bgmUIInfos[i].gameObject.transform.SetAsLastSibling();
        }

        for (int i = 0; i < canTurnOnBgm.Length; i++)
        {
            if (!bgmUIInfos[i].favoriteSettingBtn.isOn)
            {
                bgmUIInfos[i].gameObject.transform.SetAsFirstSibling();
            }
        }
    }
}
