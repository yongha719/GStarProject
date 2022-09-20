using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileTouchZoom : MonoBehaviour
{
    float m_fOldToucDis = 0f;       // ��ġ ���� �Ÿ��� �����մϴ�.
    float m_fFieldOfView = 60f;     // ī�޶��� FieldOfView�� �⺻���� 60���� ���մϴ�.
    void Update()
    {
        CheckTouch();
    }
    void CheckTouch()
    {
        int nTouch = Input.touchCount;
        float m_fToucDis = 0f;
        float fDis = 0f;

        // ��ġ�� �ΰ��̰�, �� ��ġ�� �ϳ��� �̵��Ѵٸ� ī�޶��� fieldOfView�� �����մϴ�.
        if (Input.touchCount == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            m_fToucDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;

            fDis = (m_fToucDis - m_fOldToucDis) * 0.01f;

            // ���� �� ��ġ�� �Ÿ��� ���� �� ��ġ�� �Ÿ��� ���̸� FleldOfView�� �����մϴ�.
            m_fFieldOfView -= fDis;

            // �ִ�� 100, �ּҴ� 20���� ���̻� ���� Ȥ�� ���Ұ� ���� �ʵ��� �մϴ�.
            m_fFieldOfView = Mathf.Clamp(m_fFieldOfView, 20.0f, 100.0f);

            // Ȯ�� / ��Ұ� ���ڱ� �����ʵ��� �����մϴ�.
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, m_fFieldOfView, Time.deltaTime * 5);

            m_fOldToucDis = m_fToucDis;
        }
    }
}
