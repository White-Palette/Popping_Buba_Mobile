using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _maxCombo;
    [SerializeField] TextMeshProUGUI _heightTMP;
    [SerializeField] TextMeshProUGUI _coinTMP;

    [SerializeField] TextMeshProUGUI _textBest;

    [SerializeField] Light2D light2d;

    private bool isLoading = false;

    private void Start()
    {
        //_heightTMP.text = $"{UserData.Cache.Height:0.0}m";
        //_maxCombo.text = $"{UserData.Cache.MaxCombo}";
        isLoading = false;
        UserData.Load();
        UserData.StageCoin += (UserData.Cache.MaxCombo / 2);
        Fade.Instance.FadeIn();
        StartCoroutine(HeightRecords());
    }

    private void Update()
    {
        KeyDown();
    }
    void KeyDown()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isLoading)
        {
            isLoading = true;
            Fade.Instance.FadeOutToGameScene();
        }
        else if (Input.GetKeyDown(KeyCode.M) && !isLoading)
        {
            isLoading = true;
            Fade.Instance.FadeOutToMainMenu();
        }
    }

    IEnumerator HeightRecords()
    {
        int i = 0;
        float randomHeight = 0;
        int randomCombo = 0;
        int randomCoin = 0;

        _textBest.gameObject.SetActive(false);
        while (i < 35)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
                break;

            randomHeight = Random.Range(0f, 999f);
            _heightTMP.text = $"{randomHeight:0.0}m";

            randomCombo = Random.Range(0, 200);
            _maxCombo.text = $"{randomCombo}";

            randomCoin = Random.Range(0, 9999);
            _coinTMP.text = $"+{randomCoin}";

            yield return new WaitForSeconds(0.05f);
            ++i;
        }

        _heightTMP.text = $"{UserData.Cache.Height:0.0}m";
        _heightTMP.transform.DOScale(1f, 1.5f).SetEase(Ease.OutCirc).From(3.0f);
        _maxCombo.text = $"{UserData.Cache.MaxCombo}";
        _maxCombo.transform.DOScale(1f, 1.5f).SetEase(Ease.OutCirc).From(3.0f);
        _coinTMP.text = $"+{UserData.StageCoin}";
        _coinTMP.transform.DOScale(1f, 1.5f).SetEase(Ease.OutCirc).From(3.0f);

        UserData.Coin += UserData.StageCoin;
        UserData.StageCoin = 0;

        yield return new WaitForSeconds(0.5f);

        if (UserData.Cache.Height > UserData.Record.Height&& UserData.Cache.MaxCombo > UserData.Record.MaxCombo)
        {
            _textBest.gameObject.SetActive(true);
            UserData.Record.Height = UserData.Cache.Height;
            UserData.Record.MaxCombo = UserData.Cache.MaxCombo;
            UserData.Save();
            _textBest.transform.DOScale(1f, 1.5f).SetEase(Ease.OutCirc).From(2.0f);
        }
    }
}
