using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTouchZoom : MonoBehaviour
{
    private float m_fOldToucDis = 0f;       // 터치 이전 거리를 저장합니다.
    private float m_fFieldOfView = 60f;     // 카메라의 FieldOfView의 기본값을 60으로 정합니다.

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        // if (Input.touchCount > 0) CheckTouch();

        PCDebuging();
    }
    void CheckTouch()
    {
        int nTouch = Input.touchCount;
        float m_fToucDis = 0f;
        float fDis = 0f;

        // 터치가 두개이고, 두 터치중 하나라도 이동한다면 카메라의 fieldOfView를 조정합니다.
        if (nTouch == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

            // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감합니다.
            m_fFieldOfView -= fDis;

            // 최대는 100, 최소는 20으로 더이상 증가 혹은 감소가 되지 않도록 합니다.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 20.0f, 100.0f);

            // 확대 / 축소가 갑자기 되지않도록 보간합니다.
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, m_fFieldOfView, Time.deltaTime * 5);

            m_fOldToucDis = m_fToucDis;
        }
        else if (nTouch == 1 && Input.touches[0].phase == TouchPhase.Moved)
        {

        }
    }

    void PCDebuging()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        m_fFieldOfView += scroll * Time.deltaTime;
        m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 20f, 100f);
        mainCam.fieldOfView = m_fFieldOfView;
        if (Input.GetMouseButton(0))
        {
            new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
        }
    }
}
