using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public int Health = 100;


    public void BichoLife()
    {
        if (Health == 0)
        {
            Destroy(gameObject);
        }
    }

}
