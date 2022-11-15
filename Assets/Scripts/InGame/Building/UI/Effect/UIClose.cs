using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO : 시간나면 Custom Attribute 만들어서 해봐야징 히히
[RequireComponent(typeof(Button))]
public class UIClose : MonoBehaviour, IPointerDownHandler
{
    public UIPopup UIPopup;

    void Awake()
    {
        var colorblock = GetComponent<Button>().colors;
        colorblock.pressedColor = Color.white;
        GetComponent<Button>().colors = colorblock;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject == EventSystem.current.currentSelectedGameObject)
        {
            UIPopup.Disable();
        }
    }

}
