using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    Rigidbody2D rb;
    public Collider2D Collider;
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = Random.Range(2f, 8f);
            Destroy(this.gameObject, 1.5f);
        }  
    }
}
