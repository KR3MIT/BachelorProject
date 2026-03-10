using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class FolderAnimation : MonoBehaviour
{
    public static FolderAnimation Instance { get; private set; }

    [SerializeField] private Transform left, center, right;
    [SerializeField] private GameObject stamp;
    [SerializeField] private List<GameObject> selectionStamps;

    public float jumpPower = 0.5f;


    //priv
    private Animator animator;
    private Vector3 initialStampPosition;

    void Start()
    {
        Instance = this;

        animator = GetComponent<Animator>();
        initialStampPosition = stamp.transform.position;
    }

    public void SlideIn()
    {
        animator.SetTrigger("SlideIn");
    }

    public async void SlideOut()
    {
        animator.SetTrigger("SlideOut");
        await Task.Delay(1000);
        foreach (var stamp in selectionStamps) { stamp.SetActive(false); }
    }

    public async Task MoveStamp(int index)
    {
        switch (index)
        {
            case 0:
                await StampJumpIn(left.position);
                break;
            case 1:
                await StampJumpIn(center.position);
                break;
            case 2:
                await StampJumpIn(right.position);
                break;
        }

        async Task StampJumpIn(Vector3 position)
        {
            await stamp.transform.DOJump(position, jumpPower, 1, 1f).SetEase(Ease.InOutSine).SetDelay(1).OnComplete(()=> selectionStamps[index].SetActive(true)).AsyncWaitForCompletion();

            await StampJumpOut();
        }
        async Task StampJumpOut()
        {
            await stamp.transform.DOJump(initialStampPosition, jumpPower, 1, 1).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
        }
    }
}
