using System;
using System.Linq;
using UnityEngine;

public enum GlobeType
{
    None,
    SantaGlobe,
    GuardianGlobe
}

[System.Serializable]
public class Globe : Accessories<GlobeType>
{

}

[CreateAssetMenu(fileName = "GlobeContainer", menuName = "")]
public class GlobeContainer : AccessoriesContainer<Globe, GlobeType>
{

}