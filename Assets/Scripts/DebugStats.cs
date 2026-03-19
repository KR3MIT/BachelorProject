using TMPro;
using UnityEngine;

public class DebugStats : MonoBehaviour
{
    private TMP_Text text;
    private float minFps = float.MaxValue;
    private float maxFps = float.MinValue;
    private float resetTimer = 0f;
    private const float RESET_INTERVAL = 30f;
    private float sessionTime = 0f;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float currentFps = 1f / Time.unscaledDeltaTime;
        
        if (minFps == float.MaxValue)
            minFps = currentFps;
        else
            minFps = Mathf.Min(minFps, currentFps);
        
        maxFps = Mathf.Max(maxFps, currentFps);

        resetTimer += Time.unscaledDeltaTime;
        if (resetTimer >= RESET_INTERVAL)
        {
            minFps = float.MaxValue;
            maxFps = float.MinValue;
            resetTimer = 0f;
        }

        sessionTime += Time.unscaledDeltaTime;

        float usedMemory = System.GC.GetTotalMemory(false) / 1024f / 1024f;
        float totalMemory = SystemInfo.systemMemorySize;
        string graphicsDeviceName = SystemInfo.graphicsDeviceName;
        bool hardwareAcceleration = SystemInfo.graphicsUVStartsAtTop;

        int minutes = (int)(sessionTime / 60f);
        int seconds = (int)(sessionTime % 60f);

        text.text = $"FPS: {(int)currentFps} | " +
                    $"Min: {(int)minFps} " +
                    $"Max: {(int)maxFps}\n" +
                    $"Resolution: {Screen.width}x{Screen.height}\n" +
                    $"Memory: {usedMemory:F2} / {totalMemory:F2} MB\n" +
                    $"Graphics: {graphicsDeviceName}\n" +
                    $"Hardware Acceleration: {(hardwareAcceleration ? "Enabled" : "Disabled")}\n" +
                    $"Session Time: {minutes:D2}:{seconds:D2}";
    }
}
