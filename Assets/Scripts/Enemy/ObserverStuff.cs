using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverStuff
{
    public interface IEnemyObserver
    {
        void TimeChange();
    }

    public interface ITime
    {
        void AddObserver(IEnemyObserver observer);
        void RemoveObserver(IEnemyObserver observer);
        void NotifyHour();
    }

    public enum DayMoment
    {
        Morning,
        Afternoon,
        Night
    }

}
