using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class MultiObstacle : AObstacle
{
    public Material ShinyMaterial;
    public Material TransparentShinyMaterial;
    public ObstaclePartsSet[] Set;
    public ClassicBall ClassicBallPf;
    public InversedBall InversedBallPf;
    public int HowManyGoodObsPerSet;
    //[HideInInspector]
    //public Queue<ABall> balls;
    [HideInInspector]
    public ABall currentBall;
    private List<ABall> oldBalls;
    public override void ResetObstacle()
    {
        for (int i = 0; i < Set.Length; i++)
        {
            Set[i].isLive = true;
            List<int> goodOnes = MathUtil.GetUniqueRandomNumbers(0, Set[i].Parts.Length - 1, HowManyGoodObsPerSet);
            List<bool> partsITemplate = new List<bool>();
            for (int j = 0; j < Set[i].Parts.Length; j++)
            {
                partsITemplate.Add(false);
            }
            for (int j = 0; j < goodOnes.Count; j++)
            {
                int indexToSetTrue = goodOnes[j];
                partsITemplate[indexToSetTrue] = true;
            }
            for (int k = 0; k < partsITemplate.Count; k++)
            {
                if (partsITemplate[k])
                {
                    Set[i].Parts[k].sp.material = ShinyMaterial;
                    Set[i].Parts[k].isGood = true;
                }
                else
                {
                    Set[i].Parts[k].sp.material = TransparentShinyMaterial;
                    Set[i].Parts[k].isGood = false;
                }
            }
        }

        currentBall = createRandmoBall();
        currentBall.SetFather(this);

        //Reset old balls list
        if (oldBalls == null)
            oldBalls = new List<ABall>();
        else oldBalls.Clear();
    }

    public override void SetInput(InputValue input)
    {
        if (currentBall != null)
        {
            currentBall.ApplyInput(input);
            BallsLeftCount--;
            if (BallsLeftCount > 0)
            {
                oldBalls.Add(currentBall);
                //creating another ball
                currentBall = createRandmoBall();
            }
            currentBall.SetFather(this);
            currentBall.SetYVelocity(rigidBody2d.linearVelocityY);
        }

    }

    public override void SetVelocity(Vector2 veloity)
    {
        rigidBody2d.linearVelocity = veloity;
        if (currentBall != null)
        {
            currentBall.SetYVelocity(veloity.y);
        }
    }

    public override void Destroy()
    {
        Destroy(this.gameObject);
        Destroy(currentBall.gameObject);
        foreach (ABall b in oldBalls)
            Destroy(b.gameObject);
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
        // Killing the set of parts of the shooted ObstaclePart
        for (int i = 0; i < Set.Length; i++)
        {
            for (int j = 0; j < Set[i].Parts.Length; j++)
            {
                if (Set[i].Parts[j] == part)
                    foreach (ObstaclePart p in Set[i].Parts)
                    {
                        p.KillOnGame();
                        p.SetIsLive(false);
                    }
            }
        }
    }

    private ABall createRandmoBall()
    {
        BallType ballType = MathUtil.GetRandomEnumValue<BallType>();
        switch (ballType)
        {
            case BallType.ClassicBall:
                return Instantiate(ClassicBallPf, transform.position, Quaternion.identity);
                break;
            case BallType.InversedBall:
                return Instantiate(InversedBallPf, transform.position, Quaternion.identity);
                break;
            default:
                return Instantiate(ClassicBallPf, transform.position, Quaternion.identity);
        }

    }

    [System.Serializable]
    public struct ObstaclePartsSet
    {
        public ObstaclePart[] Parts;
        [HideInInspector]
        public bool isLive;
    }
}
