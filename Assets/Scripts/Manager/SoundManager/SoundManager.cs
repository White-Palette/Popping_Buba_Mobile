using UnityEngine;
using System.Collections;
using System.Linq;

public class SoundManager : MonoSingleton<SoundManager>
{
    private SoundContainer soundContainer;
    private Music currentMusic;
    private AudioObject currentMusicSource;

    private void Awake()
    {
        soundContainer = Resources.Load<SoundContainer>("SoundContainer");
    }

    public void PlaySound(Effect sound, float volume = 1f)
    {
        if (sound == Effect.None)
            return;

        EffectSource soundSource = soundContainer.EffectSources.FirstOrDefault(x => x.Effect == sound);
        if (soundSource == null)
            return;

        PoolManager<AudioObject>.Get(transform).PlayOneShot(soundSource.AudioClip, volume);
    }

    public void PlaySound(Music music)
    {
        if (currentMusicSource == null)
        {
            currentMusicSource = PoolManager<AudioObject>.Get(transform);
        }

        if (music == Music.None)
            return;

        if (currentMusic == music)
            return;

        MusicSource soundSource = soundContainer.MusicSources.FirstOrDefault(x => x.Music == music);

        if (soundSource == null)
            return;

        currentMusic = music;

        currentMusicSource.PlayMusic(soundSource.AudioClip);
    }
}