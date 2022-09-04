using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Fade : MonoSingleton<Fade>
{
    [SerializeField] Image fadeImg;

    int sceneLoad = 0;
    float fadeTime = 0.75f;
    public static bool isTutoMap { get; set; }

    private void Start()
    {
        fadeImg.gameObject.SetActive(true);
        FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine());
    }

    public IEnumerator FadeInCoroutine()
    {
        fadeImg.fillOrigin = 0;
        fadeImg.DOFillAmount(0f, fadeTime).SetEase(Ease.InQuad).From(1f);
        yield return new WaitForSeconds(1f);
        fadeImg.raycastTarget = false;
    }

    public void FadeOutToMainMenu()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        MouseManager.Show(true);
        MouseManager.Lock(false);
        sceneLoad = 1;
        StartCoroutine(FadeOutCoroutine(sceneLoad));
    }

    public void FadeOutToGameScene()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        MouseManager.Show(false);
        MouseManager.Lock(true);
        sceneLoad = 2;
        StartCoroutine(FadeOutCoroutine(sceneLoad));
    }

    public void FadeOutToGameOverScene()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        MouseManager.Show(true);
        MouseManager.Lock(false);
        sceneLoad = 3;
        StartCoroutine(FadeOutCoroutine(sceneLoad));
    }

    public void FadeOutToTutorial()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        MouseManager.Show(false);
        MouseManager.Lock(true);
        sceneLoad = 4;
        StartCoroutine(FadeOutCoroutine(sceneLoad));
    }


    public IEnumerator FadeOutCoroutine(int sceneLoad)
    {
        if (sceneLoad == 4) isTutoMap = true;
        else isTutoMap = false;
        fadeImg.raycastTarget = true;
        fadeImg.fillOrigin = 1;
        fadeImg.DOFillAmount(1f, fadeTime).SetEase(Ease.InQuad).From(0f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneLoad);
        yield break;
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
