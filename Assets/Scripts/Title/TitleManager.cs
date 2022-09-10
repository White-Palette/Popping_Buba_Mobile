using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Image startImage;
    [SerializeField] TextMeshProUGUI startTMP;
    [SerializeField] TextMeshProUGUI usernameTMP;
    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject helpPanel;
    [SerializeField] GameObject gameQuitPanel;
    [SerializeField] GameObject shopPanel;
    [SerializeField] GameObject shopPanelBackground;

    [SerializeField] GameObject inventoryShop;
    [SerializeField] GameObject inventoryHead;
    [SerializeField] GameObject inventoryGlove;
    [SerializeField] GameObject inventoryBoots;
    [SerializeField] GameObject inventoryTrail;
    [SerializeField] TextMeshProUGUI shopTitleTMP;
    [SerializeField] TextMeshProUGUI coinAmountTMP;

    [SerializeField] TextMeshProUGUI coinTMP;

    private float fadeTime = 2f;

    private bool isSettingEnable = false;
    private bool isGameQuitEnable = false;
    private bool isShopEnable = false;
    private bool isHelpEnable = false;

    private bool isHelp = false;
    private bool isLoading = false;
    private bool isBounce = false;

    private void Start()
    {
        usernameTMP.text = UserData.UserName;
        helpPanel.SetActive(false);
        StartCoroutine(FadeInOut());
        Fade.Instance.FadeIn();
        isLoading = false;
        isHelp = false;
        coinTMP.SetText($"{SaveManager.Instance.Coin}$");
        if (PlayerPrefs.GetInt("isFirst", 0) == 0)
        {
            PlayerPrefs.SetInt("isFirst", 1);
            OpenHelpPanel();
        }

        coinAmountTMP.text = PlayerPrefs.GetInt("coin", 0).ToString();
    }

    private void Update()
    {
        KeyDown();
    }

    #region Fade
    IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(FadeTMP(1, 0.3f));

            yield return StartCoroutine(FadeTMP(0.3f, 1));
        }
    }

    private IEnumerator FadeTMP(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = startTMP.color;
            color.a = Mathf.Lerp(start, end, percent);
            startTMP.color = color;

            yield return null;
        }
    }
    #endregion

    public void KeyDown()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SettingPanel();
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            GameQuitPanel();
        }
        else if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape))
        {
            DisableAllPanel();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ShopPanel();
        }
        else if(Input.GetKeyDown(KeyCode.H))
        {
            OpenHelpPanel();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return;
        }
        else if (Input.anyKeyDown)
        {
            if (!isLoading && !isHelp)
            {
                Fade.Instance.FadeOutToGameScene();
                isLoading = true;
            }
        }
    }

    public void DisableAllPanel()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        if (isSettingEnable)
        {
            SettingPanel();
        }
        if (isGameQuitEnable)
        {
            GameQuitPanel();
        }
        if (isShopEnable)
        {
            ShopPanel();
        }
    }

    public void DisableAllInventoryObj()
    {
        inventoryShop.SetActive(false);
        inventoryHead.SetActive(false);
        inventoryGlove.SetActive(false);
        inventoryBoots.SetActive(false);
        inventoryTrail.SetActive(false);
    }

    IEnumerator TogglePanel(GameObject Panel, bool isEnable)
    {
        if (!isEnable)
        {
            Panel.transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f).From(1f);
            yield return new WaitForSeconds(0.2f);
        }

        Panel.SetActive(!Panel.activeSelf);

        if (isEnable)
        {
            isBounce = true;
            Panel.transform.DOScale(new Vector3(1f, 1f, 0f), 0.6f).SetEase(Ease.OutBounce).From(0f);
            yield return new WaitForSeconds(0.6f);
            isBounce = false;
        }

        yield break;
    }

    public void OpenHelpPanel()
    {
        isHelpEnable = !isHelpEnable;
        SoundManager.Instance.PlaySound(Effect.Click);
        StartCoroutine(TogglePanel(helpPanel, isHelpEnable));
    }

    public void HelpPanel()
    {
        isHelp = !isHelp;
        SoundManager.Instance.PlaySound(Effect.Click);
        Fade.isTutoMap = true;
        Fade.Instance.FadeOutToTutorial();
    }

    public void SettingPanel()
    {
        isSettingEnable = !isSettingEnable;
        SoundManager.Instance.PlaySound(Effect.Click);
        StartCoroutine(TogglePanel(settingPanel, isSettingEnable));
    }

    public void GameQuitPanel()
    {
        isGameQuitEnable = !isGameQuitEnable;
        SoundManager.Instance.PlaySound(Effect.Click);
        StartCoroutine(TogglePanel(gameQuitPanel, isGameQuitEnable));
    }

    public void ShopPanel()
    {
        isShopEnable = !isShopEnable;
        SoundManager.Instance.PlaySound(Effect.Click);
        StartCoroutine(TogglePanel(shopPanel, isShopEnable));
    }

    public void ClickButton()
    {
        if (isBounce) return;

        Vector3 btnPos = EventSystem.current.currentSelectedGameObject.transform.position;

        shopPanelBackground.transform.DOMove(btnPos, 0.25f).SetEase(Ease.OutQuad);
    }

    public void ClickInventoryBtns(GameObject btn)
    {
        DisableAllInventoryObj();
        shopTitleTMP.text = btn.name;
        btn.SetActive(true);
    }

    public void GameStart()
    {
        Debug.Log("Game Start");
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        SoundManager.Instance.PlaySound(Effect.Click);
        Application.Quit();
        Debug.Log("Quit");
    }
}
