using UnityEngine;

public class ClassicObstacle : AObstacle
{
    public Material ShinyMaterial;
    public Material TransparentShinyMaterial;
    public ObstaclePart rightGate;
    public ObstaclePart leftGate;
    public ClassicBall ClassicBallPf;
    public InversedBall InversedBallPf;
    [HideInInspector]
    public ABall ball;

    public override void ResetObstacle()
    {
        int a = Random.Range(0, 2);
        if (a == 0)
        {
            rightGate.isGood = false;
            rightGate.sp.material = TransparentShinyMaterial;
            leftGate.isGood = true;
            leftGate.sp.material = ShinyMaterial;
        }
        else
        {
            rightGate.isGood = true;
            rightGate.sp.material = ShinyMaterial;
            leftGate.isGood = false;
            leftGate.sp.material = TransparentShinyMaterial;
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
        if (rightGate.isGood) correctDir = InputValue.Right;
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


}
