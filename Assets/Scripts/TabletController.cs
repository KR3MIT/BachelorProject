using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TabletController : MonoBehaviour
{
    public static TabletController Instance { get; private set; }
    public Volume ppVolume;
    private DepthOfField dof;

    public Vector3 onScreenPosition;
    public Vector3 offScreenPosition;

    private void Awake()
    {
        Instance = this;

        if (ppVolume != null)
        {
            ppVolume.profile.TryGet(out dof);
        }
    }

    public async Task MoveOnScreen()
    {
        SetDepthOfField(0.31f);
        await transform.DOMove(onScreenPosition, 1).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }

    public void MoveOffScreen()
    {
        SetDepthOfField(1.25f);
        transform.DOMove(offScreenPosition, 1).SetEase(Ease.InOutSine);
    }

    public async Task MoveOffScreen(bool hasTask)//whatever
    {
        SetDepthOfField(1.25f);
        await transform.DOMove(offScreenPosition, 1).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }

    public void TurnOnScreen()
    {

    }

    private void SetDepthOfField(float value)
    {
        if (dof != null)
        {
            dof.focusDistance.value = value;
        }
    }
}
