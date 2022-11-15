using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileTouchZoom : MonoBehaviour
{
    private float m_oldCamSize = 0f;       // 터치 이전 거리를 저장합니다.
    [SerializeField]
    private float m_camSize = 5f;     // 카메라의 Size의 기본값을 5로 정합니다.


    private Vector3 touchPos;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0 && GridBuildingSystem.IsDeploying == false) CheckTouch();

        //PCDebuging();
    }
    void CheckTouch()
    {
        int nTouch = Input.touchCount;
        float m_fToucDis = 0f;
        float fDis = 0f;

        if (IsPointerOverGameObject())
            return;

        // 터치가 두개이고, 두 터치중 하나라도 이동한다면 카메라의 fieldOfView를 조정합니다.
        if (nTouch == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (m_fToucDis - m_oldCamSize) * 0.01f;

            // 이전 두 터치의 거리와 지금 두 터치의 거리의 차이를 FleldOfView를 차감합니다.
            m_camSize -= fDis;

            // 최대는 100, 최소는 20으로 더이상 증가 혹은 감소가 되지 않도록 합니다.
            m_camSize = Mathf.Clamp(m_camSize, 20.0f, 100.0f);

            // 확대 / 축소가 갑자기 되지않도록 보간합니다.
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, m_camSize, Time.deltaTime * 5);

            m_oldCamSize = m_fToucDis;
        }
        else if (nTouch == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                Camera.main.transform.position += Vector3.Normalize(touchPos - (Vector3)Input.touches[0].position) * Time.deltaTime * 5;
                touchPos = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Began)
            {
                touchPos = Input.touches[0].position;
            }
        }
    }

    private void PCDebuging()
    {
        if (IsPointerOverGameObject()) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        m_camSize -= scroll;
        m_camSize = Mathf.Clamp(m_camSize, 1f, 7f);
        mainCam.orthographicSize = m_camSize;
        if (Input.GetMouseButton(0))
        {
            mainCam.transform.position += new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0) * Time.deltaTime * 5;
        }
    }

    private bool IsPointerOverGameObject()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Check mouse
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            // Check touches
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) // 장실00
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
