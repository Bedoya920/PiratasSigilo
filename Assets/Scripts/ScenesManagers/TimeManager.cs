using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverStuff;

public class TimeManager : MonoBehaviour, ITime
{
    public static TimeManager ins { get; private set; }

    private List<IEnemyObserver> observers = new List<IEnemyObserver>();

    private void Awake()
    {
        if (ins == null) ins = this;
        else Destroy(gameObject);

    }

    public void AddObserver(IEnemyObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IEnemyObserver observer)
    {
        observers.Remove(observer);
    }

    //Cada cierto tiempo, llamar está funcion, que los enemigos suben las stats solos
    public void NotifyHour()
    {
        foreach(IEnemyObserver obs in observers)
        {
            obs.TimeChange();
        }
    }

}
