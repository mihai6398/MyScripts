using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float CurrentTime => _time;

    private float _time;
    private Coroutine _coroutineTimer;

    public void StartTimer(float delay = 0)
    {
        StopTimer();

        _time = -delay;
        _coroutineTimer = StartCoroutine(TimerOn());
    }

    public void StopTimer()
    {
        if (_coroutineTimer != null)
            StopCoroutine(_coroutineTimer);
    }

    private IEnumerator TimerOn()
    {
        while (true)
        {
            yield return null;
            _time += Time.deltaTime;
        }
    }
}
