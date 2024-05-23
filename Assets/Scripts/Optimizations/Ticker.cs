using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public float tickTime = 0.2f;
    private float _timerTimer;
    public delegate void TickAction();
    public static event TickAction OnTickAction;

    private void Update()
    {
        _timerTimer += Time.deltaTime;

        if (_timerTimer >= tickTime)
        {
            _timerTimer = 0;
            TickEvent();
        }
    }

    private void TickEvent()
    {
        OnTickAction?.Invoke();
    }
}
