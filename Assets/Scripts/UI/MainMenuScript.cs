using TMPro;
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

    private void Awake()
    {
        _Image = AudioButton.gameObject.GetComponent<Image>();
        OnImage = _Image.sprite;
        DoAudioStuff(AudioSlider.value);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSlider.gameObject.SetActive(false);

        AudioSlider.onValueChanged.AddListener(DoAudioStuff);
        AudioButton.onClick.AddListener(ToggleSlider);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DoAudioStuff(float value)
    {
        if(value == 0f)
            _Image.sprite = OffImage;
        else
            _Image.sprite = OnImage;

        string text = Mathf.Round(value * 100f) + "%";
        Text.text = text;
        AudioManager.Instance.SetVolume(value);
        AudioManager.Instance.Play(SoundType.PaperSlideSoft);
    }

    private void ToggleSlider()
    {
        AudioSlider.gameObject.SetActive(!AudioSlider.gameObject.activeSelf);
    }
}
