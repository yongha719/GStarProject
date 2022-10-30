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
    private Animator animator;
    public void StartEffect()
    {
        StartCoroutine(PlayEffect());
    }
    public void PressBtn()
    {
        animator = GetComponent<Animator>();
        animator.Play("ShadowMove"); 
        fastSnow.Play();
        slowSnow.Stop();
    }
    IEnumerator PlayEffect()
    {
        whiteScreen.DOFade(1, 1);
        yield return new WaitForSeconds(1);
        fastSnow.Stop();
        slowSnow.Play();
        whiteScreen.color = new Color(1, 1, 1, 0);
        resultUI.SetActive(true);
    }
}
