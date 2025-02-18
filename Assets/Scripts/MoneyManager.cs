using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    private int actualMoney;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    void Start()
    {
        
    }

    public void GainMoney(int amount)
    {
        actualMoney += amount;
        print($"Dinero actual: {actualMoney}");
    }

    public void TotalEarnings()
    {
        int total = actualMoney + PlayerPrefs.GetInt("TotalMoney", 0);
        PlayerPrefs.SetInt("TotalMoney", total);
        PlayerPrefs.Save();
        actualMoney = 0;
    }
}
