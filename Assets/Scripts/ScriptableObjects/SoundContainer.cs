using UnityEngine;

[System.Serializable]
public class MusicSource
{
    public Music Music;
    public AudioClip AudioClip;
}

[System.Serializable]
public class EffectSource
{
    public Effect Effect;
    public AudioClip AudioClip;
}

[CreateAssetMenu(fileName = "SoundContainer", menuName = "")]
public class SoundContainer : ScriptableObject
{
    public MusicSource[] MusicSources;
    public EffectSource[] EffectSources;
}