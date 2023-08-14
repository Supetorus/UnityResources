using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnnouncement : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    Color rgba = new Color(0, ((float)85 / 255), 0, 255);

    public void Display(string text)
    {
        label.text = text;
        rgba.a = 1;
    }

    void Update()
    {
        if (rgba.a > 0)
        {
            rgba.a -= 1 * Time.deltaTime / 2;
        }
        label.color = rgba;
    }
}
