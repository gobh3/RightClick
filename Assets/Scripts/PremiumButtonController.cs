using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PremiumButtonController : MonoBehaviour
{
    public PremiumButtonLogic logic;
    public Animator premiumButtonAnimator;
    public Button button;
    public float TIME_TO_MAKE_INTERACTABLE;
    public float TIME_TO_MAKE_UNINTERACTABLE;



    // Start is called before the first frame update
    void Start()
    {
        if (logic != null && logic.GetBalance() > 0)
        {
            UnlockAnimation();
        }
        /*else if (logic.IsLockedForever())
        {
            LockForever();
        }*/
        else
        {
            LockAnimation();
        }
    }

    public void LockAnimation()
    {

        StartCoroutine(_lockAnimation());
        //p/rint("Lock Premium Button");
    }

    private IEnumerator _lockAnimation()
    {
        premiumButtonAnimator.SetTrigger("Lock");
        yield return new WaitForSeconds(/*premiumButtonAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length*/TIME_TO_MAKE_UNINTERACTABLE);
        //p/rint("Unlcok Premium Button");
        button.interactable = false;
    }

    public void UnlockAnimation()
    {
        StartCoroutine(_unlockAnimation());
    }

    private IEnumerator _unlockAnimation()
    {
        premiumButtonAnimator.SetTrigger("Unlock");
        yield return new WaitForSeconds(/*premiumButtonAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length*/TIME_TO_MAKE_INTERACTABLE);
        //p/rint("Unlcok Premium Button");
        button.interactable = true;
    }

   /* private void LockForever()
    {


    }*/

}
