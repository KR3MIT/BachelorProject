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
    }


    void Update()
    {
        
    }

    // fix this to update the bar fill based on the approval rating
    void UpdateBarFill()
    {

    }

}
