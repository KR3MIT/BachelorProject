using UnityEngine;

public class SoundTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySoundMusic()
    {
        AudioManager.Instance.Play(SoundType.Music);
    }
    public void PlaySoundUI()
    {
        AudioManager.Instance.Play(SoundType.UIClick);
    }
    public void PlaySoundSFXSingle()
    {
        AudioManager.Instance.Play(SoundType.Stamp);
    }
    public void PlaySoundSFXMultiple()
    {
        AudioManager.Instance.Play(SoundType.PaperSlideSoft);
    }

    public void PlayStamp()
    {
        AudioManager.Instance.Play(SoundType.Stamp);
    }

    public void PlayWhooshDragon()
    {
        AudioManager.Instance.Play(SoundType.WhooshDragon);
    }

    public void PlayWhooshFree()
    {
        AudioManager.Instance.Play(SoundType.WhooshFree);
    }
}
