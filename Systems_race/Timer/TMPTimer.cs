using UnityEngine;
using TMPro;
using Tracking;
using System;
using static UnityEngine.Rendering.DebugUI;

public class TMPTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private Timer _timer;

    private void Update()
    {
        if (_timer == null)
            return;

        if (_timer.CurrentTime > 0)
        {
            if (!_timerText.gameObject.activeSelf)
            {
                _timerText.gameObject.SetActive(true);
            }

            SetTimerText(_timer.CurrentTime);
        }
        else
        {
            if (_timerText.gameObject.activeSelf)
            {
                _timerText.gameObject.SetActive(false);
            }
        }
    }

    private void SetTimerText(float time)
    {
        _timerText.text = time.ToTimeFormat();
    }
}
