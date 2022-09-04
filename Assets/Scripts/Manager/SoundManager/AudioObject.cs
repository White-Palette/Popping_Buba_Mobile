using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioObject : MonoBehaviour, IPoolable
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        audioSource.clip = null;
        audioSource.outputAudioMixerGroup = null;
        audioSource.mute = false;
        audioSource.bypassEffects = false;
        audioSource.bypassListenerEffects = false;
        audioSource.bypassReverbZones = false;
        audioSource.playOnAwake = false;
        // audioSource.loop = false;
        // audioSource.priority = 128;
        // audioSource.volume = 1f;
        // audioSource.pitch = 1f;
        // audioSource.panStereo = 0f;
        // audioSource.spatialBlend = 1f;
        // audioSource.reverbZoneMix = 1f;
    }

    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer/AudioMixer").FindMatchingGroups("Effect")[0];
        audioSource.PlayOneShot(clip, volume);
        StartCoroutine(DestroyAfter(clip.length));
    }

    public void PlayMusic(AudioClip clip, float volume = 1f)
    {
        audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("Mixer/AudioMixer").FindMatchingGroups("Music")[0];
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    private IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager<AudioObject>.Release(this);
    }
}