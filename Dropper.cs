using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] PlinkoManager manager;
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject ballPrefab;

    public bool ballActive;
    private Color highlightColor;

    void Start()
    {
        highlightColor = highlight.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (!ballActive && manager.playerBet >= manager.minBet)
        {
            Instantiate(ballPrefab, highlight.transform.position, highlight.transform.rotation);
            manager.DropBall();
            ballActive = true;
        }
    }

    private void OnMouseOver()
    {
        if (!ballActive)
        {
            highlightColor.a = 1;
            highlight.GetComponent<SpriteRenderer>().color = highlightColor;
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.z = 0;
            mPos.y = 5.25f;
            mPos.x = Mathf.Clamp(mPos.x, -3.3f, 8.3f);
            //Debug.Log(mPos);
            highlight.transform.position = mPos;
            //Debug.Log(highlight.transform.position);
        }
        else
        {
            highlightColor.a = 0;
            highlight.GetComponent<SpriteRenderer>().color = highlightColor;
        }
    }
}
