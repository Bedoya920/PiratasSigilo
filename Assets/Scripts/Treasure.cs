using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField]private int price;

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            MoneyManager.Instance.GainMoney(100);
            Destroy(gameObject);
        }
    }
}
