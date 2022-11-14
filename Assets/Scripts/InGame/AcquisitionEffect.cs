using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AcquisitionEffect : MonoBehaviour
{
    public Vector2 targetpos = new Vector2();

    RectTransform RectTransform;
    Rigidbody2D rigid;

    IEnumerator Start()
    {
        RectTransform = transform as RectTransform;
        var parentPos = (RectTransform.parent as RectTransform).anchoredPosition;
        targetpos = new Vector2(targetpos.x - parentPos.x, targetpos.y - parentPos.y);

        rigid = GetComponent<Rigidbody2D>();

        rigid.AddForce(Random.insideUnitCircle * 2, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        rigid.velocity = Vector2.zero;

        yield return RectTransform.DOAnchorPos(targetpos, 0.3f).SetEase(Ease.Linear).WaitForCompletion();


        //닷트윈 포물선 그리는 함수
        //Vector3 firstPos = transform.position;
        //Vector3 secondPos = firstPos + new Vector3(0.8f, 1.5f, 0);
        //Vector3 thirdPos = new Vector2(-1.03f, 4.7f);

        //Vector3[] path = new[] { secondPos, firstPos + Vector3.down, secondPos + Vector3.down, thirdPos, secondPos + Vector3.up * 2, thirdPos };
        //yield return transform.DOPath(path, 2f, PathType.CubicBezier).SetEase(Ease.InQuad).WaitForCompletion();

        //Destroy(gameObject);
    }
}
