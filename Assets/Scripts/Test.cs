using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] EffectController _effectController = null;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _effectController.Play(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
