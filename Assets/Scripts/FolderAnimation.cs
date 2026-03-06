using DG.Tweening;
using UnityEngine;

public class FolderAnimation : MonoBehaviour
{
    public static FolderAnimation Instance { get; private set; }

    [SerializeField] private Transform left, center, right;
    [SerializeField] private GameObject stamp;

    public float jumpPower = 0.5f;


    //priv
    private Animator animator;
    private Vector3 initialStampPosition;

    void Start()
    {
        Instance = this;

        animator = GetComponent<Animator>();
        initialStampPosition = stamp.transform.position;

        SlideIn();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");

            stamp.transform.DOJump(left.position, jumpPower, 1, 1f).SetEase(Ease.InOutSine).SetDelay(1).OnComplete(()=> stamp.transform.DOJump(initialStampPosition, jumpPower, 1, 1));
        }
    }

    public void SlideIn()
    {
        animator.SetTrigger("SlideIn");
    }

    public void SlideOut()
    {
        animator.SetTrigger("SlideOut");
    }

    public void MoveStamp(int index)
    {
        switch (index)
        {
            case 0:
                stamp.transform.DOJump(left.position, 1f, 1, 1f).SetDelay(1);
                break;
        }
    }
}
