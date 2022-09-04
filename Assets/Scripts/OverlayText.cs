using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OverlayText : MonoBehaviour, IPoolable
{
    public TMPro.TMP_Text Text;

    private void Awake()
    {
        Text = GetComponent<TMPro.TMP_Text>();
    }

    public void Initialize()
    {
        if (Text == null)
        {
            Text = GetComponent<TMPro.TMP_Text>();
        }
        Text.text = "";
        Text.color = Color.white;
        Text.DOFade(1, 0.5f).From(0);
        StartCoroutine(DestroyCoroutine());
    }

    public void MoveTo(Vector3 positoin)
    {
        transform.position = positoin;
    }

    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(1);
        Text.DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PoolManager<OverlayText>.Release(this);
    }
}
