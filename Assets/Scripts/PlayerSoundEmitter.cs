using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEmitter : MonoBehaviour
{
    public float soundRange = 5f; 

    //Dibuja rango en la escena
    void OnDrawGizmos()
    {
        //Magenta = rango sonido generado desde el player
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, soundRange);
    }

    
    public void EmitSound()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, soundRange);
        foreach (var collider in colliders)
        {
            EnemyAI enemyAI = collider.GetComponent<EnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.OnHearSound(transform.position);
                print("me escuchó");
            }
        }
    }
}
