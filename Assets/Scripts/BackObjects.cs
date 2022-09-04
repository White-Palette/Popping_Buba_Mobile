using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackObjects : MonoBehaviour
{
    [SerializeField] float multiplier = 1f;

    private SeasonContainer seasonContainer = null;

    private List<SpriteRenderer> _topSpriteRenderer = null;
    private List<SpriteRenderer> _bodySpriteRenderer = null;

    private void Awake()
    {
        seasonContainer = Resources.Load<SeasonContainer>("SeasonContainer");
        _topSpriteRenderer = new List<SpriteRenderer>();
        _bodySpriteRenderer = new List<SpriteRenderer>();

        foreach (Transform child in transform)
        {
            _bodySpriteRenderer.Add(child.GetChild(0).GetComponent<SpriteRenderer>());
            _topSpriteRenderer.Add(child.GetChild(1).GetComponent<SpriteRenderer>());
        }

        SeasonManager.Instance.OnSeasonChanged += ChangeColor;
    }

    private void Update()
    {
        Vector2 pos = Camera.main.transform.position;
        pos *= multiplier;
        pos *= -1;
        transform.localPosition = pos;

        foreach (Transform child in transform)
        {
            Vector2 childPos = child.position - Camera.main.transform.position;

            float value = 10f;

            if (childPos.x < -value)
            {
                child.localPosition += new Vector3(value * 2, 0, 0);
            }

            if (childPos.x > value)
            {
                child.localPosition -= new Vector3(value * 2, 0, 0);
            }

            if (childPos.y < -value)
            {
                child.localPosition += new Vector3(0, value * 2, 0);
            }

            if (childPos.y > value)
            {
                child.localPosition -= new Vector3(0, value * 2, 0);
            }
        }
    }

    private void ChangeColor(Season type, float duration = 1.5f)
    {
        for (int i = 0; i < _topSpriteRenderer.Count; ++i)
        {
            _topSpriteRenderer[i].DOColor(new Color(seasonContainer.GetSeasonEffect(type)._topColor.r - 0.5f, seasonContainer.GetSeasonEffect(type)._topColor.g - 0.5f, seasonContainer.GetSeasonEffect(type)._topColor.b - 0.5f, 0.2f), duration);
            _bodySpriteRenderer[i].DOColor(new Color(seasonContainer.GetSeasonEffect(type)._bodyColor.r - 0.5f, seasonContainer.GetSeasonEffect(type)._bodyColor.g - 0.5f, seasonContainer.GetSeasonEffect(type)._bodyColor.b - 0.5f, 0.35f), duration);
        }
    }
}
