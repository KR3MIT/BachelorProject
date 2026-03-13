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

    public void PlaySound0()
    {
        AudioManager.Instance.Play(SoundType.PaperSlideSoft);
    }
    public void PlaySound1()
    {
        AudioManager.Instance.Play(SoundType.PaperSlideHard);
    }
    public void PlaySound2()
    {
        AudioManager.Instance.Play(SoundType.Stamp);
    }
    public void PlaySound3()
    {
        AudioManager.Instance.Play(SoundType.PaperTear);
    }
}
