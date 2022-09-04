using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HatType
{
    None,
    SantaHat,
    GuardianHat,
    MohicanHat,
    SeorinMetaHat
}

[System.Serializable]
public class Hat : Accessories<HatType>
{

}

[CreateAssetMenu(fileName = "HatContainer", menuName = "")]
public class HatContainer : AccessoriesContainer<Hat, HatType>
{

}
