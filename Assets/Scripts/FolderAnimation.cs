using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;

public class FolderAnimation : MonoBehaviour
{
    public static FolderAnimation Instance { get; private set; }

    public MeshRenderer folderRenderer;

    public Material twoOptionMaterial, threeOptionMaterial;

    [SerializeField] private GameObject stamp;
    [SerializeField] private List<GameObject> selectionStamps;
    [SerializeField] private List<Transform> stampPositions;

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

    public async void SlideIn(bool twoOptions, int delay = 1000)
    {
        //await Task.Delay(delay);
        await Awaitable.WaitForSecondsAsync(delay / 1000);
        animator.SetTrigger("SlideIn");
        AudioManager.Instance.Play(SoundType.PaperFlutter);
        if (twoOptions) 
        {
            //Debug.Log("Using two option material");
            folderRenderer.material = twoOptionMaterial;
        }
        else
        {
            //Debug.Log("Using three option material");
            folderRenderer.material = threeOptionMaterial;
        }
    }

    public async void SlideOut()
    {
        animator.SetTrigger("SlideOut");
        await Awaitable.WaitForSecondsAsync(1);
        //await Task.Delay(1000);
        foreach (var stamp in selectionStamps) { stamp.SetActive(false); }
    }

    public async Task MoveStamp(int index)
    {
        await StampJumpIn(stampPositions[index].transform.position);

        async Task StampJumpIn(Vector3 position)
        {
            await stamp.transform.DOJump(position, jumpPower, 1, 1f).SetEase(Ease.InOutSine).SetDelay(1).OnComplete(()=> selectionStamps[index].SetActive(true)).AsyncWaitForCompletion();

            AudioManager.Instance.Play(SoundType.Stamp);

            await StampJumpOut();
        }
        async Task StampJumpOut()
        {
            await stamp.transform.DOJump(initialStampPosition, jumpPower, 1, 1).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
        }
    }
}
