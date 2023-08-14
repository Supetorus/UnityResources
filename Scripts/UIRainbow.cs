using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIRainbow : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    private int speed = 1;
    void Update()
    {
        float h, s, v;
        Color color = label.color;
        Color tempAlpha = color;
        Color.RGBToHSV(color, out h, out s, out v);

        Color beans = Color.HSVToRGB(h + Time.deltaTime * .5f, s, v);

        beans.a = tempAlpha.a;
        label.color = beans;
    }
}
