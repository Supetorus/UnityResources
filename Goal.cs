using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] PlinkoManager manager;
    [SerializeField] Dropper dropper;
    [SerializeField] int returnValue;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(collision.gameObject);
        manager.Score(returnValue);
        dropper.ballActive = false;
    }
}
