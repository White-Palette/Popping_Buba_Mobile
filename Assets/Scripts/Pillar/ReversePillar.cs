using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversePillar : Pillar
{
    public override void Initialize()
    {
        base.Initialize();
        StartCoroutine(ReversePillarCoroutine());
    }

    private IEnumerator ReversePillarCoroutine()
    {
        while (true)
        {
            yield return null;
            if (PlayerController.Instance.currentPillar == this)
            {
                PlayerController.Instance.Reverse = true;
                break;
            }
        }
    }
}
