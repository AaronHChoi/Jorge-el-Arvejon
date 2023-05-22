using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firepoint;
    public GameObject bulletPrefab;

    public float firerate;

    float nextBullet;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
        }

    }

    private void Shoot()
    {
        if (Time.time > nextBullet)
        {
            nextBullet = Time.time + firerate;
            Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);

        }
    }
}
