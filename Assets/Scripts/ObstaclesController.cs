using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;


//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;
public class ObstaclesController : MonoBehaviour, ITimerClient
{
    public SettingsManager SettingsManager;
    //public ObstaclesPool Pool;

    public float INITIAL_HEIGHT;
    public float DEADLINE_HEIGHT;
    //events
    public UnityEvent<float> OnSpeedChanged;
    public UnityEvent OnCurrentSuccess;
    public UnityEvent OnCurrentFailure;

    public GatePair pfGatesPair;

    // ##### NEW OBSTACLES VERSION
    public ObstacleConfigManager ObsTypesManager;
    public ClassicObstacle ClassicObstaclePf;
    public CircularObstacle CircularObstaclePf;
    public MultiObstacle DoubleObstacleClassicPf;
    public ClassicBall ClassicBallPf;
    public InversedBall InversedBallPf;
    private Queue<AObstacle> activeObstacles = new Queue<AObstacle>();
    private List<AObstacle> allObstaclesList = new List<AObstacle>();
    // ###########################

    /*//queue of the active obstacles - not yet shoot their ball.
    private Queue<GatePair> activeGates = new Queue<GatePair>();
    //list of all obstacles on scene
    private List<GatePair> allGatesList = new List<GatePair>();*/


    //locals
    private float createNewObsTimer;
    private bool createObs;
    private float minimalCreateNewObsTimer;

    private float currentSpace;
    private LerpRecord<float?> lerpRecord = new LerpRecord<float?>();
    private float speedBeforeStop = -1f;

    public void FirstTimeInitialize()
    {
        lerpRecord.OnCurrentChanged.AddListener(onSpeedChanged);
        minimalCreateNewObsTimer = calcTimer(SettingsManager.settings.InitialSpeed, SettingsManager.settings.InitialSpace);
    }

    public void InitializeForNewGame()
    {
        currentSpace = SettingsManager.settings.InitialSpace;
        createNewObsTimer = calcTimer(SettingsManager.settings.InitialSpeed, currentSpace);
        /*activeGates.Clear();
        bool isActive;
        foreach (Transform c in transform)
        {
            //Destroy(c.gameObject);
            
            isActive = c.gameObject.activeInHierarchy;
            if (isActive)
            {
                GatePair gp = c.GetComponent<GatePair>();
                gp.Release(Pool.Pool);
            }
        }*/
        foreach (AObstacle obstacle in allObstaclesList)
        {
            obstacle.Destroy();
        }
        allObstaclesList.Clear();
        activeObstacles.Clear();

        createObs = true;
        lerpRecord.JumpTo(SettingsManager.settings.InitialSpeed);
        StartCoroutine(generateObs());
    }

    public void StopCreatingObs()
    {
        createObs = false;
    }

    public void ClearObs()
    {
        foreach (AObstacle obstacle in allObstaclesList)
        {
            obstacle.Destroy();
        }
        allObstaclesList.Clear();
        activeObstacles.Clear();
    }

    public void ContinueCreatingObs()
    {
        createObs = true;
        StartCoroutine(waitAndStartGenerating());
    }

    private IEnumerator waitAndStartGenerating()
    {
        yield return new WaitForSeconds(createNewObsTimer);
        yield return generateObs();
    }

    public void ReleaseOb(GatePair ob)
    {
        /*Destroy(ob.gameObject);
        ob.Release(Pool.Pool);
        if (activeGates.TryPeek(out GatePair firstOb))
            if (firstOb == ob)
                activeGates.Dequeue();*/
    }

    public void OnNewLevel(LevelData level)
    {
        lerpRecord.To = SettingsManager.settings.InitialSpeed + level.speed;
        currentSpace = Math.Max(SettingsManager.settings.InitialSpace + SettingsManager.settings.SpaceIncrease * level.levelNumber, SettingsManager.settings.MinSpace);
        //createNewObsTimer = calcTimer(lerpRecord.To.Value, currentSpace);
        // the game is now in pause -> we want the continue function to 
        // change back the speed from 0f. Anyway we need to update the speed to return to.
        if (!createObs)
        {
            speedBeforeStop = lerpRecord.To.Value;
        }
        else
        {
            changeSpeedSmoothly(lerpRecord.Current.Value);
        }
        //##### NEW OBSTACLES VERSION
        ObsTypesManager.SetConfig(level.levelNumber);
        //###########################
    }


    private IEnumerator generateObs()
    {
        while (createObs)
        {
            /*GatePair newGp = createObstacle();
            Vector2 v = Vector2.down * lerpRecord.Current.Value;
            newGp.AddVelocity(v);*/
            AObstacle newObs = null;
            ObstacleType type = ObsTypesManager.GetRandObsType();
            switch (type)
            {
                case ObstacleType.ClassicObstacle:
                    newObs = Instantiate(ClassicObstaclePf,
                                           new Vector3(0, INITIAL_HEIGHT),
                                           Quaternion.identity, transform);
                    break;
                case ObstacleType.CircularObstacle:
                    newObs = Instantiate(CircularObstaclePf,
                                           new Vector3(0, INITIAL_HEIGHT),
                                           Quaternion.identity, transform);
                    break;
                case ObstacleType.DoubleObstacle:
                    newObs = Instantiate(DoubleObstacleClassicPf,
                                           new Vector3(0, INITIAL_HEIGHT),
                                           Quaternion.identity, transform);
                    break;
                default:
                    newObs = Instantiate(ClassicObstaclePf,
                                           new Vector3(0, INITIAL_HEIGHT),
                                           Quaternion.identity, transform);
                    break;
            }

            newObs.SetObstaclesController(this);
            newObs.ResetObstacle();
            Vector2 v = Vector2.down * lerpRecord.Current.Value;
            newObs.SetVelocity(v);
            activeObstacles.Enqueue(newObs);
            allObstaclesList.Add(newObs);
            if (!createObs) yield break;
            yield return new WaitForSeconds(setDeviationTofloat(createNewObsTimer,
                                                                SettingsManager.settings.MaxUpperDeviation,
                                                                SettingsManager.settings.MaxLowerDeviation));
        }
    }
    public void SendInputToObstacle(InputValue input)
    {
        if (activeObstacles.TryPeek(out AObstacle ob))
        {
            ob.SetInput(input);
            if (ob.BallsLeftCount == 0)
                activeObstacles.Dequeue();
        }
    }
    public void ShootRight()
    {
        SendInputToObstacle(InputValue.Right);
    }

