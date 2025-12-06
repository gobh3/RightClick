using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformOnEvents : MonoBehaviour
{
    private ObstaclesController obstaclesController;

    public void Initialize(ObstaclesController obstaclesController)
    {
        this.obstaclesController = obstaclesController;
    }

    public void InformOnSuccess()
    {
        if (obstaclesController != null)
            obstaclesController.OnCurrentSuccess?.Invoke();
    }

    public void InformOnFailure()
    {
        if (obstaclesController != null)
            obstaclesController.OnCurrentFailure?.Invoke();
    }

    //public void InformOnOnMissed(BallMovement b)
    //{
    //    if (obstaclesController != null)
    //        obstaclesController.OnCurrentMissed?.Invoke(b.transform.parent.gameObject);
    //}

}
