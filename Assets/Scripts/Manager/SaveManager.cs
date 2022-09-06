using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SaveManager : MonoSingleton<SaveManager>
{
    int coin;
    public int Coin { get { return coin; } private set { } }

    int max_combo;
    public int Combo { get { return max_combo; } set { max_combo = value; } }

    private void Awake()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        max_combo = PlayerPrefs.GetInt("combo", 0);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.SetInt("combo", max_combo);
    }

    public void AddCoin(int add)
    {
        coin += add;
        UIManager.Instance.CoinEffect();
    }
}