    public void ShootLeft()
    {
        SendInputToObstacle(InputValue.Left);
    }

    public bool ShootCorrectly()
    {
        if (!activeObstacles.TryPeek(out AObstacle gp) || gp is not ClassicObstacle)
            return false;
        (gp as ClassicObstacle).SetCorrectInput();
        activeObstacles.Dequeue();
        return true;
    }
    public AObstacle PeekFirst()
    {
        if (activeObstacles.TryPeek(out AObstacle gp))
            return gp;
        else
            return null;
    }

    public UnityEvent<AObstacle> OnMissed;
    public UnityEvent<AObstacle> OnNotMissed;
    public void Update()
    {
        if (Time.frameCount % 5 == 0)
        {
            if (allObstaclesList.Count > 0)
            {
                AObstacle o = allObstaclesList[0];
                if (o.transform.position.y < DEADLINE_HEIGHT)
                {
                    if (o.BallsLeftCount > 0)
                    {
                        activeObstacles.Dequeue();
                        OnMissed?.Invoke(o);
                    }
                    else
                    {
                        OnNotMissed?.Invoke(o);
                    }
                    allObstaclesList.Remove(o);
                    o.Destroy();
                }

            }

        }
    }



    // --------------------
    private float setDeviationTofloat(float num, float upperMaxDeviation, float bottomMaxDeviation)
    {
        float d = UnityEngine.Random.Range(-bottomMaxDeviation, upperMaxDeviation);
        return num + num * d;
    }

    private float calcTimer(float obstaclesSpeedFactor, float obstaclesSpace)
    {
        return obstaclesSpace / Math.Abs(obstaclesSpeedFactor);
    }
    private void onSpeedChanged(float? speed)
    {
        OnSpeedChanged?.Invoke((float)Math.Round((double)speed.Value, 2));
    }
    public void changeSpeedSmoothly(float f)
    {
        AlarmClock.GetInstance().RegisterAndReplace(SettingsManager.settings.LerpDuration, this);
    }
    public void DuringTimer(float timeElapsed)
    {
        lerpRecord.UpdateCurrent(timeElapsed / SettingsManager.settings.LerpDuration, MathUtil.Lerp);
        //change speed of all obstacles to lerpRecord.Current:
        for (int i = 0; i < allObstaclesList.Count; i++)
        {
            //PROBLEM CHANGE SPEED OF ONLY ACTIVE OBSCACLES SOMETIME _ NULLLLLLL
            if (allObstaclesList[i] == null)
                allObstaclesList.RemoveAt(i);
            else
                allObstaclesList[i].SetVelocity(Vector2.down * lerpRecord.Current.Value);
        }
        createNewObsTimer = Math.Min(calcTimer(lerpRecord.Current.Value, currentSpace), minimalCreateNewObsTimer);
    }
    public void Stop()
    {
        speedBeforeStop = lerpRecord.Current.Value;
        lerpRecord.To = SettingsManager.settings.StopSpeed;
        changeSpeedSmoothly(SettingsManager.settings.StopSpeed);
    }

    public void Continue()
    {
        lerpRecord.To = speedBeforeStop;
        changeSpeedSmoothly(speedBeforeStop);
    }

}

/*
public void ShootRight()
{
    if (activeGates.TryDequeue(out GatePair newObstacle))
    {
        newObstacle.OnShootRight?.Invoke();
    }
}

public void ShootLeft()
{
    if (activeGates.TryDequeue(out GatePair newObstacle))
    {
        newObstacle.OnShootLeft?.Invoke();
    }
}

public GatePair PeekFirst()
{
    if (activeGates.TryPeek(out GatePair gp))
        return gp;
    else
        return null;
}

private GatePair createObstacle()
{
    //GatePair newObstacle = Instantiate(pfGatesPair, new Vector3(0, INITIAL_HEIGHT), Quaternion.identity, transform);
    GatePair newObstacle = Pool.Pool.Get();
    newObstacle.transform.SetPositionAndRotation(new Vector3(0, INITIAL_HEIGHT),Quaternion.identity);
    newObstacle.transform.SetParent(transform);
    newObstacle.GetComponent<InformOnEvents>().Initialize(this);
    if (newObstacle.gameObject == null)
        print("New Obs is null");
    activeGates.Enqueue(newObstacle);
    allGatesList.Add(newObstacle);
    return newObstacle;
}*/
/*
public void DuringTimer(float timeElapsed)
{
    lerpRecord.UpdateCurrent(timeElapsed / SettingsManager.settings.LerpDuration, MathUtil.Lerp);
    //change speed of all obstacles to lerpRecord.Current:
    for (int i = 0; i < allGatesList.Count; i++)
    {
        //PROBLEM CHANGE SPEED OF ONLY ACTIVE OBSCACLES SOMETIME _ NULLLLLLL
        if (allGatesList[i] == null)
            allGatesList.RemoveAt(i);
        else
            allGatesList[i].SetYVelocity(-1 * lerpRecord.Current.Value);
    }

}*/