using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private Button AudioButton;
    [SerializeField] private Slider AudioSlider;
    [SerializeField] private TMP_Text Text;

    [SerializeField] private Sprite OnImage;
    [SerializeField] private Sprite OffImage;

    [SerializeField] private Transform SliderPos;
    private Image _Image;
    private float cooldown;
    private bool hidden = true;
    private Vector3 audioPos;

    private void Awake()
    {
        _Image = AudioButton.GetComponent<Image>();
        audioPos = SliderPos.position;
        AudioSlider.value = PlayerPrefs.GetFloat("AudioVolume", 1f);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DoAudioStuff(AudioSlider.value);
        AudioSlider.onValueChanged.AddListener(DoAudioStuff);
        AudioButton.onClick.AddListener(ToggleSlider);

        ToggleSlider(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (AudioSlider.gameObject.activeSelf)
        {
            cooldown += Time.deltaTime;
        }
    }

    private void DoAudioStuff(float value)
    {
        if (value == 0f) _Image.sprite = OffImage; else _Image.sprite = OnImage;

        string text = Mathf.Round(value * 100f) + "%";
        Text.text = text;
        AudioMixerController.Instance.SetMasterVolume(value);

        if (!AudioSlider.gameObject.activeSelf || cooldown >= 0.25f)
        {
            AudioManager.Instance.Play(SoundType.PaperSlideSoft);
            cooldown = 0f;
        }
    }
    private void ToggleSlider(bool _bool)
    {
        hidden = _bool;

        SetSliderPos();
        if (!hidden) AudioShow(); else AudioHide();
    }

    private void ToggleSlider()
    {
        hidden = !hidden;

        SetSliderPos();
        if (!hidden) AudioShow(); else AudioHide();
    }

    private void SetSliderPos()
    {
        audioPos = SliderPos.position;
    }

    private void AudioShow()
    {
        var transform = AudioSlider.gameObject.transform;
        transform.DOJump(audioPos, 1f, 1, 1f).SetEase(Ease.OutBounce);
    }

    private void AudioHide()
    {
        var transform = AudioSlider.gameObject.transform;
        transform.DOJump(audioPos + new Vector3(0, -500, 0), 1f, 1, 1f).SetEase(Ease.OutBounce);
    }
}
