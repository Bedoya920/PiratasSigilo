using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform pivot;
    public GameObject bola;
    public float shootingRange; 
    public float shootCooldown;
    public float projectileSpeed = 10f;

    private float shootTimer;

    void Start()
    {
        shootTimer = shootCooldown;
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
    }

    public void Shoot(Transform target)
    {
        if (shootTimer >= shootCooldown)
        {
            GameObject projectile = Instantiate(bola, pivot.position, pivot.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.velocity = (target.position - pivot.position).normalized * projectileSpeed; // Velocidad de la bola
            }
             
            shootTimer = 0f;
        }
    }
}
