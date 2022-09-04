using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardEntry : MonoBehaviour, IPoolable
{
    [SerializeField] TMP_Text NameText = null;
    [SerializeField] TMP_Text ComboText = null;
    [SerializeField] TMP_Text HeightText = null;

    private float _height = 0;
    private int _combo = 0;

    public void Initialize()
    {
        NameText.text = "";
        ComboText.text = "";
        HeightText.text = "";
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
            _height = value;
            HeightText.text = _height.ToString("0.0") + "m";
        }
    }

    public int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            ComboText.text = _combo.ToString();
        }
    }
}
