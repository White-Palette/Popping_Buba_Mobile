using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public enum PillarType
{
    None,
    Normal,
    Enemy,
    //Spring,
    Trap,
    Item,
    Reverse,
    EliteEnemy,
}

[Serializable]
public class PillarMap
{
    public int Count = 1;
    public PillarType LeftPillarType;
    public PillarType RightPillarType;

    public PillarMap(PillarType leftPillarType, PillarType rightPillarType)
    {
        LeftPillarType = leftPillarType;
        RightPillarType = rightPillarType;
    }
}

[CreateAssetMenu(fileName = "MapContainer", menuName = "")]
public class MapContainer : ScriptableObject
{
    public PillarMap[] PillarMaps;

    public PillarMap GetPillarMap()
    {
        int sum = PillarMaps.Sum(x => x.Count);
        int random = Random.Range(0, sum);

        foreach (var pillarMap in PillarMaps)
        {
            if (random > pillarMap.Count)
            {
                random -= pillarMap.Count;
            }
            else
            {
                return pillarMap;
            }
        }
        return null;
    }
}