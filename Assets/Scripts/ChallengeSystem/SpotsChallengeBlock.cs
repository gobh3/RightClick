using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class SpotsChallengeBlock : MonoBehaviour
{
    public SpotsBlockController spotsBlockController;

    public UnityEvent OnFinishedAnimation;

    public Animator animator;
    public TextMeshProUGUI description;

    public float TIME_TO_START_DRAW;
    public float TIME_BETWEEN_SPOTS;
    private int currentSpotsOn = 0;

    private bool isCompleted;
    private int unnormalizedProgress;

    public void Initialize()
    {
        initChallenge();
    }
    public void SetUnnormalizedProgress(int d)
    {
        unnormalizedProgress = d;
    }

    public void SetIsCompleted(bool d)
    {
        isCompleted = d;
    }

    private void initChallenge()
    {
        spotsBlockController.TurnOffAll();
    }

    public void DrawProgress()
    {
        //p/rint("SpotsChallengeBlock: DrawProgress: unnormalized progress: " + unnormalizedProgress + " " + "isCompleted: " + isCompleted);
        StartCoroutine(animate());
    }
    IEnumerator animate()
    {
        // waits to start animate turning on spots
        yield return waitForSecondsAndLog(TIME_TO_START_DRAW);
        int spotsToDraw = unnormalizedProgress;
        //p/rint("draw " + spotsToDraw + " spots");
        if (currentSpotsOn < spotsToDraw)
            for (int i = 0; i < spotsToDraw; i++)
            {
                spotsBlockController.TurnOnSpot(i);
                yield return new WaitForSeconds(TIME_BETWEEN_SPOTS);
            }
        else
            spotsBlockController.TurnOffAll();

        if (!isCompleted)
            yield break;

        OnFinishedAnimation?.Invoke();

        // Starts disappearing animation
        animator.SetTrigger("GoAway");

        // waits for disappearing animation
        yield return waitForSecondsAndLog(animator.GetCurrentAnimatorStateInfo(0).length);

        // Update challenge data
        initChallenge();

        // Starts appearing animation
        animator.SetTrigger("ComingBack");

        // waits for ComingBack animation
        yield return waitForSecondsAndLog(animator.GetCurrentAnimatorStateInfo(0).length);

        isCompleted = false;

    }
    IEnumerator waitForSecondsAndLog(float time)
    {
        //p/rint("start waiting for " + time + " sconds");
        yield return new WaitForSeconds(time);
        //p/rint("Finished waiting for " + time + " sconds");
    }

    public void WriteDesc(int Limit)
    {
        description.text = "Reach " + Limit + " points three times in a row!";
    }
}
