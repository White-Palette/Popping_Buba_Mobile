using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RealtimeLeaderboardEntry : MonoBehaviour, IPoolable
{
    [SerializeField] TMP_Text NameText = null;
    [SerializeField] TMP_Text HeightText = null;
    [SerializeField] SpriteRenderer[] spriteRenderers = null;

    private float _height = 0;

    private CanvasGroup _canvasGroup = null;
    private Image _image;
    private bool _isFired = false;

    public void Initialize()
    {
        Color = Color.white;
        NameText.text = "";
        HeightText.text = "";
        _canvasGroup.DOFade(1f, 0.5f).From(0f);
    }

    public bool IsFired
    {
        get => _isFired;
        set
        {
            if (_isFired == value) return;
            

            if (value)
            {
                foreach(var spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.gameObject.SetActive(true);
                    spriteRenderer.DOFade(1f, 0.5f).From(0f);
                }
            }
            else
            {
                foreach (var spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.DOFade(0f, 0.5f).From(1f);
                    spriteRenderer.gameObject.SetActive(false);
                }
            }

            _isFired = value;

        }
    }

    public string Name
    {
        get => NameText.text;
        set => NameText.text = value;
    }

    public float Height
    {
        get => _height;
        set
        {
            if (_height == value)
                return;
            DOTween.Kill(_height);
            DOTween.To(() => _height, x => _height = x, value, 0.1f);
            HeightText.text = _height.ToString("0.0") + "m";
        }
    }

    public Color Color 
    {
        get => _image.color;
        set => _image.color = value;
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
}
