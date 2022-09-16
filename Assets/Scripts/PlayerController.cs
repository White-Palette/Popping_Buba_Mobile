using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoSingleton<PlayerController>
{
    public Pillar currentPillar = null;
    [SerializeField] AnimationCurve jumpCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] AnimationCurve speedCurve = AnimationCurve.EaseInOut(1, 1, 0, 0);
    [SerializeField] ParticleSystem landing;
    [SerializeField] TrailRenderer trail;
    [SerializeField] EffectController effectController;
    [SerializeField] HatContainer HatSprite;
    [SerializeField] GlobeContainer GlobeSprite;
    [SerializeField] BootsContainer BootsSprite;

    [SerializeField] SpriteRenderer hat;
    [SerializeField] SpriteRenderer leftArm;
    [SerializeField] SpriteRenderer rightArm;
    [SerializeField] SpriteRenderer leftLeg;
    [SerializeField] SpriteRenderer rightLeg;

    public float MinVaild { get; set; }
    public float MaxVaild { get; set; }

    public bool Reverse = false;

    private ParticleSystem particle;

    private Animator animator;
    private bool isMoving = false;
    private float waitTime = 0;
    private float _height = 0f;
    private bool isDead = false;
    private bool isColorSeted = false;
    private float resetTime;
    private float perfactTime;
    private float combo = 0;
    private float vaild = 0;
    public float speed = 0;
    private int guard = 0;

    public bool IsLeftTouch = false;
    public bool IsRightTouch = false;

    public void LeftTouch()
    {
        IsLeftTouch = true;
        StartCoroutine(LeftTouchCoroutine());
    }

    public void RightTouch()
    {
        IsRightTouch = true;
        StartCoroutine(RightTouchCoroutine());
    }

    private IEnumerator LeftTouchCoroutine()
    {
        yield return null;
        IsLeftTouch = false;
    }

    private IEnumerator RightTouchCoroutine()
    {
        yield return null;
        IsRightTouch = false;
    }

    private void Awake()
    {
        ServerManager.Instance.OnConnected += () =>
        {
            StartCoroutine(Connected());
        };
    }

    private IEnumerator Connected()
    {
        while (true)
        {
            ServerManager.Instance.SendHeight(_height, ComboManager.Instance.Combo);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Start()
    {
        ItemChange();
        animator = GetComponent<Animator>();
        particle = transform.Find("Hit").GetComponent<ParticleSystem>();
        animator.SetBool("IsJump", true);
        MoveToPillar(currentPillar);
        UserData.StageCoin = 0;
    }

    private void Update()
    {
        if (!isMoving && !isDead)
        {
            bool isLeft = IsLeftTouch;
            bool isRight = IsRightTouch;

            if (Reverse)
            {
                bool temp = isLeft;
                isLeft = isRight;
                isRight = temp;
            }

            if (isLeft || isRight)
            {
                if (isRight)
                {
                    if (currentPillar.RightPillar != null)
                    {
                        MoveToPillar(currentPillar.RightPillar);
                        transform.localScale = new Vector3(0.8f, 0.8f, 1);
                        animator.SetBool("IsJump", true);
                    }
                    else if (currentPillar.RightPillar == null)
                    {
                        transform.localScale = new Vector3(0.8f, 0.8f, 1);
                        Dead("Miss");
                    }
                    Reverse = false;
                }
                else if (isLeft)
                {
                    if (currentPillar.LeftPillar != null)
                    {
                        MoveToPillar(currentPillar.LeftPillar);
                        transform.localScale = new Vector3(-0.8f, 0.8f, 1);
                        animator.SetBool("IsJump", true);
                    }
                    else if (currentPillar.LeftPillar == null)
                    {
                        transform.localScale = new Vector3(-0.8f, 0.8f, 1);
                        Dead("Miss");
                    }
                    Reverse = false;
                }
            }

            waitTime += Time.deltaTime;

            if (waitTime > resetTime)
            {
                ComboManager.Instance.ResetCombo();
            }
        }
    }

    public void TrailColor(Color value)
    {
        if (isColorSeted) return;
        trail.startColor = value;
        trail.endColor = value - new Color(0, 0, 0, 255);
        isColorSeted = true;
    }

    public void PlayerWin()
    {
        effectController.Play(currentPillar.transform.position + Vector3.up * 5f);
        DOTween.Complete(transform, true);
    }

    public void MoveToPillar(Pillar pillar)
    {
        isMoving = true;

        if (currentPillar.LeftPillar != null)
        {
            if (currentPillar.LeftPillar != pillar)
            {
                currentPillar.LeftPillar.Disable();
            }
        }
        if (currentPillar.RightPillar != null)
        {
            if (currentPillar.RightPillar != pillar)
            {
                currentPillar.RightPillar.Disable();
            }
        }

        ShowJudgmentTime(waitTime);
        currentPillar.Disable();
        currentPillar = pillar;
        SoundManager.Instance.PlaySound(Effect.Jump);

        currentPillar.TowerEvent();
        currentPillar.Generate();

        if (waitTime < perfactTime)
        {
            ComboManager.Instance.AddCombo();
        }

        transform.DOJump(pillar.transform.position + Vector3.up * 1.7f, 2f, 1, JumpDuration()).SetEase(jumpCurve).OnComplete(() =>
        {
            isMoving = false;
            animator.SetBool("IsJump", false);
            landing.transform.position = gameObject.transform.position;
            landing.Play();
            waitTime = 0;
        });
    }

    private void ShowJudgmentTime(float time)
    {
        var text = PoolManager<OverlayText>.Get(transform.parent);
        text.Text.text = (int)(time * 1000) + "ms";
        text.MoveTo(currentPillar.transform.position + Vector3.up * 2f);
    }

    public float JumpDuration()
    {
        animator.speed = Mathf.Clamp(1 + ComboManager.Instance.Combo / 50f, 1, 2);
        return speedCurve.Evaluate(ComboManager.Instance.Combo / 50f) - ((speed * Mathf.Clamp(ComboManager.Instance.Combo, 0, 50)) / 50f);
    }

    public float Height
    {
        get
        {
            _height = transform.position.y;
            if (_height < 0f)
            {
                _height = 0f;
            }
            return _height;
        }
    }

    public void Dead(string str)
    {
        if (str == "Hit")
        {
            if (guard > 0)
            {
                guard--;
                return;
            }
        }

        if (isDead) return;

        isDead = true;
        animator.SetTrigger(str);
        particle.Play();
        ComboManager.Instance.UpdateMaxCombo();
        UserData.Cache.Height = Height;
        UserData.Cache.MaxCombo = ComboManager.Instance.MaxCombo;
        UserData.Coin += UserData.StageCoin;
        UserData.Save();
        SoundManager.Instance.PlaySound(Effect.Die);
        if (!Fade.isTutoMap) Fade.Instance.FadeOutToGameOverScene();
        else Fade.Instance.FadeOutToMainMenu();
    }

    public void ItemChange()
    {
        hat.sprite = HatSprite.Accessories[UserData.ItemHat].Sprite;
        leftArm.sprite = GlobeSprite.Accessories[UserData.ItemGlobe].Sprite;
        rightArm.sprite = GlobeSprite.Accessories[UserData.ItemGlobe].Sprite;
        leftLeg.sprite = BootsSprite.Accessories[UserData.ItemShose].Sprite;
        rightLeg.sprite = BootsSprite.Accessories[UserData.ItemShose].Sprite;
        combo = HatSprite.Accessories[UserData.ItemHat].ComboDuration + GlobeSprite.Accessories[UserData.ItemGlobe].ComboDuration + BootsSprite.Accessories[UserData.ItemShose].ComboDuration;
        vaild = HatSprite.Accessories[UserData.ItemHat].validValue + GlobeSprite.Accessories[UserData.ItemGlobe].validValue + BootsSprite.Accessories[UserData.ItemShose].validValue;
        speed = HatSprite.Accessories[UserData.ItemHat].Speed + GlobeSprite.Accessories[UserData.ItemGlobe].Speed + BootsSprite.Accessories[UserData.ItemShose].Speed;
        MinVaild = 35 - (vaild / 2);
        MaxVaild = 65 + (vaild / 2);
        resetTime = 1f + combo;
        perfactTime = resetTime / 2;
        if (!isColorSeted)
        {
            ColorUtility.TryParseHtmlString(UserData.ColorStr, out Color color);
            UserData.Color = color;
            TrailColor(color);
        }
    }

    private void Remove()
    {

    }

    public void AddGuard()
    {
        guard = 1;
    }
}
