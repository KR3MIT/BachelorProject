using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIApprovalRating : MonoBehaviour
{
    public static UIApprovalRating Instance { get; private set; }

    // UI elements
    [SerializeField] private float approvalRating;
    [SerializeField] private Image approvalBarFill;
    [SerializeField] private Image approvalBarFade;
    [SerializeField] private Color approvalBarFadeColorPos;
    [SerializeField] private Color approvalBarFadeColorNeg;

    // Coroutine variables
    private bool addRating = true;
    private float fadeWaitTimer = 2f;
    private float fadeDuration = 0.5f;

    // Debug
    [SerializeField] private bool debug;
    [SerializeField] private int fadeRuns;

    void Start()
    {
        Instance = this;

        approvalBarFill.fillAmount = approvalRating;
        approvalBarFade.fillAmount = 0f;
    }


    void Update()
    {
        
    }

    //returns approvalrating in normalizee value
    public float GetApprovalRating()
    {
        return approvalRating;
    }


    private float ConvertToPercent (float amount)
    {
        float percent = amount / 100f;
        return percent;
    }
    // addapproval (set barfade current barfill + added approval rating, fill approvalbar over a lerp to barfade value)
    public void AddApproval(float amount)
    {
        
        amount = ConvertToPercent(amount);
        if (debug)
        {
            Debug.Log("AddApproval Amount: " + amount);
        }
        float newRating = Mathf.Clamp(approvalRating + amount, 0f, 1f);
        approvalBarFill.fillAmount = approvalRating;
        approvalRating = newRating;
        approvalBarFade.fillAmount = approvalRating;
        StartCoroutine(FadeBar(addRating));
    }

    // removeapproval (set barfade to current barfill, barfill set to new approval rating, fade out barfade towards barfill)
    public void RemoveApproval(float amount)
    {
        
        amount = ConvertToPercent(amount);
        if (debug)
        {
            Debug.Log("RemoveApprovalAmount: " + amount);
        }
        float newRating = Mathf.Clamp(approvalRating - amount, 0f, 1f);
        approvalBarFade.fillAmount = approvalRating;
        approvalRating = newRating;
        approvalBarFill.fillAmount = approvalRating;
        StartCoroutine(FadeBar(!addRating));
    }

    IEnumerator FadeBar(bool isPositive)
    {
        approvalBarFade.color = isPositive ? approvalBarFadeColorPos : approvalBarFadeColorNeg;
        float startPosition = isPositive ? approvalBarFill.fillAmount : approvalBarFade.fillAmount;
        float timer = 0f;
        yield return new WaitForSeconds(fadeWaitTimer);

        if (isPositive) {
            AudioManager.Instance.Play(SoundType.ApprovalPositive);
        }
        else
        {
            AudioManager.Instance.Play(SoundType.ApprovalNegative);
        }
        // Loop over frames until fadeDuration elapses, updating t in [0,1]
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            if (debug)
            {
                Debug.Log("Fading: " + timer + " Amount of Runs: " + fadeRuns);
                fadeRuns++;
            }

            if (isPositive)
            {
                // move barfill towards approvalRating over normalized t
                approvalBarFill.fillAmount = Mathf.Lerp(startPosition, approvalRating, t);
            }
            else
            {
                // move barfade towards approvalRating over normalized t
                approvalBarFade.fillAmount = Mathf.Lerp(startPosition, approvalRating, t);
            }

            yield return null;
        }

        // Ensure final values are exact at the end of the fade
        if (isPositive)
        {
            approvalBarFill.fillAmount = approvalRating;
            approvalBarFade.fillAmount = 0f;
        }
        else
        {
            approvalBarFade.fillAmount = approvalBarFill.fillAmount;
        }
    }
}
