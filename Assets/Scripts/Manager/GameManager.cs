using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private bool _isGamePaused = false;

    private bool _isChaser = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isGamePaused)
            {
                PauseGame();
            }
        }

        if (!_isChaser)
        {
            _isChaser = ChaserGenerator.Instance.GenerateChaser();
        }

        //if(RealtimeLeaderboardManager.Instance.GetNameById()
    }

    public void ResumeGame()
    {
        if (_isGamePaused)
        {
            StartCoroutine(ResumeGameCoroutine());
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _isGamePaused = true;
        MouseManager.Show(true);
        MouseManager.Lock(false);
        UIManager.Instance.SetPauseImage(_isGamePaused);
    }

    public IEnumerator ResumeGameCoroutine()
    {
        UIManager.Instance._countDownTMP.gameObject.SetActive(true);
        _isGamePaused = false;
        MouseManager.Show(false);
        MouseManager.Lock(true);
        UIManager.Instance.SetPauseImage(_isGamePaused);
        for(int i = 3; i > 0; --i)
        {
            UIManager.Instance._countDownTMP.text = $"{i}";
            yield return new WaitForSecondsRealtime(1f);
        }
        UIManager.Instance._countDownTMP.gameObject.SetActive(false);
        Time.timeScale = 1;

        yield return null;
    }
}
