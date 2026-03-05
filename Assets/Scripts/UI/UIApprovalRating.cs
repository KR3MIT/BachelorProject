using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIApprovalRating : MonoBehaviour
{
    public float approvalRating { get; set; }
    public Image approvalBarFill;
    public Image approvalBarFade;


    void Start()
    {
        approvalRating = 0.5f;
        approvalBarFill.fillAmount = approvalRating;
        approvalBarFade.enabled = false;
    }


    void Update()
    {
        
    }

    /* 
     updatefill bar
    removeapproval (set barfade to current barfill, barfill set to new approval rating, fade out barfade)
    addapproval (set barfade current barfill + added approval rating, fill approvalbar over a lerp to barfade value)

     
     
     */

    // fix this to update the bar fill based on the approval rating
    void UpdateBarFill()
    {

    }

    IEnumerator FadeBar()
    {
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        Color initialColor = approvalBarFade.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            approvalBarFade.color = Color.Lerp(initialColor, targetColor, t);
            yield return null;
        }
        approvalBarFade.color = targetColor;
    }

}
