using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeController : MonoSingleton<VolumeController>
{
    public VolumeProfile Profile { get; private set; }

    private void Awake()
    {
        Profile = GetComponent<Volume>().profile;
    }

    public float ChromaticAberration 
    {
        get
        {
            Profile.TryGet(out ChromaticAberration chromaticAberration);
            return chromaticAberration.intensity.value;
        }
        set
        {
            Profile.TryGet(out ChromaticAberration chromaticAberration);
            chromaticAberration.intensity.value = value;
        }
    }

    public float Bloom
    {
        get
        {
            Profile.TryGet(out Bloom bloom);
            return bloom.intensity.value;
        }
        set
        {
            Profile.TryGet(out Bloom bloom);
            bloom.intensity.value = value;
        }
    }

    public float MotionBlur
    {
        get
        {
            Profile.TryGet(out MotionBlur motionBlur);
            return motionBlur.intensity.value;
        }
        set
        {
            Profile.TryGet(out MotionBlur motionBlur);
            // motionBlur.intensity.value = value;
        }
    }
}
