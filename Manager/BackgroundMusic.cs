using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClipsBackground;
    [SerializeField] private AudioMixerGroup _audioMixerGroupBackground;
    [SerializeField, Range(0, 1)] public float _volume = 1;

    private AudioSource _audioSource;

    private void Start()
    {
        _volume = PlayerData.masterVolume;
        var cam = Camera.main;
        var parent = cam != null ? cam.transform : transform;

        _audioSource = SoundTools.NewAudioSource(parent, _audioMixerGroupBackground, _audioClipsBackground[0], 20f, 50f, _volume, false, false, false);
        StartCoroutine(LoopChangeMusics());
    }

    private IEnumerator LoopChangeMusics()
    {
        int indexClip = 0;

        while (true)
        {
            _audioSource.clip = _audioClipsBackground[indexClip];
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(_audioClipsBackground[indexClip].length);

            indexClip++;
            indexClip = indexClip >= _audioClipsBackground.Length ? 0 : indexClip;
        }
    }

    // Method to change the volume in real-time
    public void ChangeVolume(float newVolume)
    {
        _volume = Mathf.Clamp01(newVolume); // Ensure the volume is between 0 and 1
        _audioSource.volume = _volume;
    }
}

public class SoundTools
{
    public static AudioSource NewAudioSource(Transform parent, 
        AudioMixerGroup audioMixer, AudioClip audioClip, 
        float minDistance,  float maxDistance, float volume, 
        bool isLoop, bool playNow, bool destroyAfterFinished) 
    {
        var audioSourceObject = new GameObject(audioClip.name);
        audioSourceObject.transform.SetParent(parent, false);

        var source = audioSourceObject.AddComponent<AudioSource>();

        if (audioMixer)
            source.outputAudioMixerGroup = audioMixer;

        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.volume = volume;
        source.clip = audioClip;
        source.loop = isLoop;
        source.dopplerLevel = .5f;

        if (minDistance == 0 && maxDistance == 0)
            source.spatialBlend = 0f;
        else
            source.spatialBlend = 1f;

        if (playNow) 
        {
            source.playOnAwake = true;
            source.Play();
        } 
        else 
        {
            source.playOnAwake = false;
        }

        if (destroyAfterFinished) 
        { 
            MonoBehaviour.Destroy(audioSourceObject, audioClip.length);
        }

        return source;
    }
}

