using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

public class TabletController : MonoBehaviour
{
    public static TabletController Instance { get; private set; }

    public Vector3 onScreenPosition;
    public Vector3 offScreenPosition;

    private void Awake()
    {
        Instance = this;
    }

    public async Task MoveOnScreen()
    {
        await transform.DOMove(onScreenPosition, 1).SetEase(Ease.InOutSine).AsyncWaitForCompletion();
    }

    public void MoveOffScreen()
    {
        transform.DOMove(offScreenPosition, 1).SetEase(Ease.InOutSine);
    }
}
