using System;
using System.Collections;
using UnityEngine;

public class ComboManager : MonoSingleton<ComboManager>
{
    public int Combo { get; private set; }

    public bool isFired = false;
    private float freezeTime = 0;

    public void AddCombo()
    {
        if (freezeTime > 0)
            return;
        Combo++;
        if (isFired)
        {
            UserData.StageCoin += Combo + 10; // ºÒÅ» ½Ã ÄÚÀÎ 10 Ãß°¡ È¹µæ
        }
        else
        {
            UserData.StageCoin += Combo;
        }
        StartCoroutine(UIManager.Instance.ComboEffect());
        UpdateMaxCombo();
    }

    public void ResetCombo()
    {
        if (freezeTime > 0)
            return;
        UpdateMaxCombo();
        ChaserGenerator.Instance.Chaser?.MoveNearPlayer(10);
        Combo = 0;
    }

    public void UpdateMaxCombo()
    {
        if (Combo > UserData.Cache.MaxCombo)
        {
            UserData.Cache.MaxCombo = Combo;
            UserData.Save();
        }
    }

    public void AddCombo(int combo)
    {
        if (freezeTime > 0)
            return;
        Combo += combo;
    }

    public void FreezeCombo(float time)
    {
        StartCoroutine(FreezeComboCoroutine(time));
    }

    private IEnumerator FreezeComboCoroutine(float time)
    {
        freezeTime = time;
        yield return new WaitForSeconds(time);
        freezeTime = 0;
    }
}