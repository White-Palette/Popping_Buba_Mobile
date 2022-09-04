using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] _particleSystem = null;
    [SerializeField] Effect[] _soundEffect = null;
    [SerializeField] bool _flash = false;
    [SerializeField] bool _chromaticAberration = false;
    [SerializeField] bool _bloom = false;
    [SerializeField] bool _motionBlur = false;
    [SerializeField] Image _flashImage = null;

    public void Play(Vector2 lookPos)
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, lookPos - (Vector2)transform.position);

        foreach (var particle in _particleSystem)
        {
            particle.Play();
        }
        foreach (var sound in _soundEffect)
        {
            SoundManager.Instance.PlaySound(sound);
        }
        if (_flash)
        {
            _flashImage.DOFade(0.5f, 0.1f).From(0f).OnComplete(() =>
            {
                _flashImage.DOFade(0f, 0.1f).From(1f);
            });
        }
        if (_chromaticAberration)
        {
            DOTween.To(() => VolumeController.Instance.ChromaticAberration, x => VolumeController.Instance.ChromaticAberration = x, 1f, 0.15f).From(0f).OnComplete(() =>
            {
                DOTween.To(() => VolumeController.Instance.ChromaticAberration, x => VolumeController.Instance.ChromaticAberration = x, 0f, 0.15f);
            });
        }
        if (_bloom)
        {
            DOTween.To(() => VolumeController.Instance.Bloom, x => VolumeController.Instance.Bloom = x, 10f, 0.15f).From(2f).OnComplete(() =>
            {
                DOTween.To(() => VolumeController.Instance.Bloom, x => VolumeController.Instance.Bloom = x, 2f, 0.15f);
            });
        }
        if (_motionBlur)
        {
            DOTween.To(() => VolumeController.Instance.MotionBlur, x => VolumeController.Instance.MotionBlur = x, 1f, 0.2f).From(0f).OnComplete(() =>
            {
                DOTween.To(() => VolumeController.Instance.MotionBlur, x => VolumeController.Instance.MotionBlur = x, 0f, 0.2f);
            });
        }
    }
}
