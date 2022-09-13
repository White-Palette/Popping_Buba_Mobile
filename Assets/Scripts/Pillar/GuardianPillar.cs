using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GuardianPillar : Pillar
{
    private Guardian _guardian = null;

    float _inputValue = 0f;

    bool _hit = false;

    protected override void Awake()
    {
        base.Awake();
        _guardian = GetComponentInChildren<Guardian>();
    }

    public override void Initialize()
    {
        base.Initialize();
        _guardian.gameObject.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
        
        if(!_hit)
        {
            if (transform.position.x < PlayerController.Instance.transform.position.x)
                _guardian.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            else
                _guardian.transform.localScale = new Vector3(-1.2f, 1.2f, 1);
        }
    }

    public override void TowerEvent()
    {
        StartCoroutine(nameof(DuelCoroutine));
    }

    private IEnumerator DuelCoroutine()
    {
        while (Vector3.Distance(transform.position + Vector3.up * 1.7f, PlayerController.Instance.transform.position) > 2f)
        {
            yield return null;
        }

        var test = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();

        Time.timeScale = 0.1f;
        DOTween.To(() => test.m_Lens.OrthographicSize, x => test.m_Lens.OrthographicSize = x, 1, 0.5f);

        UIManager.Instance.TimingSlider.StartMove();
        UIManager.Instance.TimingSlider.MoveTo(_guardian.transform.position + new Vector3(0, -0.5f, 0));

        while (!UIManager.Instance.TimingSlider.IsFail)
        {
            if (Input.GetKeyDown(KeyCode.Space) || PlayerController.Instance.IsLeftTouch || PlayerController.Instance.IsRightTouch)
            {
                break;
            }
            yield return null;
        }

        _inputValue = UIManager.Instance.TimingSlider.StopMove();

        if (_inputValue == -1f)
        {
            _guardian.Attack2();
            PlayerController.Instance.Dead("Hit");
        }
        else if (_inputValue < PlayerController.Instance.MinVaild || _inputValue > PlayerController.Instance.MaxVaild)
        {
            _guardian.Attack1();
            PlayerController.Instance.Dead("Hit");
        }
        else
        {
            Debug.Log("inputValue : " + _inputValue);
            SoundManager.Instance.PlaySound(Effect.Attack);
            ComboManager.Instance.AddCombo(1);
            _guardian.Hit();
            _hit = true;
            PlayerController.Instance.PlayerWin();
        }

        Time.timeScale = 1f;
        DOTween.To(() => test.m_Lens.OrthographicSize, x => test.m_Lens.OrthographicSize = x, 5, 0.5f);
        CameraManager.Instance.Noise(0.5f, 15f);
        yield return new WaitForSeconds(0.2f);
        CameraManager.Instance.Noise(0, 0);
    }
}
