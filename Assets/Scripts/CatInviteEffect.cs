using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CatInviteEffect : MonoBehaviour
{
    public Image whiteScreen;
    public GameObject resultUI;
    public ParticleSystem slowSnow;
    public ParticleSystem fastSnow;
    public void StartEffect()
    {
        StartCoroutine(PlayEffect());
    }
    IEnumerator PlayEffect()
    {
        whiteScreen.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        fastSnow.Stop();
        slowSnow.Play();
        whiteScreen.color = new Color(1, 1, 1, 0);
        gameObject.SetActive(false);
        resultUI.SetActive(true);
    }
}