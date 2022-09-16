using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoSingleton<TowerGenerator>
{
    public Pillar GenerateTower(Transform parent, Vector2 position, PillarType type)
    {
        Pillar pillar = null;

        switch (type)
        {
            case PillarType.None:
                pillar = null;
                break;
            case PillarType.Normal:
                pillar = PoolManager<NormalPillar>.Get(parent, position);
                break;
            case PillarType.Enemy:
                pillar = PoolManager<GuardianPillar>.Get(parent, position);
                break;
            case PillarType.Trap:
                pillar = PoolManager<TrapPillar>.Get(parent, position);
                break;
            case PillarType.Item:
                pillar = PoolManager<ItemPillar>.Get(parent, position);
                break;
            case PillarType.Reverse:
                if (PlayerController.Instance.Height > 200f)
                    pillar = PoolManager<ReversePillar>.Get(parent, position);
                else
                    pillar = PoolManager<NormalPillar>.Get(parent, position);
                break;
            case PillarType.EliteEnemy:
                if (PlayerController.Instance.Height > 500f)
                    pillar = PoolManager<GuardianPillar>.Get(parent, position);
                else
                    pillar = PoolManager<GuardianPillar>.Get(parent, position);
                break;
            default:
                pillar = null;
                break;
        }

        return pillar;
    }
}
