using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObserverStuff;

public class TimeManager : MonoBehaviour, ITime
{
    public static TimeManager ins { get; private set; }
    private List<IEnemyObserver> observers = new List<IEnemyObserver>();

    public static DayMoment currentMoment{ get; private set; }

    [Header("Day moment config")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float totalCycleTime = 100f;
    private float timeLeft;

    private Color targetColor;
    private float targetIntensity;
    private Quaternion targetRotation;

    private float morningThreshold;
    private float afternoonThreshold;
    private float nightThreshold;

    private bool dayEnded = false;

    private void Awake()
    {
        if (ins == null) ins = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        timeLeft = totalCycleTime;
        currentMoment = DayMoment.Morning;
        UpdateLighting(currentMoment);

        float third = totalCycleTime / 3f;
        morningThreshold = totalCycleTime;
        afternoonThreshold = totalCycleTime - third;
        nightThreshold = totalCycleTime - (2f * third);
    }

    private void Update()
    {
        //Aqui debería llamar una cinemtacia como de derrota
        if (dayEnded) return;

        timeLeft -= Time.deltaTime;

        UpdateMomentIfNeeded();

        // Interpolación suave de la luz, para que no cambie tan brusco
        if (directionalLight != null)
        {
            directionalLight.color = Color.Lerp(directionalLight.color, targetColor, Time.deltaTime * 1.5f);
            directionalLight.intensity = Mathf.Lerp(directionalLight.intensity, targetIntensity, Time.deltaTime * 1.5f);
            directionalLight.transform.rotation = Quaternion.Slerp(directionalLight.transform.rotation, targetRotation, Time.deltaTime * 1.5f);
        }

        // Termino el día
        if (timeLeft <= 0f)
        {
            dayEnded = true;
        }
    }

    private void UpdateMomentIfNeeded()
    {
        if (timeLeft <= afternoonThreshold && currentMoment == DayMoment.Morning)
        {
            currentMoment = DayMoment.Afternoon;
            UpdateLighting(currentMoment);
            NotifyHour();
        }
        else if (timeLeft <= nightThreshold && currentMoment == DayMoment.Afternoon)
        {
            currentMoment = DayMoment.Night;
            UpdateLighting(currentMoment);
            NotifyHour();
        }
    }

    private void UpdateLighting(DayMoment moment)
    {
        switch (moment)
        {
            case DayMoment.Morning:
                targetColor = new Color(1f, 0.95f, 0.8f);
                targetIntensity = 1.0f;
                targetRotation = Quaternion.Euler(30f, 30f, 0f);
                break;

            case DayMoment.Afternoon:
                targetColor = Color.white;
                targetIntensity = 1.2f;
                targetRotation = Quaternion.Euler(60f, 45f, 0f);
                break;

            case DayMoment.Night:
                targetColor = new Color(0.1f, 0.1f, 0.35f);
                targetIntensity = 0.2f;
                targetRotation = Quaternion.Euler(-30f, 0f, 0f);
                break;
        }
    }

    public void AddObserver(IEnemyObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IEnemyObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyHour()
    {
        foreach (IEnemyObserver obs in observers)
        {
            obs.TimeChange();
        }
    }
}
