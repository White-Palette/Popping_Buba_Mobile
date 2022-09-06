using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private TimingSlider _timingSlider = null;

    [SerializeField]
    private TMP_Text _heightText = null;

    [SerializeField]
    private TextMeshProUGUI _comboText = null;

    [SerializeField]
    private Image _PauseImage = null;

    [SerializeField]
    private Light2D light2d;

    [SerializeField]
    private Image _chaserIcon = null;

    [SerializeField]
    private TMP_Text _distanceText = null;

    [SerializeField]
    private TextMeshProUGUI _coinText = null;

    public TimingSlider TimingSlider
    {
        get
        {
            if (_timingSlider == null)
            {
                _timingSlider = FindObjectOfType<TimingSlider>();

                if (_timingSlider == null)
                {
                    Debug.LogError("TimingSlider is not found.");
                }
            }
            return _timingSlider;
        }
    }

    private IEnumerator Start()
    {
        yield return null;
        UserData.Load();
        _timingSlider.gameObject.SetActive(false);
        _PauseImage.gameObject.SetActive(false);
        _heightText.gameObject.SetActive(true);
    }

    private void Update()
    {
        _heightText.text = $"{PlayerController.Instance.Height:0.0}m";
        _comboText.text = $"{ComboManager.Instance.Combo} Combo";
        _coinText.text = $"{SaveManager.Instance.Coin}$";
        if (ChaserGenerator.Instance.Chaser != null)
        {
            if (_chaserIcon.enabled == false)
            {
                _chaserIcon.enabled = true;
                _chaserIcon.DOFade(1, 0.5f);
            }
            _distanceText.text = $"{ChaserGenerator.Instance.Chaser.Distance:0.0}m";
        }
        else
        {
            if (_chaserIcon.enabled == true)
            {
                _chaserIcon.DOFade(0, 0f);
                _chaserIcon.enabled = false;
            }
            _distanceText.text = "";
        }
    }

    public void SetPauseImage(bool isActive)
    {
        _PauseImage.gameObject.SetActive(isActive);
    }

    public IEnumerator ComboEffect()
    {
        _comboText.transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.2f);
        _comboText.transform.DOScale(new Vector2(1f, 1f), 0.2f).SetEase(Ease.OutSine);

        yield break;
    }

    public IEnumerator CoinEffect()
    {
        _coinText.transform.DOScale(new Vector2(1.2f, 1.2f), 0.2f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.2f);
        _coinText.transform.DOScale(new Vector2(1f, 1f), 0.2f).SetEase(Ease.OutSine);

        yield break;
    }

    public void FireEffect(GameObject fireEffect)
    {
        fireEffect.SetActive(true);
    }

}
