using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BgmUIInfo : MonoBehaviour
{
    public Toggle favoriteSettingBtn;
    public Button purchaseBtn;
    public Button playBtn;
    void Start()
    {
        favoriteSettingBtn = transform.GetChild(0).GetComponent<Toggle>();
        purchaseBtn = transform.GetChild(1).GetComponent<Button>();
        playBtn = transform.GetChild(2).GetComponent<Button>();
    }
}
