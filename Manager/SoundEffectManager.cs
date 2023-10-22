using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
}

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;

    public SoundEffect[] soundEffects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundEffect(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        GameObject soundObject = new GameObject("SoundEffect");
        soundObject.transform.position = position;
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.Play();

        Destroy(soundObject, clip.length);
    }

    public void PlaySoundEffect(string clipName, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        SoundEffect sfx = GetSoundEffect(clipName);
        if (sfx != null)
        {
            PlaySoundEffect(sfx.clip, position, sfx.volume * volume, sfx.pitch * pitch);
        }
    }

    private SoundEffect GetSoundEffect(string clipName)
    {
        foreach (SoundEffect sfx in soundEffects)
        {
            if (sfx.clip.name == clipName)
            {
                return sfx;
            }
        }
        Debug.LogWarning("Sound effect not found: " + clipName);
        return null;
    }
}