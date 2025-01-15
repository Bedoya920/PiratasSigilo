using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 50;

    void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject); 
        }else{
            Destroy(gameObject, 5f);
        }
        
        
    }
}
