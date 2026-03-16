using System;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Transform gameCameraTransform;
    [SerializeField] private Button startButton;
    private GameObject cam;

    [Header("Audio")]
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
        cam = Camera.main.gameObject;

        AudioSlider.onValueChanged.AddListener(DoAudioStuff);
        AudioButton.onClick.AddListener(ToggleSlider);
    }

    public void Show(Action onStartGame)
    {
        gameObject.SetActive(true);

        AudioSlider.gameObject.SetActive(false);
        DoAudioStuff(AudioSlider.value);

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => onStartGame?.Invoke());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if(AudioSlider.gameObject.activeSelf)
        {
            cooldown += Time.deltaTime;
        }
    }

    public async Task MoveCameraToGame() 
    {
        cam.transform.DOMove(gameCameraTransform.position, 1.5f).SetEase(Ease.InOutQuad);
        await cam.transform.DORotateQuaternion(gameCameraTransform.rotation, 1.5f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    }

    public void MoveCameraToMenu() 
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
        AudioMixerController.Instance.SetMasterVolume(value);
        // Only apply volume changes if enough time has passed (throttle while slider is open)
        // Allow immediate application when the slider is not visible (e.g. initial setup)
        if (!AudioSlider.gameObject.activeSelf || cooldown >= 0.1f)
        {
            AudioManager.Instance.Play(SoundType.PaperSlideSoft);
            cooldown = 0f;
        }
    }

    private void ToggleSlider()
    {
        AudioSlider.gameObject.SetActive(!AudioSlider.gameObject.activeSelf);
    }
}
