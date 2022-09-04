using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

[Serializable]
public class Range
{
    public float min;
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}

public abstract class Pillar : MonoBehaviour, IPoolable
{
    public Pillar LeftPillar;
    public Pillar RightPillar;
    public Range HorizontalRange = new Range(3, 5);
    public Range VerticalRange = new Range(3, 5);

    [SerializeField] SpriteRenderer topSprite = null;
    [SerializeField] SpriteRenderer bodySprite = null;

    private SpriteRenderer overlaySprite = null;
    private MapContainer mapContainer = null;

    protected virtual void Awake()
    {
        overlaySprite = GetComponent<SpriteRenderer>();
        mapContainer = Resources.Load<MapContainer>("MapContainer");
    }

    public virtual void TowerEvent()
    {

    }

    public virtual void Generate()
    {
        if (LeftPillar != null || RightPillar != null)
            return;

        var map = mapContainer.GetPillarMap();

        Vector2 rightPillarPosition = transform.position + Vector3.right * Random.Range(HorizontalRange.min, HorizontalRange.max);
        rightPillarPosition = rightPillarPosition + Vector2.up * Random.Range(VerticalRange.min, VerticalRange.max);
        RightPillar = TowerGenerator.Instance.GenerateTower(transform.parent, rightPillarPosition, map.RightPillarType);

        Vector2 leftPillarPosition = transform.position + Vector3.left * Random.Range(HorizontalRange.min, HorizontalRange.max);
        leftPillarPosition = leftPillarPosition + Vector2.up * Random.Range(VerticalRange.min, VerticalRange.max);
        LeftPillar = TowerGenerator.Instance.GenerateTower(transform.parent, leftPillarPosition, map.LeftPillarType);
    }

    protected virtual void Update()
    {
        if (transform.position.y - Camera.main.transform.position.y < -10f)
        {
            PoolManager<Pillar>.Release(this, true);
        }
    }

    public virtual void Initialize()
    {
        LeftPillar = null;
        RightPillar = null;
        overlaySprite.color = new Color(0, 0, 0, 0);
        transform.DOMoveY(transform.position.y, 0.2f).From(transform.position.y - 1f);
        topSprite.DOFade(1f, 0.2f).From(0f);
        bodySprite.DOFade(1f, 0.2f).From(0f);
        topSprite.color = SeasonManager.Instance.GetSeasonEffect()._topColor;
        bodySprite.color = SeasonManager.Instance.GetSeasonEffect()._bodyColor;
        SeasonManager.Instance.AddTower(this);
    }

    public void SetTopColor(Color color, float duration)
    {
        topSprite.DOColor(color, duration);
    }

    public void SetTopColor(Color color)
    {
        topSprite.color = color;
    }

    public Color GetTopColor()
    {
        return topSprite.color;
    }

    public void SetBodyColor(Color color, float duration)
    {
        bodySprite.DOColor(color, duration);
    }

    public void SetBodyColor(Color color)
    {
        bodySprite.color = color;
    }

    public Color GetBodyColor()
    {
        return bodySprite.color;
    }

    public virtual void Disable()
    {
        overlaySprite.DOColor(new Color(0f, 0f, 0f, 0.5f), 0.2f);
    }

    public void Enable()
    {
        overlaySprite.DOColor(new Color(0f, 0f, 0f, 0f), 0.2f);
    }

    private void OnDisable()
    {
        DOTween.Kill(overlaySprite);
        DOTween.Kill(topSprite);
        DOTween.Kill(bodySprite);
    }
}
