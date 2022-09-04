using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundVolumeController : MonoBehaviour
{
    private AudioMixer _audioMixer = null;

    [SerializeField]
    private Slider _masterSoundSlider = null;

    [SerializeField]
    private Slider _musicSoundSlider = null;

    [SerializeField]
    private Slider _effectSoundSlider = null;

    private void Awake()
    {
        _audioMixer = Resources.Load<AudioMixer>("Mixer/AudioMixer");
    }

    public void MasterAudioControl()
    {
        float soundVolume = _masterSoundSlider.value;

        if (soundVolume == -40)
        {
            soundVolume = -80;
        }
        _audioMixer.SetFloat("Master", soundVolume);
    }

    public void MusicAudioControl()
    {
        float soundVolume = _musicSoundSlider.value;

        if (soundVolume == -40)
        {
            soundVolume = -80;
        }
        _audioMixer.SetFloat("Music", soundVolume);
    }

    public void EffectAudioControl()
    {
        float soundVolume = _effectSoundSlider.value;

        if (soundVolume == -40)
        {
            soundVolume = -80;
        }
        _audioMixer.SetFloat("Effect", soundVolume);
    }
}
