using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button AudioButton;
    [SerializeField] private Slider AudioSlider;
    [SerializeField] private TMP_Text Text;

    [SerializeField] private Sprite OnImage;
    [SerializeField] private Sprite OffImage;
    private Image _Image;
    private float cooldown;

    private void Awake()
    {
        _Image = AudioButton.gameObject.GetComponent<Image>();
        OnImage = _Image.sprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSlider.gameObject.SetActive(false);

        AudioSlider.onValueChanged.AddListener(DoAudioStuff);
        AudioButton.onClick.AddListener(ToggleSlider);

        DoAudioStuff(AudioSlider.value);
    }

    void Update()
    {
        if(AudioSlider.gameObject.activeSelf)
        {
            cooldown += Time.deltaTime;
        }
    }

    private void DoAudioStuff(float value)
    {
        if(value == 0f)
            _Image.sprite = OffImage;
        else
            _Image.sprite = OnImage;

        string text = Mathf.Round(value * 100f) + "%";
        Text.text = text;

        // Only apply volume changes if enough time has passed (throttle while slider is open)
        // Allow immediate application when the slider is not visible (e.g. initial setup)
        if (!AudioSlider.gameObject.activeSelf || cooldown >= 0.1f)
        {
            AudioManager.Instance.SetVolume(value);
            AudioManager.Instance.Play(SoundType.PaperSlideSoft);
            cooldown = 0f;
        }
    }

    private void ToggleSlider()
    {
        AudioSlider.gameObject.SetActive(!AudioSlider.gameObject.activeSelf);
    }
}
