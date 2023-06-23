using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStomp : MonoBehaviour
{

    public PlayerMovement playerMovement;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="weak point")
        {
            playerMovement.KBcounter = playerMovement.KBtotaltime;

            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.KBfromRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.KBfromRight = false;
            }

            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(1);

        }
    }
}
