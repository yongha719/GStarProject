using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// TODO : 시간나면 Custom Attribute 만들어서 해봐야징 히히
[RequireComponent(typeof(Button))]
public class UIClose : MonoBehaviour, IPointerDownHandler
{
    private UIPopup UIPopup;

    void Awake()
    {
        UIPopup = transform.GetChild(0).GetComponent<UIPopup>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.Equals(EventSystem.current.currentSelectedGameObject))
        {
            UIPopup.Disable();
        }
    }

}
