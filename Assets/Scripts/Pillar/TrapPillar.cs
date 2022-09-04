using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPillar : Pillar
{
    public override void TowerEvent()
    {
        SoundManager.Instance.PlaySound(Effect.trap);
        PlayerController.Instance.Dead("Fall");
    }

    public override void Generate()
    {

    }
}
