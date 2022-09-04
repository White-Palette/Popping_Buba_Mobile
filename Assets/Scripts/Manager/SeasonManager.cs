using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SeasonManager : MonoSingleton<SeasonManager>
{
    private SeasonContainer seasonContainer = null;
    private List<Pillar> _towers = null;
    private Season _currentSeason = Season.None;
    private int _currentSeasonIndex = 0;
    private GameObject _currentSeasonEffect = null;
    private AudioObject _currentSeasonMusic = null;

    public Action<Season, float> OnSeasonChanged = null;

    private void Awake()
    {
        seasonContainer = Resources.Load<SeasonContainer>("SeasonContainer");
        _towers = new List<Pillar>();
        _currentSeason = Season.None;
    }

    private IEnumerator Start()
    {
        ChangeSeason(Season.Winter);
        while (true)
        {
            yield return null;
            if ((int)(PlayerController.Instance.Height / 300f) > _currentSeasonIndex)
            {
                ++_currentSeasonIndex;
                ChangeSeason();
            }
        }
    }

    public void ChangeSeason()
    {
        if (_currentSeason == Season.Spring)
        {
            _currentSeason = Season.Summer;
        }
        else if (_currentSeason == Season.Summer)
        {
            _currentSeason = Season.Autumn;
        }
        else if (_currentSeason == Season.Autumn)
        {
            _currentSeason = Season.Winter;
        }
        else if (_currentSeason == Season.Winter)
        {
            _currentSeason = Season.Spring;
        }
        else if (_currentSeason == Season.None)
        {
            _currentSeason = (Season)UnityEngine.Random.Range(1, 5);
        }
        else
        {
            Debug.LogError("Season is invalid");
        }

        ChangeBackgroundColor();

        ChangeEffect();

        ChangeTowerColor();

        OnSeasonChanged(_currentSeason, 1.5f);

        ChangeMusic();
    }

    public void ChangeSeason(Season season)
    {
        if (_currentSeason == season) return;

        _currentSeason = season;

        ChangeBackgroundColor();

        ChangeEffect();

        ChangeTowerColor();

        OnSeasonChanged(_currentSeason, 1.5f);

        ChangeMusic();
    }

    public void AddTower(Pillar tower)
    {
        if (_towers == null)
        {
            _towers = new List<Pillar>();
        }

        if (_towers.Find(x => x == tower) == null)
        {
            _towers.Add(tower);
            ChangeSeason(_currentSeason);
        }
    }

    private void ChangeBackgroundColor()
    {
        Camera.main.DOColor(seasonContainer.GetSeasonEffect(_currentSeason)._backgroundColor, 1.5f);
    }

    private void ChangeEffect()
    {
        if (seasonContainer.GetSeasonEffect(_currentSeason).Effect != null)
        {
            /*
            if (_currentSeasonEffect != null)
            {
                return;
            }*/
            Destroy(_currentSeasonEffect);
            _currentSeasonEffect = Instantiate(seasonContainer.GetSeasonEffect(_currentSeason).Effect, Camera.main.transform);
            _currentSeasonEffect.transform.localPosition = new Vector3(-2.95f, 7.9f, 9f);
            _currentSeasonEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
            _currentSeasonEffect.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            if (_currentSeasonEffect != null)
            {
                Destroy(_currentSeasonEffect);
            }
            _currentSeasonEffect = null;
        }
    }

    private void ChangeTowerColor()
    {
        foreach (var tower in _towers)
        {
            if (tower.GetTopColor() != seasonContainer.GetSeasonEffect(_currentSeason)._topColor && tower.gameObject.activeSelf)
            {
                tower.SetTopColor(seasonContainer.GetSeasonEffect(_currentSeason)._topColor, 1.5f);
            }

            if (tower.GetBodyColor() != seasonContainer.GetSeasonEffect(_currentSeason)._bodyColor && tower.gameObject.activeSelf)
            {
                tower.SetBodyColor(seasonContainer.GetSeasonEffect(_currentSeason)._bodyColor, 1.5f);
            }
        }
    }

    private void ChangeMusic()
    {
        Music music = seasonContainer.GetSeasonEffect(_currentSeason).Music;
        SoundManager.Instance.PlaySound(music);
    }

    public SeasonEffect GetSeasonEffect()
    {
        return seasonContainer.GetSeasonEffect(_currentSeason);
    }
}
