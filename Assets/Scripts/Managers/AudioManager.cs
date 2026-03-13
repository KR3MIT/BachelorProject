using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip[] clips;
    [UnityEngine.Range(0f, 2f)]
    public float volume = 1f;
    [UnityEngine.Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool randomPitch = false;
    [UnityEngine.Range(0.1f, 0.3f)]
    public float randomRange = 0f;

    [HideInInspector]
    public AudioSource source;
}


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public Sound[] sounds;
    private Dictionary<SoundType, Sound> soundDictionary;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        soundDictionary = new Dictionary<SoundType, Sound>();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

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

        AudioClip clip = s.clips[Random.Range(0, s.clips.Length)];
        s.source.clip = clip;

        if (s.randomPitch)
            s.source.pitch = Random.Range(s.pitch - s.randomRange, s.pitch + s.randomRange);
        else
            s.source.pitch = s.pitch;

        s.source.Play();


    }
}
