using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour
{
    public GameObject WarningUI;
    [SerializeField] protected TextMeshProUGUI WarningText;
    [SerializeField] protected Button YesButton;
    [SerializeField] protected Button NoButton;
}
