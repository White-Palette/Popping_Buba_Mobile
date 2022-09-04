using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;

    private bool isLoading = false;

    private void Start()
    {
        isLoading = false;
        inputField.Select();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputField.Select();
        }
        
        if (inputField.text.Length > 16)
            inputField.text = inputField.text.Substring(0, 16);
        if (Input.GetKeyDown(KeyCode.Return) && !isLoading)
        {
            isLoading = true;
            UserData.UserName = inputField.text;
            if(inputField.text == "")
            {
                UserData.UserName = $"Guest{Random.Range(1, 10000):0000}";
            }
            Fade.Instance.FadeOutToMainMenu();
        }
    }
}