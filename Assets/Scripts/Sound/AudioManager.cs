using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    PaperSlideSoft,
    PaperSlideHard,
    PaperTear,
    Stamp,
    PaperFlutter,
    Click,
    Music,
    UIClick
}
public enum SoundCategory
{
    SFX,
    Music,
    UI
}

[System.Serializable]
public class Sound
{
    public SoundType type;
    public SoundCategory category;
    public AudioClip[] clips;
    [UnityEngine.Range(0f, 1f)]
    public float volume = 1f;
    [UnityEngine.Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool randomPitch = false;
    [UnityEngine.Range(0.1f, 0.3f)]
    public float randomRange = 0f;

    [HideInInspector]
    public AudioClip currentClip;
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }


    public Sound[] sounds;

    [Header("Audio Mixers")]
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup uiGroup;


    private Dictionary<SoundType, Sound> soundDictionary;

    // AudioSources
    private AudioSource sfxSource;
    private AudioSource musicSource;
    private AudioSource uiSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);


        // Create shared AudioSources
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = musicGroup;
        musicSource.loop = true;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.outputAudioMixerGroup = sfxGroup;

        uiSource = gameObject.AddComponent<AudioSource>();
        uiSource.outputAudioMixerGroup = uiGroup;

        soundDictionary = new Dictionary<SoundType, Sound>();

        foreach (Sound s in sounds)
        {
           // s.source = gameObject.AddComponent<AudioSource>();
            //s.source.outputAudioMixerGroup = mixerGroup;
            //s.source.volume = s.volume;
           // s.source.pitch = s.pitch;

            if (!soundDictionary.ContainsKey(s.type))
                soundDictionary.Add(s.type, s);
            else
                Debug.LogWarning("Duplicate sound type: " + s.type);
        }
    }


    public void Play(SoundType type)
    {
        if (!soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning("Sound not found: " + type);
            return;
        }

        RandomizeClip(s);
        RandomizePitch(s);

        switch (s.category)
        {
            case SoundCategory.Music:
                PlayMusic(s);
                break;
            case SoundCategory.SFX:
                PlaySFX(s);
                break;
            case SoundCategory.UI:
                PlayUI(s);
                break;
        }
    }

    private void RandomizeClip(Sound s)
    {
        s.currentClip = s.clips[Random.Range(0, s.clips.Length)];
    }

    private void RandomizePitch(Sound s)
    {
        if (s.randomPitch)
        {
            float randomized = Random.Range(s.pitch - s.randomRange, s.pitch + s.randomRange);
            switch (s.category)
            {
                case SoundCategory.Music:
                    musicSource.pitch = randomized;
                    break;
                case SoundCategory.SFX:
                    sfxSource.pitch = randomized;
                    break;
                case SoundCategory.UI:
                    uiSource.pitch = randomized;
                    break;
            }
        }
        else
        {
            switch (s.category)
            {
                case SoundCategory.Music:
                    musicSource.pitch = s.pitch;
                    break;
                case SoundCategory.SFX:
                    sfxSource.pitch = s.pitch;
                    break;
                case SoundCategory.UI:
                    uiSource.pitch = s.pitch;
                    break;
            }
        }
    }


    private void PlaySFX(Sound s)
    {
        sfxSource.PlayOneShot(s.currentClip, s.volume);
    }

    private void PlayMusic(Sound s)
    {
        musicSource.clip = s.currentClip;
        musicSource.volume = s.volume;
        musicSource.Play();
    }

    private void PlayUI(Sound s)
    {
        uiSource.PlayOneShot(s.currentClip, s.volume);
    }
}
