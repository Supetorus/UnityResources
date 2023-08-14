using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAnnouncement : MonoBehaviour
{
    [SerializeField] TMP_Text label;
    [SerializeField] float waitTimer = 0;
    float timer = 0;

    public void Display(string text)
    {
        label.text = text;
        Color color = label.color;
        color.a = 1;
        timer = waitTimer;
        label.color = color;

    }

    void Update()
    {
        Color color = label.color;

        if ( color.a  >  0)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                color.a -= 1 * Time.deltaTime / 2;
            }
        }
        
        label.color = color;
    }
}

