using UnityEngine;

public class CircularObstacle : AObstacle
{
    public Material ShinyMaterial;
    public Material TransparentShinyMaterial;
    public ObstaclePart part1;
    public ObstaclePart part2;
    public ClassicBall ClassicBallPf;
    public InversedBall InversedBallPf;
    [HideInInspector]
    public ABall ball;
    public int ANGULAR_VELOCITY;

    public void Awake()
    {
        rigidBody2d.angularVelocity = ANGULAR_VELOCITY;
    }

    public override void ResetObstacle()
    {
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            part1.isGood = false;
            part1.sp.material = TransparentShinyMaterial;
            part2.isGood = true;
            part2.sp.material = ShinyMaterial;
        }
        else
        {
            part1.isGood = true;
            part1.sp.material = ShinyMaterial;
            part2.isGood = false;
            part2.sp.material = TransparentShinyMaterial;
        }

        a = Random.Range(0, 2);
        if (a == 0)
        {
            //TODO - add more parameters
            ball = Instantiate(InversedBallPf, transform.position, Quaternion.identity);
        }
        else
        {
            ball = Instantiate(ClassicBallPf, transform.position, Quaternion.identity);
        }
        //ball.obstaclesController = obstaclesController;
        ball.SetFather(this);
    }

    public override void SetInput(InputValue input)
    {
        ball.ApplyInput(input);
        BallsLeftCount--;
    }

    public void SetCorrectInput()
    {
        InputValue correctDir;
        if (part1.isGood) correctDir = InputValue.Right;
        else correctDir = InputValue.Left;

        if (ball is ClassicBall) ball.ApplyInput(correctDir);
        else
        {
            if (correctDir == InputValue.Right)
                ball.ApplyInput(InputValue.Left);
            else ball.ApplyInput(InputValue.Right);
        }
        BallsLeftCount--;
    }

    public override void SetVelocity(Vector2 veloity)
    {
        rigidBody2d.linearVelocity = veloity;
        ball.SetYVelocity(veloity.y);
    }

    public override void Destroy()
    {
        Destroy(ball.gameObject);
        Destroy(this.gameObject);
    }

    public override void OnBallCollision(ObstaclePart part, bool isGoodCollision)
    {
        switch (isGoodCollision)
        {
            case true:
                obstaclesController.OnCurrentSuccess?.Invoke();
                break;
            case false:
                obstaclesController.OnCurrentFailure?.Invoke();
                break;
        }
    }

    public void SetImmediateGoodExplosion()
    {
        part1.KillOnGame();
        part2.KillOnGame();
        ball.KillOnGame(true);
        obstaclesController.OnCurrentSuccess?.Invoke();
    }
}
