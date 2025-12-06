using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class InGamePass3ChBl : MonoBehaviour
{
    public SpotsBlockController spotsBlockController;

    public UnityEvent OnFinishedAnimation;

    public Animator animator;

    public float TIME_TO_START_DRAW;
    public float TIME_BETWEEN_SPOTS;
    private int currentSpotsOn = 0;

    private int unnormalizedProgress;
    public void Awake()
    {
        initChallenge();
    }
    public void SetUnnormalizedProgress(int d)
    {
        unnormalizedProgress = d;
    }

    private void initChallenge()
    {
        spotsBlockController.TurnOffAll();
    }

    public void DrawChallenge()
    {
        initChallenge();
        animator.SetTrigger("ComingBack");
    }

    public void DrawProgress()
    {
        StartCoroutine(animate());
    }
    IEnumerator animate()
    {
        // waits to start animate turning on spots
        yield return waitForSecondsAndLog(TIME_TO_START_DRAW);
        int spotsToDraw = unnormalizedProgress;
        if (currentSpotsOn < spotsToDraw)
            for (int i = 0; i < spotsToDraw; i++)
            {
                spotsBlockController.TurnOnSpot(i);
                yield return new WaitForSeconds(TIME_BETWEEN_SPOTS);
            }
        else
            spotsBlockController.TurnOffAll();
        /*
        if (!isCompleted)
            yield break;*/

        OnFinishedAnimation?.Invoke();

        // Starts disappearing animation
        animator.SetTrigger("GoAway");

        // waits for disappearing animation
        yield return waitForSecondsAndLog(animator.GetCurrentAnimatorStateInfo(0).length);
        /*
        // Update challenge data
        initChallenge();

        // Starts appearing animation
        animator.SetTrigger("ComingBack");
        */

    }
    IEnumerator waitForSecondsAndLog(float time)
    {
        //p//rint("start waiting for " + time + " sconds    ");
        yield return new WaitForSeconds(time);
        //p//rint("Finished waiting for " + time + " sconds");
    }
}
