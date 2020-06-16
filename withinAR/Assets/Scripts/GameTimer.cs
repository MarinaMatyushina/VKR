using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float maxTimerValue;
    private float currentTimerValue;
    private int intTimerValue;
    private bool isLevelStarted;

    private void Awake()
    {
        isLevelStarted = false;
        currentTimerValue = maxTimerValue;
        intTimerValue = (int)currentTimerValue;
    }

    public void StopTick()
    {
        Debug.LogError("Stop timer");
        isLevelStarted = false;
    }

    public int GetMaxTime()
    {
        return (int)maxTimerValue;
    }

    public int GetCurrentTime()
    {
        return intTimerValue;
    }

    public void StartTick()
    {
        Debug.LogError("Start timer");
        isLevelStarted = true;
    }

    public void ResetTimer(int maxTimerValue)
    {
        isLevelStarted = false;
        currentTimerValue = this.maxTimerValue;
        intTimerValue = (int)currentTimerValue;
    }

    private void Update()
    {
        if (!isLevelStarted) return;
        currentTimerValue -= Time.deltaTime;
        intTimerValue = (int)currentTimerValue;        
    }
}
