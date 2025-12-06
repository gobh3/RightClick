using System;
using UnityEngine;

public abstract class AObstacle : MonoBehaviour
{
    public Rigidbody2D rigidBody2d;
    public int BallsLeftCount;

    protected ObstaclesController obstaclesController;

    public void AddVelocity(Vector2 veloity)
    {
        rigidBody2d.linearVelocity += veloity;
    }
    
    public bool CanGetMoreInput()
    {
        if (BallsLeftCount == 0)
            return false;
        return true;
    }

    public void SetObstaclesController(ObstaclesController _obstaclesController)
    {
        obstaclesController = _obstaclesController;
    }
    public abstract void ResetObstacle(); 
    public abstract void SetInput(InputValue input);
    public abstract void SetVelocity(Vector2 veloity);
    public abstract void Destroy();

    public abstract void OnBallCollision(ObstaclePart part,bool isGoodCollision);
}
