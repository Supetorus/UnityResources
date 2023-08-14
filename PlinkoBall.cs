using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlinkoBall : MonoBehaviour
{
    [SerializeField] new Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody.sharedMaterial.bounciness = 0.75f + (Random.value / 10.0f);
    }
}
