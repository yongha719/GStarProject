using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmUIInfo : MonoBehaviour
{
    public Toggle favoriteSettingBtn;
    public Button purchaseBtn;
    public Button playBtn;
    void Awake()
    {
        favoriteSettingBtn = transform.GetChild(0).GetComponent<Toggle>();
        purchaseBtn = transform.GetChild(2).GetComponent<Button>();
        playBtn = transform.GetChild(3).GetComponent<Button>();
    }
}
