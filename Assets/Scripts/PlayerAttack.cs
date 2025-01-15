using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 2f;
    public int attackDamage = 100;
    public LayerMask enemyLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

            if (enemyHealth != null && enemyAI != null)
            {
                if (CanBeKilledByPlayer(enemyAI))
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    bool CanBeKilledByPlayer(EnemyAI enemyAI)
    {
        return !enemyAI.isAgro && !enemyAI.IsPlayerInSight() && Vector3.Distance(transform.position, enemyAI.transform.position) <= attackRange;
    }

    void OnDrawGizmos()
    {
        //negro = Rango para matar
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
