using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EliteGuardianPillar : Pillar
{
    private EliteGuardian _guardian;
    private KeyCode[] _keyCodes = new KeyCode[3];
    [SerializeField] SpriteRenderer[] _spriteRenderers = new SpriteRenderer[3];

    bool _hit;

    public override void Initialize()
    {
        base.Initialize();
        _guardian.gameObject.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                _keyCodes[i] = KeyCode.RightArrow;
                _spriteRenderers[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (random == 1)
            {
                _keyCodes[i] = KeyCode.LeftArrow;
                _spriteRenderers[i].transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
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

    protected override void Awake()
    {
        base.Awake();
        _guardian = GetComponentInChildren<EliteGuardian>();
    }

    public override void TowerEvent()
    {
        StartCoroutine(DuelCoroutine());
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

        int inputIndex = 0;

        while (!UIManager.Instance.TimingSlider.IsFail)
        {
            if (Input.GetKeyDown(_keyCodes[inputIndex]))
            {
                ++inputIndex;
                _spriteRenderers[inputIndex - 1].color = Color.yellow;
                if (inputIndex == 3)
                {
                    break;
                }
            }
            else if (Input.anyKeyDown)
            {
                for (int i = 0; i < 3; i++)
                {
                    _spriteRenderers[i].color = Color.red;
                }
                inputIndex = 0;
                yield return null;
                continue;
            }
            yield return null;
        }

        var inputTime = UIManager.Instance.TimingSlider.StopMove();

        SoundManager.Instance.PlaySound(Effect.Attack);
        ComboManager.Instance.AddCombo(1);
        _guardian.Hit();
        PlayerController.Instance.PlayerWin();
        if (inputTime == -1f)
        {
            _guardian.Attack2();
            PlayerController.Instance.Dead("Hit");
        }
        else if ((inputTime < PlayerController.Instance.MinVaild || inputTime > PlayerController.Instance.MaxVaild))
        {
            _guardian.Attack1();
            PlayerController.Instance.Dead("Hit");
        }
        else
        {
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
