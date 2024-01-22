using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class СountdownBoard : MonoBehaviour
{
    [Header("Sounds")] 
    [SerializeField] private AudioClip _tickClip;
    [SerializeField] private AudioClip _finishСountdownClip;
    [SerializeField] private AudioMixerGroup _audioMixer;
    [Header("Settings")]
    [SerializeField, Range(0, 1f)] private float _volume = 1f;
    [SerializeField, Range(0, 10f)] private float _delayOffBoard = 5f;
    [SerializeField, Range(0, 5)] private int _onYellowLightTo = 2;
    [Header("Objects")]
    [SerializeField] private GameObject _firstNumber;
    [SerializeField] private GameObject[] _secondNumbers;
    [SerializeField] private GameObject _redLight;
    [SerializeField] private GameObject _yellowLight;
    [SerializeField] private GameObject _greenLight;

    private AudioSource _sourceTick;
    private AudioSource _sourceFinish;
    
    private enum Light { Green, Yellow, Red, None}
    Coroutine _coroutine;

    public void StartСountdown(float seconds)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(СountdownAnimation(seconds));
        StartCoroutine(Timer(seconds));
    }

    private void Start()
    {
        var cam = Camera.main;
        var parent = cam != null ? cam.transform : transform;
        
        _sourceTick = SoundTools.NewAudioSource(parent, _audioMixer, _tickClip, 20f, 100f, _volume, false, false, false);
        _sourceFinish = SoundTools.NewAudioSource(parent, _audioMixer, _finishСountdownClip, 20f, 100f, _volume, false, false, false);
    }

    private IEnumerator СountdownAnimation(float countdownTime)
    {
        float delay = countdownTime + 1 - _secondNumbers.Length;
        int startIndex = _secondNumbers.Length - 2;

        if (delay >= 0)
        {
            StartState(startIndex + 1);
            yield return new WaitForSeconds(delay);
        }
        else
        {
            startIndex += (int)delay;
            StartState(startIndex + 1);
        }

        for (int i = startIndex; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            if(i == _onYellowLightTo)
                SwitchLightTo(Light.Yellow);

            _sourceTick.Play();
            _secondNumbers[i + 1].SetActive(false);
            _secondNumbers[i].SetActive(true);
        }
        SwitchLightTo(Light.Green);
        _sourceFinish.Play();

        yield return new WaitForSeconds(_delayOffBoard);
        HideAll();
        _coroutine = null;
    }

    private IEnumerator Timer(float countdownTime)
    {
        float t = countdownTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
    }

    private void StartState(int i)
    {
        HideAll();
        _firstNumber.SetActive(true);

        if (i >= 0 && _secondNumbers.Length > i)
            _secondNumbers[i].SetActive(true);

        if (i > _onYellowLightTo)
            SwitchLightTo(Light.Red);
        else if (i < _onYellowLightTo)
            SwitchLightTo(Light.Green);
        else
            SwitchLightTo(Light.Yellow);

    }

    private void HideAll()
    {
        SwitchLightTo(Light.None);

        _firstNumber.SetActive(false);
        foreach (var item in _secondNumbers)
            item.SetActive(false);
    }

    private void SwitchLightTo(Light light)
    {
        _redLight.SetActive(light == Light.Red);
        _yellowLight.SetActive(light == Light.Yellow);
        _greenLight.SetActive(light == Light.Green);
    }
}
