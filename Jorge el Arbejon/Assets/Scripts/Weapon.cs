using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bullets;


    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {

        playerMovement = GetComponent<PlayerMovement>();        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && cooldownTimer > attackCooldown)
            Attack();

        cooldownTimer += Time.deltaTime;

    }

    private void Attack()
    {
        cooldownTimer = 0;

        bullets.transform.position = firepoint.position;
        bullets.GetComponent<bullet>().SetDirection(Mathf.Sign(transform.localScale.x));


    }
}
