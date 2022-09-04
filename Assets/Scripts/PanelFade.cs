using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelFade : MonoBehaviour
{
    [SerializeField] GameObject panel;
    private bool isSettingEnable = false;

    private void Start()
    {
        SoundManager.Instance.PlaySound(Music.Wonderful_Story_Alt_Version);   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            Panel();
    }

    public void Panel()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        StartCoroutine(TogglePanel(panel));
    }

    IEnumerator TogglePanel(GameObject Panel)
    {
        //if (!DOTween.Sequence(Panel.transform).IsPlaying()) yield break;
        if (isSettingEnable)
        {
            Panel.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f).From(1f);
            isSettingEnable = false;
            yield return new WaitForSeconds(0.3f);
            Panel.SetActive(false);
        }
        else
        {
            Panel.SetActive(true);
            Panel.transform.DOScale(new Vector3(0.9f, 0.9f, 0f), 1f).SetEase(Ease.OutBounce).From(0f);
            isSettingEnable = true;
        }
        
    }
}
