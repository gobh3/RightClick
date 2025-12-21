using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyOnlyOncePremiumButton : BuyOnlyOnce
{
    public bool activateChallengeAlreadyDoneCheck;
    public UnityEvent OnChllengeAlreadyDone;
    public SavedBool challlengeAlreadyDone;
    public Animator premimumAnimator;

    public override void Initialize()
    {
        base.Initialize();
        if ((Application.isEditor && activateChallengeAlreadyDoneCheck) || !Application.isEditor)
            if (challlengeAlreadyDone.GetValue())
                OnChllengeAlreadyDone?.Invoke();
        premimumAnimator.keepAnimatorStateOnDisable = true;
    }
    public void SaveChallengeDone()
    {
        challlengeAlreadyDone.SaveValue(true);
    }

}
