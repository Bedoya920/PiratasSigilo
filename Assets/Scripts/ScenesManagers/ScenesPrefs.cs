using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScenesPrefs : MonoBehaviour
{
    int totalMoney;
    public Button play;
    public Button restar;

    void Start()
    {
        if (!PlayerPrefs.HasKey("TotalMoney"))
        {
            PlayerPrefs.SetInt("TotalMoney", 0); 
            PlayerPrefs.Save();
        }

        totalMoney = PlayerPrefs.GetInt("TotalMoney");
        print($"Dinero al iniciar: {totalMoney}");

        if (play != null)
            play.onClick.AddListener(EscenaJuego);
        
        if (restar != null)
            restar.onClick.AddListener(() => RestarMoney(200));

        
    }

    public void RestarMoney(int amount)
    {
        if (totalMoney >= amount)
        {
            totalMoney -= amount;
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
            PlayerPrefs.Save();
            print($"Compra realizada. Te quedan: {totalMoney}");
        }
        else
        {
            print($"No tienes dinero patrón, solo tienes: {totalMoney}, te faltan {amount - totalMoney}");
        }
    }

    public void EscenaJuego()
    {
        SceneManager.LoadScene(0);
    }

    public void EscenaShop()
    {
        SceneManager.LoadScene(1);
    }

    void OnTriggerEnter(Collider col)
    {
        MoneyManager.Instance.TotalEarnings();
        EscenaShop();
    }
}
