using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeBlock : MonoBehaviour
{
    public Progressbar progressbar;
    public TextControllerLerp progressText;
    public TextMeshProUGUI destText;
    public Animator animator;
    public bool hasProgress;
    public int PROGRESS_FILL_TIME;

    public UnityEvent OnFinishedAnimation;

    private int dest;
    private int initialVal;
    private float progress;
    private bool isCompleted;

    public TextMeshProUGUI description;
    public void Initialize()
    {
        if (hasProgress)
        {
            progressbar.FILL_DURATION = PROGRESS_FILL_TIME;
            progressText.LERP_DURATION = PROGRESS_FILL_TIME;
        }
        initChallenge();
    }
    public void SetDest(int d)
    {
        dest = d;
    }

    public void SetInitialVal(int d)
    {
        initialVal = d;
    }

    public void SetIsCompleted(bool d)
    {
        isCompleted = d;
    }

    public void SetProgress(double p)
    {
        progress = (float)p;
    }

    private void initChallenge()
    {
        if (hasProgress)
        {
            destText.text = "~" + dest.ToString();
            //init progress
            progressbar.Reset(0f);
            progressText.ChangeValueContinuosly(0);
        }
    }

    public void DrawProgress()
    {
        //p/rint("dest: " + dest + " " + "initialVal: " + initialVal + " " + "progress: " + progress + " " + "isCompleted: " + isCompleted);
        if (hasProgress)
        {
            // Reset previus state
            progressbar.Reset(initialVal);
            progressText.ChangeValue(initialVal);

            // set update progress
            progressbar.OnFillChanged(progress);
            progressText.ChangeValueContinuosly(Mathf.RoundToInt((progress * 100)));

            if(isCompleted)
                StartCoroutine(animate());
        }
    }

    IEnumerator animate()
    {
        // waits for filling animation
        yield return waitForSecondsAndLog(PROGRESS_FILL_TIME);

        OnFinishedAnimation?.Invoke();

        // Starts disappearing animation
        animator.SetTrigger("GoAway");

        // waits for disappearing animation
        yield return waitForSecondsAndLog(animator.GetCurrentAnimatorStateInfo(0).length);

        // Update challenge data
        initChallenge();

        // Starts appearing animation
        animator.SetTrigger("ComingBack");


        IEnumerator waitForSecondsAndLog(float time)
        {
            //p/rint("start waiting for " + time + " sconds");
            yield return new WaitForSeconds(time);
            //p/rint("Finished waiting for " + time + " sconds");
        }
    }

    public void WriteDesc(int Limit)
    {
        description.text = "Get " + Limit + " points!";
    }

}
