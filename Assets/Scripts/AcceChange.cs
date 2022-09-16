using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceChange : MonoBehaviour
{
    [SerializeField] HatContainer hatcon;
    [SerializeField] GlobeContainer globecon;
    [SerializeField] BootsContainer bootcon;

    public void BuyHat(int number)
    {
        UserData.ItemHat = number;
        SoundManager.Instance.PlaySound(Effect.Click);
        foreach(var tmps in TitleManager.Instance.headEquidTmp)
            tmps.text = "Equid";
        TitleManager.Instance.headEquidTmp[number - 1].text = "Equied";
        UserData.Save();
    }
    public void BuyGlobe(int number)
    {
        UserData.ItemGlobe = number;
        SoundManager.Instance.PlaySound(Effect.Click);
        foreach (var tmps in TitleManager.Instance.GloveEquidTmp)
            tmps.text = "Equid";
        TitleManager.Instance.GloveEquidTmp[number - 1].text = "Equied";
        UserData.Save();
    }
    public void BuyBoots(int number)
    {
        UserData.ItemShose = number;
        SoundManager.Instance.PlaySound(Effect.Click);
        foreach (var tmps in TitleManager.Instance.BootsEquidTmp)
            tmps.text = "Equid";
        TitleManager.Instance.BootsEquidTmp[number - 1].text = "Equied";
        UserData.Save();
    }

    public void TrailColorChange(string color)
    {
        ColorUtility.TryParseHtmlString(color, out Color _color);
        UserData.Color = _color;
        UserData.ColorStr = color;
        SoundManager.Instance.PlaySound(Effect.Click);
        UserData.Save();
    }
}
