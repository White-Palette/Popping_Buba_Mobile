using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScrollbarArrow : MonoSingleton<TitleScrollbarArrow>
{
    [SerializeField]
    private GameObject leftArrow;
    [SerializeField]
    private GameObject rightArrow;

    private Scrollbar scrollbar;

    private void Start()
    {
        scrollbar = GetComponent<Scrollbar>();
        EnabledArrows();
    }

    private void Update()
    {
        if(scrollbar.value <= 0)
        {
            rightArrow.SetActive(true);
            leftArrow.SetActive(false);
        }
        else if(scrollbar.value >= 1)
        {
            rightArrow.SetActive(false);
            leftArrow.SetActive(true);
        }
        else
        {
            rightArrow.SetActive(true);
            leftArrow.SetActive(true);
        }
    }

    public void EnabledArrows()
    {
        rightArrow.SetActive(false);
        leftArrow.SetActive(false);
    }
}
