using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStomp : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public Rigidbody2D rb;
    public float bounceForce = 20f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="weak point")
        {
            rb.velocity = new Vector2(bounceForce, bounceForce);

            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);

        }
    }
}
