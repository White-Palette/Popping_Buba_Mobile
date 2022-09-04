using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] Slider brightness;

    [SerializeField] Light2D light2d;

    private void Start()
    {
        light2d.intensity = UserData.Brightness;
        brightness.value = light2d.intensity - 1;
    }

    private void Update()
    {
        light2d.intensity = brightness.value + 1;
        UserData.Brightness = light2d.intensity;
        UserData.Save();
    }

}
