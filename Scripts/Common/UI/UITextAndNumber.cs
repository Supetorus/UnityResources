using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextAndNumber : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] TMP_Text text;
    [SerializeField] StringData strData;
    [SerializeField] IntData intData;

    private void OnValidate()
    {
        if (strData != null)
        {
            name = strData.name;
            label.text = strData.value;
        }
    }

    private void Update()
    {
        label.text = strData.value;
        if (label.text.Equals("​"))
        {
            text.text = "";
        }
        else
        {
            text.text = intData.value.ToString();
        }
        //text.text = intData.value.ToString();
    }
}
