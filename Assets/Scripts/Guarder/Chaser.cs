using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public float Distance => _distance;

    private Animator _animator = null;
    [SerializeField]
    private float _speed = 1f;

    private bool _isAddingSpeed = false;

    private float _distance = 0f;
    private bool _freezed = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Eat()
    {
        _animator.SetTrigger("Bite");
        PlayerController.Instance.Dead("Eat");
    }

    void Update()
    {
        if (_freezed)
            return;

        _distance = Vector2.Distance(transform.position, PlayerController.Instance.transform.position);

        if (_distance < 30f)
        {
            CameraManager.Instance.Noise(0.5f, 15);
            VolumeController.Instance.MotionBlur = 0.5f;
        }
        else
        {
            CameraManager.Instance.Noise(0f, 0f);
            VolumeController.Instance.MotionBlur = 0f;
        }

        if (_distance < 5f)
        {
            Eat();
            VolumeController.Instance.MotionBlur = 1f;
        }
        SetSpeed(2 / PlayerController.Instance.JumpDuration());
        Move();
    }

    public void MoveNearPlayer(float distance)
    {
        if (_freezed)
            return;

        if (_distance < distance)
        {
            Eat();
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, PlayerController.Instance.transform.position, distance);
    }

    public void Move()
    {
        Vector3 playerDir = PlayerController.Instance.transform.position - transform.position;
        playerDir.Normalize();
        transform.position += playerDir * _speed * Time.deltaTime;
    }

    public void AddSpeed()
    {
        if (_isAddingSpeed)
        {
            return;
        }

        _speed += _distance / 80f;
        StartCoroutine(AddSpeedCoroutine());
    }

    public void SetSpeed(float speed)
    {
        if (_speed > speed)
        {
            return;
        }
        _speed = speed;
    }

    public void AddSpeed(float speed)
    {
        if (_isAddingSpeed)
        {
            return;
        }

        _speed += speed;
        StartCoroutine(AddSpeedCoroutine());
    }

    private IEnumerator AddSpeedCoroutine()
    {
        _isAddingSpeed = true;
        yield return new WaitForSeconds(5f);
        _isAddingSpeed = false;
    }

    public void Freeze(float time)
    {
        _freezed = true;
        StartCoroutine(FreezeCoroutine(time));
    }

    private IEnumerator FreezeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        _freezed = false;
    }
}
