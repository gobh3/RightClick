using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContinueGameEffect : MonoBehaviour
{
    public SettingsManager settingsManager;
    public UnityEvent OnFinishedEffect;
    public ObstaclesController obstaclesController;
    //public float DELAY_TIME_BEFORE;
    //public float DELAY_TIME_AFTER;
    //public float TIME_BETWEEN_GATES;
    //public int HOW_MANY_GATES_TO_KILL;
    public void ContinueGame()
    {
        StartCoroutine(continueGame());
    }

    private IEnumerator continueGame()
    {
        obstaclesController.ClearObs();

        yield return new WaitForSeconds(settingsManager.effectsSettings.DelayTimeBefore);

        for (int i = 0; i < settingsManager.effectsSettings.HowManyGatesToKill; i++)
        {
            /*GatePair gp = obstaclesController.PeekFirst();
            if (gp == null)
                i = settingsManager.effectsSettings.HowManyGatesToKill;
            else
            {
                GatePairData gpData = gp.GetComponent<GatePairData>();
                if (gpData != null)
                {
                    KeySide correctSide = gpData.GetRightSideToShoot();
                    if (correctSide == KeySide.Left)
                        obstaclesController.ShootLeft();
                    else obstaclesController.ShootRight();
                }
                yield return new WaitForSeconds(settingsManager.effectsSettings.TimeBetweenGates);
            }
            AObstacle ob = obstaclesController.PeekFirst();
            if (ob == null)
                i = settingsManager.effectsSettings.HowManyGatesToKill
            else;
            {
                if(obstaclesController.ShootCorrectly());
                yield return new WaitForSeconds(settingsManager.effectsSettings.TimeBetweenGates);
            }*/
            obstaclesController.ShootCorrectly();
            yield return new WaitForSeconds(settingsManager.effectsSettings.TimeBetweenGates);
        }

        yield return new WaitForSeconds(settingsManager.effectsSettings.DelayTimeAfter);
        OnFinishedEffect?.Invoke();
    }
}
