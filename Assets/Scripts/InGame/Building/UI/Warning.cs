using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI WarningText;
    [SerializeField] protected Button YesButton;
    [SerializeField] protected Button NoButton;

    public void WarningUISetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
