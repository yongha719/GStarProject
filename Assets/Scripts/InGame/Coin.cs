using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    IEnumerator Start()
    {
        Vector3 firstPos = transform.position;
        Vector3 secondPos = firstPos + new Vector3(0.8f, 1.5f, 0);
        Vector3 thirdPos = new Vector2(-1.03f, 4.7f);

        Vector3[] path = new[] { secondPos, firstPos + Vector3.down, secondPos + Vector3.down, thirdPos, secondPos + Vector3.up * 2, thirdPos };
        yield return transform.DOPath(path, 2f, PathType.CubicBezier).SetEase(Ease.InQuad).WaitForCompletion();

        Destroy(gameObject);
    }

    void Update()
    {

    }
}
