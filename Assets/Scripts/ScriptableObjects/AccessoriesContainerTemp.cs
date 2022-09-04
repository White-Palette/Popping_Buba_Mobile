using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class Accessories<T> where T : Enum
{
    public T Accessory;
    public Sprite Sprite;
    public float ComboDuration;
    public float validValue;
    public float Speed;
}

public abstract class AccessoriesContainer<T1, T2> : ScriptableObject where T1 : Accessories<T2> where T2 : Enum
{
    public List<T1> Accessories;

    public T1 GetAccessory(T2 accessory)
    {
        return Accessories.FirstOrDefault(x => (Enum)x.Accessory == (Enum)accessory);
    }
}
