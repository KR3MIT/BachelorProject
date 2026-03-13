using UnityEngine;

public class ViewpointView : MonoBehaviour
{
    [SerializeField] private RectTransform option1;
    [SerializeField] private RectTransform option2;

    private RectTransform initialPos1;
    private RectTransform initialPos2;

    private void Awake()
    {
        initialPos1 = option1;
        initialPos2 = option2;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnabled()
    {
        
    }
}
