using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

public class MainMenuScript : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private Transform gameCameraTransform;
    [SerializeField] private Button startButton;
    [SerializeField] private List<GameObject> popUpObjects;
    
    private GameObject cam;

    private void Awake()
    {
        foreach (var obj in popUpObjects)
            obj.SetActive(false);

        cam = Camera.main.gameObject;
    }

    public void Start()
    {

    }

    public void Show(Action onStartGame)
    {
        gameObject.SetActive(true);

        //AudioSlider.gameObject.SetActive(false);

        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(() => onStartGame?.Invoke());
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public async Task MoveCameraToGame()
    {
        cam.transform.DOMove(gameCameraTransform.position, 1.5f).SetEase(Ease.InOutQuad);
        await cam.transform.DORotateQuaternion(gameCameraTransform.rotation, 1.5f).SetEase(Ease.InOutQuad).AsyncWaitForCompletion();
    }

    public void MoveCameraToMenu() 
    {

    }

    public void PopUp()
    {
        Debug.Log("1");
        StartCoroutine(PopUpDelay());
    }
    IEnumerator PopUpDelay()
    {
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        foreach (var obj in popUpObjects)
            obj.SetActive(true);
        Debug.Log("3");
    }
}
