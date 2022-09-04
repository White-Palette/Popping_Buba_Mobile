using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BootsType
{
    None,
    SantaBoots,
    IronBoots,
    HedgehogBoots
}

[System.Serializable]
public class Boots : Accessories<BootsType>
{

}

[CreateAssetMenu(fileName = "BootsContainer", menuName = "")]
public class BootsContainer : AccessoriesContainer<Boots, BootsType>
{

}